namespace Note.Services
{
    using API.Common.Helpers;
    using API.Common.Messages;
    using AutoMapper;
    using Note.API.Common.Extensions;
    using Note.Services.Contracts;
    using Repository.Data;
    using Repository.Data.Entities;
    using System;
    using System.Linq;
    using DC = API.DataContracts;

    public class UserService : IUserService
    {
        private NoteDataContext _context = null;
        private readonly IPropertyMappingService _propertyMappingService;

        public UserService(NoteDataContext context, IMapper mapper, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        #region Utilities
        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        #endregion Utilities

        #region ServiceMethods

        public Tuple<User, bool, string> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Tuple.Create(new User(), false, AlertMessages.Username_Password_Empty);
            }

            var user = _context.User.SingleOrDefault(x => x.Username.Equals(username) && x.IsActive);

            // check if username exists
            if (user == null)
            {
                return Tuple.Create(new User(), false, AlertMessages.User_Doesnot_Exits);
            }

            // check if password is correct
            //string[] pwdWithKey = password.Split(new string[] { "$$" }, StringSplitOptions.None);
            //var decyptedPwd = AESEncrytDecry.DecryptStringAES(pwdWithKey[0], pwdWithKey[1]);

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return Tuple.Create(new User(), false, AlertMessages.Password_Incorrect);

            user.LastLoginDateUTC = DateTime.UtcNow;
            _context.User.Update(user);
            _context.SaveChanges();

            // authentication successful
            return Tuple.Create(user, true, AlertMessages.Authenticate_Success);
        }

        public Tuple<PagedList<User>, bool, string> GetAll(UserResourceParameters pageparams)
        {
            var collectionBeforePaging =
                _context.User
                .ApplySort(pageparams.OrderBy,
                _propertyMappingService.GetPropertyMapping<DC.Responses.UserCreationResponse, User>());

            if (pageparams.Clinic > 0)
            {
                // trim & ignore casing
                int clinicForWhereClause = pageparams.Clinic;
                // collectionBeforePaging = collectionBeforePaging
                //  .Where(a => a.ClinicId == clinicForWhereClause);
            }

            if (!string.IsNullOrEmpty(pageparams.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = pageparams.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Username.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.FirstName.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.LastName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            var pagedCollection = new PagedList<User>(); //PagedList<User>.Create(collectionBeforePaging, pageparams.PageNumber, pageparams.PageSize);

            return Tuple.Create(pagedCollection, true, AlertMessages.User_Success);
        }

        public Tuple<User, bool> GetById(string id)
        {
            return Tuple.Create(_context.User.SingleOrDefault(x => x.Id.Equals(id) && x.IsActive && x.LockoutEnabled), true);
        }

        public Tuple<User, bool, string> Create(DC.Requests.UserCreationRequest userRequest)
        {
            if (string.IsNullOrWhiteSpace(userRequest.Password))
                return Tuple.Create(new User(), true, "Password is required");

            if (_context.User.Any(x => x.Username == userRequest.Username))
                return Tuple.Create(new User(), true, "Username \"" + userRequest.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(userRequest.Password, out passwordHash, out passwordSalt);

            User userEntity = new User()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedDateUTC = DateTime.UtcNow,
                EmailConfirmed = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                LastLoginDateUTC = DateTime.UtcNow,
                Email = userRequest.Email,
                FirstName = userRequest.FirstName,
                IsActive = userRequest.IsActive,
                LastName = userRequest.LastName,
                LockoutEndDateUtc = DateTime.UtcNow,
                Username = userRequest.Username,
                Phone = userRequest.Phone,
                Timezone = userRequest.Timezone,
                OrganizationId = int.Parse(userRequest.OrganizationId)
            };

            _context.User.Add(userEntity);
            _context.SaveChanges();

            return Tuple.Create(userEntity, true, "User created successfully");
        }

        public Tuple<User, bool, string> Update(User userParam, string password = null)
        {
            var user = _context.User.Find(userParam.Id);

            if (user == null)
                return Tuple.Create(new User(), true, "User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.User.Any(x => x.Username == userParam.Username))
                    return Tuple.Create(new User(), true, "Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.User.Update(user);
            _context.SaveChanges();

            return Tuple.Create(user, true, "User details updated successfully");
        }

        public Tuple<bool, string> Delete(string id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }

            return Tuple.Create(true, "User account deleted successfully");
        }

        public Tuple<bool, string> UserActivateDeactivate(string id)
        {
            var user = _context.User.Find(id);
            string msg = "";
            if (user != null)
            {
                if (user.IsActive)
                {
                    user.IsActive = false;
                    msg = "User account de-activated successfully";
                }
                else
                {
                    user.IsActive = true;
                    msg = "User account activated successfully";
                }

                _context.User.Update(user);
                _context.SaveChanges();
            }

            return Tuple.Create(true, msg);
        }

        #endregion ServiceMethods
    }
}
