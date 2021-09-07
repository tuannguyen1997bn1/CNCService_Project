using Lucene.Net.Util;
using Magnum.Collections;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using static CNCService.DataRequest1;
using static CNCService.StatusVM;

namespace CNCService
{
    public partial class Service1 : ServiceBase
    {
        
        private Timer timer1 = null;
        private AdvantechHttpWebUtility m_httpRequest1;
        private AdvantechHttpWebUtility m_httpRequest2;
        private AdvantechHttpWebUtility m_httpRequest3;
        private AdvantechHttpWebUtility m_httpRequest4;
        public Service1()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            Utilities.WriteLogError(" ++++++++++++ " + "Service START at : " + DateTime.Now.ToString() + " ++++++++++++ ");
            m_httpRequest1 = new AdvantechHttpWebUtility();
            m_httpRequest2 = new AdvantechHttpWebUtility();
            m_httpRequest3 = new AdvantechHttpWebUtility();
            m_httpRequest4 = new AdvantechHttpWebUtility();
            m_httpRequest1.ResquestResponded += this.OnGetRequestData1;
            m_httpRequest2.ResquestResponded += this.OnGetRequestData2;
            m_httpRequest3.ResquestResponded += this.OnGetRequestData3;
            m_httpRequest4.ResquestResponded += this.OnGetRequestData4;
            //m_httpRequest1.ResquestOccurredError += this.OnGetErrorRequest1;
            //m_httpRequest2.ResquestOccurredError += this.OnGetErrorRequest2;
            //m_httpRequest3.ResquestOccurredError += this.OnGetErrorRequest3;
            //m_httpRequest4.ResquestOccurredError += this.OnGetErrorRequest4;
            timer1 = new Timer();
            timer1.Interval = 200;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(Send1);
            timer1.Enabled = true;
        }
        private void Send1(object sender, System.Timers.ElapsedEventArgs args)
        {  
            m_httpRequest1.SendGETRequest(m_httpRequest1.BasicAuthAccount1, m_httpRequest1.BasicAuthPassword1, m_httpRequest1.URL1);
            m_httpRequest2.SendGETRequest(m_httpRequest2.BasicAuthAccount2, m_httpRequest2.BasicAuthPassword2, m_httpRequest2.URL2);
            m_httpRequest3.SendGETRequest(m_httpRequest3.BasicAuthAccount3, m_httpRequest3.BasicAuthPassword3, m_httpRequest3.URL3);
            m_httpRequest4.SendGETRequest(m_httpRequest4.BasicAuthAccount4, m_httpRequest4.BasicAuthPassword4, m_httpRequest4.URL4);
        }

        protected override void OnStop()
        {
            base.OnStop();
            timer1.Enabled = false;
            timer1.Stop();
            timer1.Dispose();
            Utilities.WriteLogError(" ============ "+"Service STOP at : " +DateTime.Now.ToString() +" ============ ");
        }
        private void OnGetErrorRequest1(Exception e)
        {
            Utilities.WriteLogError("Wise1 error request");
        }
        private void OnGetErrorRequest2(Exception e)
        {
            Utilities.WriteLogError("Wise2 error request");
        }
        private void OnGetErrorRequest3(Exception e)
        {
            Utilities.WriteLogError("Wise3 error request");
        }
        private void OnGetErrorRequest4(Exception e)
        {
            Utilities.WriteLogError("Wise4 error request");
        }
        private void OnGetRequestData1(string Jsonstr1)
        {
            //thay bằng 2 store procedure : https://referbruv.com/blog/posts/working-with-stored-procedures-in-aspnet-core-ef-core
            /* tạo một ánh xạ tương tự entityFramWork dùng chung cho mọi bảng! bao gồm cả tên bảng
             * 
             * 
             * 
            /* // The IUserRepository declares a single method that validates if a user exists in the database 
                  and if not exists just creates it and returns the Id.
             * namespace SpReadersApi.Providers.Repositories
               {
                    public interface IUserRepository
                    {
                        Task<int> GetOrCreateUserAsync(EventManagerCNC1 CNC1); // tên bảng
                    }
               }
            ///////////////////////////////////////////////////////////////////////
             * #SQL#
                1---  
                CREATE OR ALTER PROCEDURE sp_GetTop1 (@tablename VARCHAR(100)))
                AS
                BEGIN
                SELECT TOP 1 * FROM @tablename ORDER BY ID DESC;
                END

                2---
                CREATE OR ALTER PROCEDURE sp_InsertValue (@tablename VARCHAR(100), @IdHienTrangMayCNC INT(10), @IdHienTrangCuaMayCNC INT(10), @ThoiDiem DATETIME))
                AS
                BEGIN
                INSERT into @tablename (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem);
                END
             * #.NET#
             * var val1 = new SqlParameter("@tablename", CNC1.NameTable);
               var val2 = new SqlParameter("@IdHienTrangMayCNC", CNC1.IdHienTrangMayCNC);
               var val3 = new SqlParameter("@IdHienTrangCuaMayCNC", CNC1.IdHienTrangCuaMayCNC);
               var val4 = new SqlParameter("@ThoiDiem", CNC1.IdHienTrangCuaMayCNC);
               var CNCval = context.UserProfiles.FromSqlRaw("exec sp_InsertValue @tablename, @IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem ", tablename, IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem).ToList();
             */
            string sql11 = @"SELECT TOP 1 * FROM EventManagerCNC1 ORDER BY ID DESC;";
            string sql12 = @"INSERT into EventManagerCNC1 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            string sql21 = @"SELECT TOP 1 * FROM EventManagerCNC2 ORDER BY ID DESC;";
            string sql22 = @"INSERT into EventManagerCNC2 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            DeserialiseJson(Jsonstr1, StatusVM.instance,sql11,sql12);
            DeserialiseJson(Jsonstr1, StatusVM.instance1, sql21, sql22);
            //timer1.Start();
        }
        private void OnGetRequestData2(string Jsonstr2)
        {
            //timer2.Stop();
            string sql11 = @"SELECT TOP 1 * FROM EventManagerCNC3 ORDER BY ID DESC;";
            string sql12 = @"INSERT into EventManagerCNC3 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            string sql21 = @"SELECT TOP 1 * FROM EventManagerCNC4 ORDER BY ID DESC;";
            string sql22 = @"INSERT into EventManagerCNC4 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            DeserialiseJson(Jsonstr2, StatusVM.instance, sql11, sql12);
            DeserialiseJson(Jsonstr2, StatusVM.instance1, sql21, sql22);
            //timer2.Start();
        }
        private void OnGetRequestData3(string Jsonstr3)
        {
            //timer3.Stop();
            string sql11 = @"SELECT TOP 1 * FROM EventManagerCNC5 ORDER BY ID DESC;";
            string sql12 = @"INSERT into EventManagerCNC5 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            string sql21 = @"SELECT TOP 1 * FROM EventManagerCNC6 ORDER BY ID DESC;";
            string sql22 = @"INSERT into EventManagerCNC6 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            DeserialiseJson(Jsonstr3, StatusVM.instance, sql11, sql12);
            DeserialiseJson(Jsonstr3, StatusVM.instance1, sql21, sql22);
            //timer3.Start();
        }
        private void OnGetRequestData4(string Jsonstr4)
        {
            //timer4.Stop();
            string sql11 = @"SELECT TOP 1 * FROM EventManagerCNC7 ORDER BY ID DESC;";
            string sql12 = @"INSERT into EventManagerCNC7 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            string sql21 = @"SELECT TOP 1 * FROM EventManagerCNC8 ORDER BY ID DESC;";
            string sql22 = @"INSERT into EventManagerCNC8 (IdHienTrangMayCNC, IdHienTrangCuaMayCNC, ThoiDiem) "
                                        + " VALUES (@IdHienTrangMayCNC, @IdHienTrangCuaMayCNC, @ThoiDiem)";
            DeserialiseJson(Jsonstr4, StatusVM.instance, sql11, sql12);
            DeserialiseJson(Jsonstr4, StatusVM.instance1, sql21, sql22);
            //timer4.Start();
        }

