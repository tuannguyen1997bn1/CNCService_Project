
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CNCService
{
    public class DataRequest1
    {
        /// <summary>
        ///emun Http request method
        /// </summary>
        public enum HttpRequestOption
        {
            GET,
            PATCH
        }
        public delegate void ResquestRespondedCallback(string raw_data);
        public delegate void ResquestOccurredErrorCallback(Exception e);
        public class AdvantechHttpWebUtility
        {

            public event ResquestRespondedCallback ResquestResponded;
            public event ResquestOccurredErrorCallback ResquestOccurredError;

            public string BasicAuthAccount1 = "root";
            public string BasicAuthPassword1 = "00000000";
            public string URL1 = @"http://192.168.68.199/di_value/slot_0";

            public string BasicAuthAccount2 = "root"; // có thêm thiết bị sẽ config lại
            public string BasicAuthPassword2 = "00000000";
            public string URL2 = @"http://192.168.68.199/di_value/slot_0";

            public string BasicAuthAccount3 = "root";// có thêm thiết bị sẽ config lại
            public string BasicAuthPassword3 = "00000000";
            public string URL3 = @"http://192.168.68.199/di_value/slot_0";

            public string BasicAuthAccount4 = "root";// có thêm thiết bị sẽ config lại
            public string BasicAuthPassword4 = "00000000";
            public string URL4 = @"http://192.168.68.199/di_value/slot_0";
            public string JsonifyString { get; set; }

            protected bool HasData { get; set; }
            protected HttpRequestOption Method { get; set; }

            public AdvantechHttpWebUtility()
            {
            }
            /// <summary>
            /// Invoke ResquestResponded Callback function
            /// </summary>
            /// <param name="raw_data"></param>
            protected virtual void OnResquestResponded(string raw_data)
            {
                if (ResquestResponded != null)
                {
                    ResquestResponded.Invoke(raw_data);
                }
            }

            /// <summary>
            /// Invoke ResquestOccurredError Callback function
            /// </summary>
            /// <param name="raw_data"></param>
            protected virtual void OnResquestOccurredError(Exception e)
            {
                if (ResquestOccurredError != null)
                {
                    ResquestOccurredError.Invoke(e);
                }
            }
            public void SendGETRequest(string account, string password, string URL)
            {
                //this.BasicAuthAccount1 = account;
                //this.BasicAuthPassword1 = password;
                //this.URL1 = URL;
                this.HasData = false;
                this.Method = HttpRequestOption.GET;
                SendRequest(URL, account, password);
            }
            //public void SendGETRequest2(string account, string password, string URL)
            //{
            //    this.BasicAuthAccount2 = account;
            //    this.BasicAuthPassword2 = password;
            //    this.URL2 = URL;
            //    this.HasData = false;
            //    this.Method = HttpRequestOption.GET;
            //    SendRequest(this.URL2, this.BasicAuthAccount2, this.BasicAuthPassword2);
            //}
            //public void SendGETRequest3(string account, string password, string URL)
            //{
            //    this.BasicAuthAccount3 = account;
            //    this.BasicAuthPassword3 = password;
            //    this.URL3 = URL;
            //    this.HasData = false;
            //    this.Method = HttpRequestOption.GET;
            //    SendRequest(this.URL3, this.BasicAuthAccount3, this.BasicAuthPassword3);
            //}
            //public void SendGETRequest4(string account, string password, string URL)
            //{
            //    this.BasicAuthAccount4 = account;
            //    this.BasicAuthPassword4 = password;
            //    this.URL4 = URL;
            //    this.HasData = false;
            //    this.Method = HttpRequestOption.GET;
            //    SendRequest(this.URL4, this.BasicAuthAccount4, this.BasicAuthPassword4);
            //}
            public void SendPATCHRequest(string account, string password, string URL, string JSONString)
            {
                this.BasicAuthAccount1 = account;
                this.BasicAuthPassword1 = password;
                this.URL1 = URL;
                this.JsonifyString = JSONString;
                this.HasData = true;
                this.Method = HttpRequestOption.PATCH;
                SendRequest(this.URL1, this.BasicAuthAccount1, this.BasicAuthPassword1);
            }
            protected void SendRequest(string url, string account, string pass)
            {
                HttpWebRequest myRequest;
                System.IO.Stream outputStream;// End the stream request operation

                myRequest = (HttpWebRequest)WebRequest.Create(url); // create request
                myRequest.Headers.Add("Authorization", "Basic " +
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(account + ":" + pass)));
                myRequest.Method = Method.ToString();
                myRequest.KeepAlive = false;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ReadWriteTimeout = 1000;
                // Create the patch data
                if (this.HasData)//Append data for send
                {
                    byte[] byData = Encoding.ASCII.GetBytes(this.JsonifyString); // convert POST data to bytes
                    myRequest.ContentLength = byData.Length;
                    // Add the post data to the web request
                    outputStream = myRequest.GetRequestStream();
                    outputStream.Write(byData, 0, byData.Length);
                    outputStream.Close();
                }
                try
                {
                    myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
                }
                catch (Exception e)
                {
                    OnResquestOccurredError(e);
                }
            }
            void GetResponsetStreamCallback(IAsyncResult callbackResult)
            {
                bool bRet = false;
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                string result = "";
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                    using (System.IO.StreamReader httpWebStreamReader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (response.ContentLength > 0)
                            {
                                result = httpWebStreamReader.ReadToEnd();
                            }
                            else
                            {
                                result = ((int)(HttpStatusCode.OK)).ToString() + " " + response.StatusDescription;
                            }
                            bRet = true;
                        }
                        else
                            OnResquestOccurredError(new Exception(response.StatusCode.ToString()));
                    }
                    response.Close();
                }
                catch (Exception e)
                {
                    OnResquestOccurredError(e);
                }
                finally
                {
                    request.Abort();
                    if (bRet)
                        OnResquestResponded(result);
                }
            }
        }
    }
}

