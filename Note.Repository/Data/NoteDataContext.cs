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
        public IEnumerable<operatory_notes> GetOperatoryNotesByPatientIdByClinicIDByProviderId( string patientId, string clinicId, string UserId)

        {
            List<operatory_notes> lstNotes = new List<operatory_notes>();
            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {

                string query2 = string.Format(@"SELECT  p.first_name as patientFirstName,p.last_name as patientLastName,pr.first_name ,pr.last_name ,* FROM operatory_notes o_n INNER JOIN patient p ON o_n.patient_Id=p.patient_Id
INNER JOIN provider pr  ON  o_n.user_Id=pr.provider_Id  where p.patient_id  ='{0}' AND  o_n.practice_id= '{1}' AND  o_n.user_id= '{2}' order by o_n.date_entered desc", patientId, clinicId, UserId);

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




        
        /// <summary>
        /// Insert or Update Notes basedon the autoNoteId passing from the front end or not
        /// </summary>
        public void InsertOrUpdateOperatoryNotes(operatory_notes operatoryNotes, int? autoNoteId)
        {

            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {

                var dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                if (autoNoteId != null)
                {
                    string query = string.Format(@"Select description,note_text from autonotes where note_id='{0}'", autoNoteId);
                    OdbcCommand cmd1 = new OdbcCommand(query, con);
                    con.Open();
                    OdbcDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        operatory_notes on = new operatory_notes();
                        on.description = rdr["description"].ToString();
                        on.note = rdr["note_text"].ToString();
                        // If AutoNote Id is passing from the FrontEnd to an Existing Note.

                        if (operatoryNotes.note_id != 0)
                        {
                            // Note class should be always T
                            string query1 = string.Format(@"Update 
                                        operatory_notes SET date_modified = '{0}',user_id='{1}',description='{2}',  note = note +','+'{3}',note_class='{4}' 
                                       where note_id ='{5}' AND patient_id ='{6}' AND  practice_id= '{7}' AND  user_id= '{8}'",
                                     dateTimeNow, operatoryNotes.user_id, on.description, on.note + operatoryNotes.note,"T", operatoryNotes.note_id, 
                                      operatoryNotes.patient_id, operatoryNotes.practice_id, operatoryNotes.user_id);

                            OdbcCommand cmd2 = new OdbcCommand(query1, con);
                            cmd2.ExecuteNonQuery();
                        }
                        else
                        {
                            // If AutoNote Id is passing from the FrontEnd to an Fresh Note.

                            string query2 = "Insert into operatory_notes (patient_id,Date_entered,user_id,note_class,note_type,note_type_id,description,note,color,post_proc_status,date_modified,modified_by,locked_eod,status,tooth_data,claim_id,statement_yn,resp_party_id,tooth,tran_num,archive_name,archive_path,service_code,practice_id,freshness,surface_detail,surface)  VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";//, $data['patient_id'],$Date_entered,$user_id,$note_class,$note_type,$note_type_id,$description,$note,$color,$post_proc_status,$date_modified,$modified_by,$locked_eod,$status,$tooth_data,$claim_id,$statement_yn,$resp_party_id,$tooth,$tran_num,$archive_name,$archive_path,$service_code,$practice_id,$freshness,$surface_detail,$surface)";



                            OdbcCommand cmd = new OdbcCommand(query2, con);

                            cmd.Parameters.AddWithValue("?", operatoryNotes.patient_id);
                            cmd.Parameters.AddWithValue("?", dateTimeNow);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.user_id);
                            cmd.Parameters.AddWithValue("?", "T");// Note class should be always T
                            cmd.Parameters.AddWithValue("?", operatoryNotes.note_type);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.note_type_id);

                            cmd.Parameters.AddWithValue("?", operatoryNotes.description);
                            cmd.Parameters.AddWithValue("?", on.note + operatoryNotes.note);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.color);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.post_proc_status);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.date_modified);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.modified_by);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.locked_eod);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.status);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.tooth_data);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.claim_id);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.statement_yn);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.resp_party_id);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.tooth);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.tran_num);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.archive_name);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.archive_path);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.service_code);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.practice_id);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.freshness);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.surface_detail);
                            cmd.Parameters.AddWithValue("?", operatoryNotes.surface);

                            cmd.ExecuteNonQuery();
                           // con.Close();

                        }

                    }
                    con.Close();
                }

                // If AutoNote Id is NOT passing from the FrontEnd and provider is writing to an existing note    
                else
                {

                        if (operatoryNotes.note_id != 0)
                    {
                        con.Open();
                        // Note class should be always T
                        string query3 = string.Format(@"Update operatory_notes SET date_modified = '{0}',user_id='{1}', note = note +','+'{2}', note_class='{3}'  where note_id ='{4}' AND patient_id ='{5}' AND  practice_id= '{6}' AND  user_id= '{7}'",
                    dateTimeNow, operatoryNotes.user_id, operatoryNotes.note,"T", operatoryNotes.note_id, operatoryNotes.patient_id, operatoryNotes.practice_id, operatoryNotes.user_id);
                        OdbcCommand cmd3 = new OdbcCommand(query3, con);
                            cmd3.ExecuteNonQuery();
                            con.Close();

                    }
                    else
                    {
                        string query4 = "Insert into operatory_notes (patient_id,Date_entered,user_id,note_class,note_type,note_type_id,description,note,color,post_proc_status,date_modified,modified_by,locked_eod,status,tooth_data,claim_id,statement_yn,resp_party_id,tooth,tran_num,archive_name,archive_path,service_code,practice_id,freshness,surface_detail,surface)  VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";//, $data['patient_id'],$Date_entered,$user_id,$note_class,$note_type,$note_type_id,$description,$note,$color,$post_proc_status,$date_modified,$modified_by,$locked_eod,$status,$tooth_data,$claim_id,$statement_yn,$resp_party_id,$tooth,$tran_num,$archive_name,$archive_path,$service_code,$practice_id,$freshness,$surface_detail,$surface)";



                        OdbcCommand cmd4 = new OdbcCommand(query4, con);

                        cmd4.Parameters.AddWithValue("?", operatoryNotes.patient_id);
                        cmd4.Parameters.AddWithValue("?", dateTimeNow);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.user_id);
                        cmd4.Parameters.AddWithValue("?", "T");// Note class should be always T
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.note_type);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.note_type_id);

                        cmd4.Parameters.AddWithValue("?", operatoryNotes.description);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.note);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.color);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.post_proc_status);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.date_modified);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.modified_by);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.locked_eod);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.status);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.tooth_data);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.claim_id);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.statement_yn);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.resp_party_id);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.tooth);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.tran_num);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.archive_name);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.archive_path);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.service_code);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.practice_id);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.freshness);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.surface_detail);
                        cmd4.Parameters.AddWithValue("?", operatoryNotes.surface);

                        // con.Close();

                        con.Open();
                        cmd4.ExecuteNonQuery();
                        con.Close();

                    }

                }
               
           
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
       

    }
}