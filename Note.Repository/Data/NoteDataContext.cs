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
    using System.Configuration;
    using Note.API.Common.Extensions;
    using DC = API.DataContracts;

    public class NoteDataContext : DbContext
    {
        public OdbcConnection DBConnection { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionPath = SettingsExtensions.DBConnectionString;


        public IEnumerable<DC.operatory_notes> GetOperatoryNotes()
        {
            List<DC.operatory_notes> lstNotes = new List<DC.operatory_notes>();

            using (OdbcConnection con = new OdbcConnection(ConnectionPath))

            {
                OdbcCommand cmd = new OdbcCommand("select * from operatory_notes", con);
                con.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    DC.operatory_notes on = new DC.operatory_notes();

                    on.note_id = rdr["note_id"].ToString(); ;
                    on.patient_id = rdr["patient_id"].ToString();
                    on.note_type = rdr["note_type"].ToString();
                    on.description = rdr["description"].ToString();
                    on.practice_id = rdr["practice_id"].ToString();
                    lstNotes.Add(on);
                }
                con.Close();
            }
            return lstNotes;
        }
        public IEnumerable<DC.operatory_notes> GetOperatoryNotesByPatientId(string patient_id)

        {
            List<DC.operatory_notes> lstNotes = new List<DC.operatory_notes>();
            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {
                OdbcCommand cmd = new OdbcCommand("SELECT * FROM operatory_notes WHERE patient_id = " + patient_id, con);

                con.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    DC.operatory_notes on = new DC.operatory_notes();
                    on.note_id = rdr["note_id"].ToString(); ;
                    on.patient_id = rdr["patient_id"].ToString();
                    on.note_type = rdr["note_type"].ToString();
                    on.description = rdr["description"].ToString();

                    lstNotes.Add(on);

                }
                con.Close();
            }
            return lstNotes;
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
    }
}