        #region oldcode
        private void DeserialiseJson(string Jsonstr, StatusVM ins, string sql1, string sql2)
        {
            try
            {
                if (Jsonstr == null)
                {
                    Utilities.WriteLogError("JSON NULL");
                }
                else
                {
                    var sqlconnectstring = @"Data Source=DESKTOP-QRDMR9C\SQLRAV;
                             Initial Catalog=CNCproject;
                             User ID=sa;Password=13011408Bn";
                    using (SqlConnection connection = new SqlConnection(sqlconnectstring))
                    {
                        connection.Open();
                        int[] lastrs = { 0, 0 };
                        int[] rs = { 0, 0 };
                        using (var command = connection.CreateCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = sql1;
                            var reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int col1 = reader.GetOrdinal("IdHienTrangMayCNC");
                                    int col2 = reader.GetOrdinal("IdHienTrangCuaMayCNC");
                                    lastrs[0] = (int)reader.GetInt32(col1);
                                    lastrs[1] = (int)reader.GetInt32(col2);
                                }
                            }
                            reader.Close();
                            var Jwise = JsonConvert.DeserializeObject<Wise1>(Jsonstr);
                            StatusVM.CheckState(Jwise);
                            if (ins.machinestate == Machine3State.Running)
                            {
                                rs[0] = 1;
                            }
                            else if (ins.machinestate == Machine3State.Stopping)
                            {
                                rs[0] = 2;
                            }
                            else if (ins.machinestate == Machine3State.Falling)
                            {
                                rs[0] = 3;
                            }
                            else if (ins.machinestate == Machine3State.Waiting)
                            {
                                rs[0] = 4;
                            }
                            if (ins.statedoor == MachineDoor2Status.Closing)
                            {
                                rs[1] = 2;
                            }
                            else
                            {
                                rs[1] = 1;
                            }
                            command.CommandText = sql2;
                            command.Parameters.AddWithValue("@IdHienTrangMayCNC", rs[0]);
                            command.Parameters.AddWithValue("@IdHienTrangCuaMayCNC", rs[1]);
                            command.Parameters.AddWithValue("@ThoiDiem", DateTime.Now);
                            if (rs[0] != lastrs[0] || rs[1] != lastrs[1])
                            {
                                if (rs[0] != 0 && rs[1] != 0 && lastrs[0] != 0 && lastrs[1] != 0)
                                {
                                    try
                                    {
                                        int result = command.ExecuteNonQuery();
                                        if (result < 0)
                                        {
                                            Utilities.WriteLogError("ERROR CODE SQL : " + result.ToString());
                                        }
                                    }
                                    catch (SqlException e)
                                    {
                                        Utilities.WriteLogError("ERROR SQL : " + e.Message);
                                    }
                                    finally
                                    {
                                        command.Dispose();
                                    }
                                }    
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Utilities.WriteLogError("ERROR SERVICE : " + e.Message);
            }
            finally
            {
                GC.Collect();
            }
        }
        #endregion
    }
}
