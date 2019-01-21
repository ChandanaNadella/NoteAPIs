namespace Note.Services.Contracts
{
    using Note.API.Common.Helpers;
    using Note.Repository.Data.Entities;
    using DC = API.DataContracts;
    using System;
    public interface IUserService
    {
        /// <summary>
        /// Passing user credentials for Athentication .
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Tuple<User, bool, string> Authenticate(string username, string password);

        /// <summary>
        /// Get list of all users.
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <returns></returns>
        Tuple<PagedList<User>, bool, string> GetAll(UserResourceParameters pageparams);

        /// <summary>
        /// Get user details by user id.
        /// Aviral, Bpktech. 
        /// </summary>
        /// <param name="id">guid id</param>
        /// <returns>user details</returns>
        Tuple<User, bool> GetById(string id);

        /// <summary>
        /// User registration.
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        Tuple<User, bool, string> Create(DC.Requests.UserCreationRequest userRequest);

        /// <summary>
        /// User Update by user id.
        /// Shiva Ch, Bpktech.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Tuple<User, bool, string> Update(User user, string password = null);

        /// <summary>
        /// User Deletion by user id.
        /// Shiva Ch, Bpktech. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tuple<bool, string> Delete(string id);

        /// <summary>
        /// User Activate/De-activation by user id.
        /// Shiva Ch, Bpktech. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Tuple<bool, string> UserActivateDeactivate(string id);
    }
}
