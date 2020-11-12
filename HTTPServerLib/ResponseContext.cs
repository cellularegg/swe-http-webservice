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
            MessageCollection _msgColl = MessageCollection.GetMessageCollection();
            switch (request.Method)
            {
                case RequestMethod.GET:
                    if (request.Path == "/hello")
                    {
                        string body = "{\"Hello\":\"World\"}";
                        return new ResponseContext("200 OK", "application/json", body);
                    }
                    else if (request.Path == "/messages")
                    {
                        // TODO CHECK FOR EMTPY body -> Send correct "Error"
                        string body = _msgColl.GetMessagesArrayAsJson();
                        return new ResponseContext("200 OK", "application/json", body);
                    }
                    return PageNotFoundResponse();
                case RequestMethod.POST:
                    if (request.Path == "/messages")
                    {
                        string msgContent = _msgColl.GetMsgContentFromJson(request.Body);
                        if (msgContent == string.Empty)
                        {
                            return new ResponseContext("400 Bad Request", "text/plain", "Error, Msg was not in the correct JOSN Format.\n");
                        }
                        else
                        {
                            _msgColl.AddMessage(msgContent);
                            return new ResponseContext("201 Created", "text/plain", "Message successfully created.\n");
                        }
                    }
                    return PageNotFoundResponse();
                //break;
                case RequestMethod.PUT:
                    if (request.Path == "/messages")
                    {
                        Tuple<int, string> msg = _msgColl.GetMsgTupleFromJson(request.Body);
                        if (msg == null)
                        {
                            return new ResponseContext("400 Bad Request", "text/plain", "Error, Msg was not in the correct JOSN Format.\n");
                        }
                        else
                        {
                            if (_msgColl.UpdateMessage(msg.Item1, msg.Item2))
                            {
                                return new ResponseContext("200 OK", "text/plain", "Message successfully edited.\n");
                            }
                            // TODO Maybe change status code to 400?
                            return new ResponseContext("500 Internal Server Error", "text/plain", $"Error, when editing msg with Id: {msg.Item1}.\n");
                        }
                    }
                    return PageNotFoundResponse();
                //break;
                case RequestMethod.DELETE:
                //break;
                default:
                    return MethodNotAllowedResponse();
            }
        }

        private static ResponseContext NullResponse()
        {
            return new ResponseContext("400 Bad Request", "text/plain", "Error bad request\n");
        }
        private static ResponseContext PageNotFoundResponse()
        {
            return new ResponseContext("404 Bad Request", "text/plain", "Error page not found\n");
        }
        private static ResponseContext MethodNotAllowedResponse()
        {
            return new ResponseContext("405 Bad Request", "text/plain", "Error method not allowed\n");
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
