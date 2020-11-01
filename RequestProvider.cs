using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MyCar
{
    class RequestProvider
    {
        private string endpointUri;

        public int Id { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public RequestProvider(string endpointUri)
        {
            this.endpointUri = endpointUri;
        }

        public byte[] performPost()
        {
            WebRequest request = WebRequest.Create(endpointUri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = ParamsByteArray();
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Stream dataStreamResponse = response.GetResponseStream();
            byte[] responseBody = Encoding.ASCII.GetBytes(new StreamReader(dataStreamResponse).ReadToEnd());
            response.Close();
            return responseBody;

        }

        public byte[] performGet()
        {
            WebRequest request = WebRequest.Create(endpointUri + Id.ToString());
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream dataStreamResponse = response.GetResponseStream();
            byte[] responseBody = Encoding.ASCII.GetBytes(new StreamReader(dataStreamResponse).ReadToEnd());
            response.Close();
            return responseBody;
        }

        private byte[] ParamsByteArray()
        {
            string result = String.Empty;
            foreach (var key in Parameters)
            {
                result += key.Key + '=' + key.Value + '&';
            }
            result.Remove(result.Length - 1);
            byte[] bytes = Encoding.ASCII.GetBytes(result);
            return bytes;
        }
    }
}
