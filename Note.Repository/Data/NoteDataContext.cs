namespace Note.Repository.Data
{
    using Microsoft.EntityFrameworkCore;
    using Note.API.Common.Extensions;
    using Note.Repository.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Odbc;

    public class NoteDataContext : DbContext
    {
        public OdbcConnection DBConnection { get; set; }
        public string ConnectionString { get; set; }
        public string ConnectionPath = SettingsExtensions.DBConnectionString;


        public IEnumerable<operatory_notes> GetOperatoryNotes()
        {
            List<operatory_notes> lstNotes = new List<operatory_notes>();


            using (OdbcConnection con = new OdbcConnection(ConnectionPath))

            {
                OdbcCommand cmd = new OdbcCommand("select * from operatory_notes", con);
                con.Open();
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    operatory_notes on = new operatory_notes();

                    #region Operatory Data Table

                    on.note_id = Convert.ToInt32(rdr["note_id"]);
                    on.Date_entered = Convert.ToDateTime(rdr["Date_entered"]);
                    on.note_class = Convert.ToChar(rdr["note_class"]);
                    on.note_type = rdr["note_type"].ToString();
                    on.note_type_id = Convert.ToInt32(rdr["note_type_id"]);
                    on.description = rdr["description"].ToString();
                    on.note = rdr["note"].ToString();
                    on.color = Convert.ToInt32(rdr["color"]);
                    on.post_proc_status = Convert.ToChar(rdr["post_proc_status"]);
                    on.date_modified = rdr["date_modified"].ToString();
                    on.modified_by = rdr["modified_by"].ToString();
                    on.locked_eod = Convert.ToInt32(rdr["locked_eod"]);
                    on.status = Convert.ToChar(rdr["status"]);
                    on.tooth_data = rdr["tooth_data"].ToString();
                    on.claim_id = Convert.ToInt32(rdr["claim_id"]);
                    on.statement_yn = Convert.ToChar(rdr["statement_yn"]);
                    on.resp_party_id = rdr["resp_party_id"].ToString();
                    on.tooth = rdr["tooth"].ToString();
                    on.tran_num = Convert.ToInt32(rdr["tran_num"]);
                    on.archive_name = rdr["archive_name"].ToString();
                    on.archive_path = rdr["archive_path"].ToString();
                    on.service_code = rdr["service_code"].ToString();
                    on.practice_id = Convert.ToInt16(rdr["practice_id"]);
                    on.freshness = Convert.ToDateTime(rdr["freshness"]);
                    on.surface_detail = rdr["surface_detail"].ToString();
                    on.surface = rdr["surface"].ToString();

                    #endregion Operatory Data Table


                    #region Provider Data Table

                    on.provider_id = rdr["user_id"].ToString();
                    on.first_name = rdr["first_name"].ToString();
                    on.last_name = rdr["last_name"].ToString();

                    #endregion Provider Data Table

                    lstNotes.Add(on);

                }
                con.Close();
            }
            return (lstNotes);
        }
        public IEnumerable<operatory_notes> GetOperatoryNotesByPatientIdByClinicIDByProviderId( string patientId, string clinicId, string providerId)

        {
            List<operatory_notes> lstNotes = new List<operatory_notes>();
            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {

                string query2 = string.Format(@"SELECT  p.first_name as patientFirstName,p.last_name as patientLastName,pr.first_name ,pr.last_name ,* FROM operatory_notes o_n INNER JOIN patient p ON o_n.patient_Id=p.patient_Id
INNER JOIN provider pr  ON  o_n.user_Id=pr.provider_Id  where p.patient_id  ='{0}' AND  o_n.practice_id= '{1}' AND  o_n.user_id= '{2}' order by o_n.date_entered desc", patientId, clinicId, providerId);

                OdbcCommand cmd = new OdbcCommand(query2, con);

                con.Open();

                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {

                    operatory_notes on = new operatory_notes();

                    #region Operatory Data Table

                    on.note_id = Convert.ToInt32(rdr["note_id"]);
                    on.Date_entered = Convert.ToDateTime(rdr["Date_entered"]);
                    on.note_class = Convert.ToChar(rdr["note_class"]);
                    on.note_type = rdr["note_type"].ToString();
                    on.note_type_id = Convert.ToInt32(rdr["note_type_id"]);
                    on.description = rdr["description"].ToString();
                    on.note = rdr["note"].ToString();
                    on.color = Convert.ToInt32(rdr["color"]);
                    on.post_proc_status = Convert.ToChar(rdr["post_proc_status"]);
                    on.date_modified = rdr["date_modified"].ToString();
                    on.modified_by = rdr["modified_by"].ToString();
                    on.locked_eod = Convert.ToInt32(rdr["locked_eod"]);
                    on.status = Convert.ToChar(rdr["status"]);
                    on.tooth_data = rdr["tooth_data"].ToString();
                    on.claim_id = Convert.ToInt32(rdr["claim_id"]);
                    on.statement_yn = Convert.ToChar(rdr["statement_yn"]);
                    on.resp_party_id = rdr["resp_party_id"].ToString();
                    on.tooth = rdr["tooth"].ToString();
                    on.tran_num = Convert.ToInt32(rdr["tran_num"]);
                    on.archive_name = rdr["archive_name"].ToString();
                    on.archive_path = rdr["archive_path"].ToString();
                    on.service_code = rdr["service_code"].ToString();
                    on.practice_id = Convert.ToInt16(rdr["practice_id"]);
                    on.freshness = Convert.ToDateTime(rdr["freshness"]);
                    on.surface_detail = rdr["surface_detail"].ToString();
                    on.surface = rdr["surface"].ToString();

                    #endregion Operatory Data Table

                    #region Patient Data Table

                    on.patient_id = rdr["patient_id"].ToString();
                    on.patientFirstName = rdr["patientFirstName"].ToString();
                    on.patientLastName = rdr["patientLastName"].ToString();

                    #endregion Patient Data Table

                    #region Provider Data Table

                    on.provider_id = rdr["user_id"].ToString();
                    on.first_name = rdr["first_name"].ToString();
                    on.last_name = rdr["last_name"].ToString();

                    #endregion Provider Data Table
                    lstNotes.Add(on);
                    on.surface = rdr["surface"].ToString();
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