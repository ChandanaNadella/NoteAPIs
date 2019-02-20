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

        /// <summary>
        /// Get's OperatoryNotes based on PatientId, ClinicId, UserId passing from the front end
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="clinicId"></param>
        /// <param name="UserId"></param>
        /// <returns>OperatoryNotes table filtered by PatientId, ClinicId, UserId</returns>
        #region GetOperatoryNotes
        public Tuple<List<operatory_notes>, int> GetOperatoryNotesByPatientIdByClinicIDByUserId(string patientId, string clinicId, string UserId, string OrderBy, int? pageSize, int currentPage = 1)

        {
            List<operatory_notes> lstNotes = new List<operatory_notes>();
            int totalCount = 0;

            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {


                string query1 = string.Format(@"SELECT Count(*) as totalCount FROM operatory_notes o_n INNER JOIN patient p ON o_n.patient_Id=p.patient_Id
INNER JOIN provider pr  ON  o_n.user_Id=pr.provider_Id  where p.patient_id  ='{0}' AND  o_n.practice_id= '{1}' AND  o_n.user_id= '{2}'", patientId, clinicId, UserId);

                OdbcCommand cmd1 = new OdbcCommand(query1, con);


                string pagination = "";
                if (pageSize.HasValue)
                {
                    pagination = string.Format("Top {0} start at(({0}*{1}+1)-{0})", pageSize, currentPage);
                }
               
                
              string query2 = string.Format(@"SELECT {0} p.first_name as patientFirstName,p.last_name as patientLastName,pr.first_name ,pr.last_name ,* FROM operatory_notes o_n INNER JOIN patient p ON o_n.patient_Id=p.patient_Id INNER JOIN provider pr  ON  o_n.user_Id=pr.provider_Id  where p.patient_id  ='{1}' AND  o_n.practice_id= '{2}'AND  o_n.user_id= '{3}' order by {4}", pagination, patientId, clinicId, UserId, OrderBy);

                OdbcCommand cmd2 = new OdbcCommand(query2, con);



                con.Open();

                OdbcDataReader rdr1 = cmd1.ExecuteReader();

                while (rdr1.Read())
                {
                    totalCount = Convert.ToInt32(rdr1["totalCount"]);
                }

                OdbcDataReader rdr2 = cmd2.ExecuteReader();

                while (rdr2.Read())
                {

                    operatory_notes on = new operatory_notes();

                    #region Operatory Data Table

                    on.note_id = Convert.ToInt32(rdr2["note_id"]);
                   // on.patient_id = rdr2["patient_id"].ToString();
                    on.date_entered = Convert.ToDateTime(rdr2["Date_entered"]);
                   //on.user_id = rdr2["user_id"].ToString();
                    on.note_class = Convert.ToChar(rdr2["note_class"]);
                    on.note_type = rdr2["note_type"].ToString();
                    on.note_type_id = Convert.ToInt32(rdr2["note_type_id"]);
                    on.description = rdr2["description"].ToString();
                    on.note = rdr2["note"].ToString();
                    on.color = Convert.ToInt32(rdr2["color"]);
                    on.post_proc_status = Convert.ToChar(rdr2["post_proc_status"]);
                    on.date_modified = rdr2["date_modified"].ToString();
                    on.modified_by = rdr2["modified_by"].ToString();
                    on.locked_eod = Convert.ToInt32(rdr2["locked_eod"]);
                    on.status = Convert.ToChar(rdr2["status"]);
                    on.tooth_data = rdr2["tooth_data"].ToString();
                    on.claim_id = Convert.ToInt32(rdr2["claim_id"]);
                    on.statement_yn = Convert.ToChar(rdr2["statement_yn"]);
                    on.resp_party_id = rdr2["resp_party_id"].ToString();
                    on.tooth = rdr2["tooth"].ToString();
                    on.tran_num = Convert.ToInt32(rdr2["tran_num"]);
                    on.archive_name = rdr2["archive_name"].ToString();
                    on.archive_path = rdr2["archive_path"].ToString();
                    on.service_code = rdr2["service_code"].ToString();
                    on.practice_id = Convert.ToInt16(rdr2["practice_id"]);
                    on.freshness = Convert.ToDateTime(rdr2["freshness"]);
                    on.surface_detail = rdr2["surface_detail"].ToString();
                    on.surface = rdr2["surface"].ToString();

                    #endregion Operatory Data Table

                    #region Patient Data Table

                    on.patient_id = rdr2["patient_id"].ToString();
                    on.patientFirstName = rdr2["patientFirstName"].ToString();
                    on.patientLastName = rdr2["patientLastName"].ToString();

                    #endregion Patient Data Table

                    #region Provider Data Table

                    on.provider_id = rdr2["user_id"].ToString();
                    on.first_name = rdr2["first_name"].ToString();
                    on.last_name = rdr2["last_name"].ToString();

                    #endregion Provider Data Table
                    lstNotes.Add(on);
                    
                }
                con.Close();
            }

            return Tuple.Create(lstNotes, totalCount);
        }

        #endregion GetOperatoryNotes

        /// <summary>
        /// Insert or Update Notes basedon the autoNoteId passing from the front end or not
        /// </summary>
        /// <param name="operatoryNotes"></param>
        /// <param name="autoNoteId"></param>
        
        #region InsertOrUpdateOperatoryNotes
       
        public void InsertOrUpdateOperatoryNotes(operatory_notes operatoryNotes, int? autoNoteId , string noteType)
        {

            using (OdbcConnection con = new OdbcConnection(ConnectionPath))
            {

                var dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                if (autoNoteId != null)
                {
                    string query = string.Format(@"Select note_text, description from autonotes where note_id='{0}'", autoNoteId);
                    string querydesc = string.Format(@"Select  id, note_type, description from operatory_notes_type  where note_type='{0}'", noteType);
                    OdbcCommand cmd1 = new OdbcCommand(query, con);
                    OdbcCommand cmd5 = new OdbcCommand(querydesc, con);
                    con.Open();
                    OdbcDataReader rdr = cmd1.ExecuteReader();
                    OdbcDataReader rdr1 = cmd5.ExecuteReader();

                    while (rdr1.Read())
                    {
                        operatory_notes opn = new operatory_notes();
                        opn.note_type_id = Convert.ToInt32(rdr1["id"]);
                        opn.note_type = rdr1["note_type"].ToString();
                        opn.description = rdr1["description"].ToString();

                       

                        while (rdr.Read())
                        {
                            operatory_notes on = new operatory_notes();
                            on.note = rdr["note_text"].ToString();


                            // If AutoNote Id is passing from the Api to an Existing Note.

                            if (operatoryNotes.note_id != 0 && operatoryNotes.note_type != "N")
                            {

                                string query1 = string.Format(@"Update operatory_notes SET note_type_id='{0}',note_type='{1}', date_modified = '{2}', freshness = '{2}' , user_id='{3}', description='{4}', note = note +','+'{5}', note_class='{6}'  where note_id ='{7}' AND patient_id ='{8}' AND  practice_id= '{9}' AND  user_id= '{3}'",
                                   opn.note_type_id, opn.note_type, dateTimeNow, operatoryNotes.user_id, opn.description, operatoryNotes.note, "T", operatoryNotes.note_id, operatoryNotes.patient_id, operatoryNotes.practice_id, operatoryNotes.user_id);

                                OdbcCommand cmd2 = new OdbcCommand(query1, con);
                                cmd2.ExecuteNonQuery();
                            }
                            else
                            {
                                // If AutoNote Id is passing from the Api to an Fresh Note.

                                string query2 = "Insert into operatory_notes (patient_id,Date_entered,user_id,note_class,note_type,note_type_id,description,note,color,post_proc_status,date_modified,modified_by,locked_eod,status,tooth_data,claim_id,statement_yn,resp_party_id,tooth,tran_num,archive_name,archive_path,service_code,practice_id,freshness,surface_detail,surface)  VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";//, $data['patient_id'],$Date_entered,$user_id,$note_class,$note_type,$note_type_id,$description,$note,$color,$post_proc_status,$date_modified,$modified_by,$locked_eod,$status,$tooth_data,$claim_id,$statement_yn,$resp_party_id,$tooth,$tran_num,$archive_name,$archive_path,$service_code,$practice_id,$freshness,$surface_detail,$surface)";



                                OdbcCommand cmd = new OdbcCommand(query2, con);
                                #region Operatory Data Table
                                cmd.Parameters.AddWithValue("?", operatoryNotes.patient_id);
                                cmd.Parameters.AddWithValue("?", dateTimeNow);
                                cmd.Parameters.AddWithValue("?", operatoryNotes.user_id);
                                cmd.Parameters.AddWithValue("?", "T");// Note class should be always T

                                cmd.Parameters.AddWithValue("?", opn.note_type);
                                cmd.Parameters.AddWithValue("?", opn.note_type_id);
                                cmd.Parameters.AddWithValue("?", opn.description);

                                cmd.Parameters.AddWithValue("?", on.note + operatoryNotes.note);

                                cmd.Parameters.AddWithValue("?", operatoryNotes.color);
                                cmd.Parameters.AddWithValue("?", operatoryNotes.post_proc_status);
                                cmd.Parameters.AddWithValue("?", dateTimeNow);
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
                                cmd.Parameters.AddWithValue("?", dateTimeNow);
                                cmd.Parameters.AddWithValue("?", operatoryNotes.surface_detail);
                                cmd.Parameters.AddWithValue("?", operatoryNotes.surface);
                                #endregion Operatory Data Table
                                cmd.ExecuteNonQuery();


                            }

                        }
                    }
                        con.Close();
                }

                else
                {

                    string query = string.Format(@"Select  id, note_type, description from operatory_notes_type  where note_type='{0}'", noteType);
                    OdbcCommand cmd1 = new OdbcCommand(query, con);

                    con.Open();
                    OdbcDataReader rdr = cmd1.ExecuteReader();

                    while (rdr.Read())
                    {
                        operatory_notes opn = new operatory_notes();
                        opn.note_type_id = Convert.ToInt32(rdr["id"]);
                        opn.note_type = rdr["note_type"].ToString();
                        opn.description = rdr["description"].ToString();

                        // If AutoNote Id is NOT passed from the Api and provider is Modifing an existing note   
                        if (operatoryNotes.note_id != 0 && operatoryNotes.note_type != "N")
                        {
                          
                            string query3 = string.Format(@"Update operatory_notes SET note_type_id='{0}',note_type='{1}', date_modified = '{2}', freshness = '{2}' , user_id='{3}', description='{4}', note = note +','+'{5}', note_class='{6}'  where note_id ='{7}' AND patient_id ='{8}' AND  practice_id= '{9}' AND  user_id= '{3}'",
                                 opn.note_type_id, opn.note_type, dateTimeNow, operatoryNotes.user_id, opn.description, operatoryNotes.note, "T", operatoryNotes.note_id, operatoryNotes.patient_id, operatoryNotes.practice_id, operatoryNotes.user_id);
                            OdbcCommand cmd3 = new OdbcCommand(query3, con);
                            cmd3.ExecuteNonQuery();

                        }
                        // If AutoNote Id is NOT passed from the Api and provider is creating an new note   
                        else
                        {
                            string query4 = "Insert into operatory_notes (patient_id,Date_entered,user_id,note_class,note_type,note_type_id,description,note,color,post_proc_status,date_modified,modified_by,locked_eod,status,tooth_data,claim_id,statement_yn,resp_party_id,tooth,tran_num,archive_name,archive_path,service_code,practice_id,freshness,surface_detail,surface)  VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";//, $data['patient_id'],$Date_entered,$user_id,$note_class,$note_type,$note_type_id,$description,$note,$color,$post_proc_status,$date_modified,$modified_by,$locked_eod,$status,$tooth_data,$claim_id,$statement_yn,$resp_party_id,$tooth,$tran_num,$archive_name,$archive_path,$service_code,$practice_id,$freshness,$surface_detail,$surface)";

                            OdbcCommand cmd4 = new OdbcCommand(query4, con);
                            #region Operatory Data Table
                            cmd4.Parameters.AddWithValue("?", operatoryNotes.patient_id);
                            cmd4.Parameters.AddWithValue("?", dateTimeNow);
                            cmd4.Parameters.AddWithValue("?", operatoryNotes.user_id);
                            cmd4.Parameters.AddWithValue("?", "T");// Note class should be always T
                            cmd4.Parameters.AddWithValue("?", opn.note_type);
                            cmd4.Parameters.AddWithValue("?", opn.note_type_id);

                            cmd4.Parameters.AddWithValue("?", opn.description);

                            cmd4.Parameters.AddWithValue("?", operatoryNotes.note);

                            cmd4.Parameters.AddWithValue("?", operatoryNotes.color);
                            cmd4.Parameters.AddWithValue("?", operatoryNotes.post_proc_status);
                            cmd4.Parameters.AddWithValue("?", dateTimeNow);
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
                            cmd4.Parameters.AddWithValue("?", dateTimeNow);
                            cmd4.Parameters.AddWithValue("?", operatoryNotes.surface_detail);
                            cmd4.Parameters.AddWithValue("?", operatoryNotes.surface);
                            #endregion Operatory Data Table
                            cmd4.ExecuteNonQuery();

                        }
                       

                    }
                    con.Close();

                }
               
           
            }
        }
        #endregion InsertOrUpdateOperatoryNotes

        // The Db Context class which acts as bridge between entity classes and database
        public NoteDataContext()
        {
        }

        public NoteDataContext(DbContextOptions<NoteDataContext> options)
            : base(options)
        {
        }

        public DbSet<operatory_notes> operatory_notes { get; set; }


    }
}