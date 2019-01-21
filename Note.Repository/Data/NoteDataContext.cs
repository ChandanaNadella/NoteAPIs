namespace Note.Repository.Data
{
    using System;
    using Note.Repository.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Data;
    using System.Data.Odbc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;    

    public class NoteDataContext : DbContext
    {
        string connectionString = "Dsn=Dental;uid=PDBA;databasename=DENTSERV;databasefile=C:\\EagleSoft\\Data\\PattersonPM.db;servername=DENTAL;startline='C:\\EagleSoft\\Shared Files\\dbeng16.exe';autostop=YES;integrated=NO;description='Eaglesoft Database'";
        
        public IEnumerable<category> GetAllCategories()
        {
            List<category> lstcategory = new List<category>();
           
                using (OdbcConnection con = new OdbcConnection(connectionString)) {

                OdbcCommand cmd = new OdbcCommand("select * from category", con);
                 con.Open();          

                    OdbcDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    category category = new category();
                    category.category_id= rdr["category_id"].ToString();
                    category.description = rdr["description"].ToString();
                    category.color = Convert.ToInt32(rdr["color"]);
                    category.may_delete = Convert.ToBoolean(rdr["may_delete"]);

                    lstcategory.Add(category);

                }
                con.Close();
                return lstcategory;

                }
            

        }
        public NoteDataContext()
        {
        }

        public NoteDataContext(DbContextOptions<NoteDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NewPatient> NewPatient { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<Subscriber> Subscriber { get; set; }

        public DbSet<Clinic> Clinic { get; set; }
        public DbSet<category> Category { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=DESKTOP-2GP9UUI;Database=Note;Trusted_Connection=True;");
        //    }
        //}
    }
}
 