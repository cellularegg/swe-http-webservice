using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace HTTPServerLib
{
    class ResponseContext
    {
        private string _Body = null;
        private string _Status;
        private string _ContentType;

        private ResponseContext(string status, string contentType, string body)
        {
            this._ContentType = contentType;
            this._Status = status;
            this._Body = body;
        }

        public static ResponseContext From(RequestContext request)
        {
            if (request == null)
            {
                return NullResponse();
            }
            if (request.Type == "GET")
            {
                if (request.Path == "/hello")
                {
                    string body = "{\"Hello\":\"World\"}";
                    return new ResponseContext("200 OK", "application/json", body);
                }
                else
                {
                    return PageNotFoundResponse();
                }
            }
            else
            {
                return MethodNotAllowedResponse();
            }
        }

        private static ResponseContext NullResponse()
        {
            return new ResponseContext("400 Bad Request", "text/plain", "Error bad request");
        }
        private static ResponseContext PageNotFoundResponse()
        {
            return new ResponseContext("404 Bad Request", "text/plain", "Error page not found");
        }
        private static ResponseContext MethodNotAllowedResponse()
        {
            return new ResponseContext("405 Bad Request", "text/plain", "Error method not allowed");
        }
        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(HTTPServer.VERSION + " " + _Status);
            sb.AppendLine("Content-Type: " + _ContentType);
            sb.AppendLine("Content-Length: " + Encoding.UTF8.GetBytes(_Body).Length);
            sb.AppendLine();
            sb.AppendLine(_Body);
            System.Diagnostics.Debug.WriteLine(sb.ToString());
            writer.Write(sb.ToString());
        }
    }
}
