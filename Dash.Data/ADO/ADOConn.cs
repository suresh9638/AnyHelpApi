using anyhelp.Data.DataContext;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace anyhelp.Data.ADO
{

    public class ADOConn : IDisposable
    {
        public AppConfiguration obj_app = new AppConfiguration();

         
                
        public DataTable GetData(string str)
        {
            DataTable objresutl = new DataTable();
            try
            {
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(obj_app.SqlConnectonString))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(str, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        objresutl.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);


            }

            return objresutl;

        }
        public int ExecuteData(string str, params IDataParameter[] sqlParams)
        {
            int rows = -1;
            try
            {

                using (SqlConnection conn = new SqlConnection(obj_app.SqlConnectonString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(str, conn))
                    {
                        if (sqlParams != null)
                        {
                            foreach (IDataParameter para in sqlParams)
                            {
                                cmd.Parameters.Add(para);
                            }
                            rows = cmd.ExecuteNonQuery();
                        }





                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

            }


            return rows;


        }

        public long GetCountFriends(long UserId)
        {

            try
            {
                Dictionary<object, object> ParameterList = new Dictionary<object, object>();
                ParameterList.Add("userid", UserId);
                 var dt = StoreProcedureExecuteReader("Sp_GetCountFriends", ParameterList);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt64(dt.Rows[0][0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return 0;
            }



        }
        public bool GetIsFriend(long senderid, long requesterid)
        {

            try
            {
                Dictionary<object, object> ParameterList = new Dictionary<object, object>();
                ParameterList.Add("senderid", senderid);
                ParameterList.Add("requesterid", requesterid);

                var dt = StoreProcedureExecuteReader("Sp_GetIsFriend", ParameterList);
                
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return false;
            }



        }

        public bool GetIsAlreadyFriendRequestsend(long senderid, long requesterid)
        {

            try
            {
                Dictionary<object, object> ParameterList = new Dictionary<object, object>();
                ParameterList.Add("senderid", senderid);
                ParameterList.Add("requesterid", requesterid);

                var dt = StoreProcedureExecuteReader("Sp_GetIsAlreadyFriendRequestsend", ParameterList);

               
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                return false;
            }



        }
        

        public void Dispose()
        {
            try
            {
                SqlConnection.ClearAllPools();
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }

        }

        public Int64 StoreProcedureExecuteNonQuery(string Sp_Name, Dictionary<object, object> DictionaryList)
        {

            Int64 rows = -1;
            try
            {

                using (SqlConnection conn = new SqlConnection(obj_app.SqlConnectonString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Sp_Name, conn))
                    {

                        if (DictionaryList != null)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            foreach (var data in DictionaryList)
                            {
                                cmd.Parameters.AddWithValue(Convert.ToString(data.Key), data.Value);
                               
                            }
                        }
                        try
                        {
                            rows = (Int64)cmd.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.SendErrorToText(ex);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);


            }


            return rows;




        }
        public DataTable StoreProcedureExecuteReader(string Sp_Name, Dictionary<object, object> DictionaryList)
        {
            DataTable dt = new DataTable();           
            try
            {

                using (SqlConnection conn = new SqlConnection(obj_app.SqlConnectonString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(Sp_Name, conn))
                    {
                       
                        if (DictionaryList != null)
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            foreach (var data in DictionaryList)
                            {
                                cmd.Parameters.AddWithValue(Convert.ToString(data.Key), data.Value);
                             
                            }
                        }                                                                

                        SqlDataReader myReader = cmd.ExecuteReader();

                        dt.Load(myReader);

                        myReader.Close();
                        conn.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);


            }
            return dt;
        }
    }
}
