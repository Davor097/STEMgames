using System;
using System.IO;
using System.Net;
using System.Text;

namespace STEM.Utility
{
    public class RequestManager
    {
        public string LastResponse { protected set; get; }

        public string GetResponseContent(HttpWebResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                dataStream = response.GetResponseStream();
                reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                response.Close();
            }
            LastResponse = responseFromServer;
            return responseFromServer;
        }

        public string SendPOSTRequest(string uri, string content, string authorization)
        {
            HttpWebRequest request = GeneratePOSTRequest(uri, content, authorization);
            return GetResponse(request);
        }

        public string SendGETRequest(string uri, string authorization)
        {
            HttpWebRequest request = GenerateGETRequest(uri, authorization);
            return GetResponse(request);
        }

        public string SendRequest(string uri, string content, string method, string authorization)
        {
            HttpWebRequest request = GenerateRequest(uri, content, method, authorization);
            return GetResponse(request);
        }

        public HttpWebRequest GenerateGETRequest(string uri, string authorization)
        {
            return GenerateRequest(uri, null, "GET", authorization);
        }

        public HttpWebRequest GeneratePOSTRequest(string uri, string content, string authorization)
        {
            return GenerateRequest(uri, content, "POST", authorization);
        }

        internal HttpWebRequest GenerateRequest(string uri, string content, string method, string authorization)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.Headers.Add(HttpRequestHeader.Authorization, authorization);
            request.Credentials = CredentialCache.DefaultNetworkCredentials;

            if (method == "POST")
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                request.ContentType = "text/plain";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            return request;
        }

        internal string GetResponse(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                Console.WriteLine("Web exception occurred. Status code: {0}", ex.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return GetResponseString(response);
        }

        private string GetResponseString(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
            {
                return reader.ReadToEnd();
            }
        }
    }
}