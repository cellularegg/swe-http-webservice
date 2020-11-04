using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace HTTPServerLib
{
    public class RequestContext
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }
        public string Body { get; set; }
        // TODO Change Headers to Dict
        public Dictionary<string, string> Headers { get; set; }
        // TODO Add body for POST request.

        private RequestContext(string type, string path, string host, Dictionary<string, string> headers, string body = "")
        {
            this.Type = type;
            this.Path = path;
            this.Host = host;
            this.Headers = headers;
            this.Body = body;
        }

        public static RequestContext GetRequestContext(string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return null;
            }
            string[] content = request.Split('\n');
            string type = content[0].Split(' ')[0];
            string path = content[0].Split(' ')[1];
            Dictionary<string, string> headers = new Dictionary<string, string>();
            int bodyStartIdx = -1;
            string body = "";
            // Read Headers
            for (int i = 1; i < content.Length; i++)
            {
                if (string.IsNullOrEmpty(content[i]) || content[i] == "\r")
                {
                    bodyStartIdx = i;
                    break;
                }
                // Headers are case-insensetive -> https://www.w3.org/Protocols/rfc2616/rfc2616-sec4.html#sec4.2
                // Every header to lowercase
                string[] tmp = content[i].ToLower().Split(' ');
                if (tmp.Length == 2)
                {
                    // Remove ':'
                    tmp[0] = tmp[0].Remove(tmp[0].Length - 1);
                    headers.Add(tmp[0], tmp[1]);
                }
            }
            // Read Body 
            if (bodyStartIdx != -1)
            {
                for (int i = bodyStartIdx + 1; i < content.Length; i++)
                {
                    body += content[i] + "\n";
                }
            }
            return new RequestContext(type, path, headers["host"], headers, body);
        }

        public override string ToString()
        {
            string tmp = $"RequestContext:{Environment.NewLine}";
            tmp += $"\tType: {this.Type}{Environment.NewLine}";
            tmp += $"\tPath {this.Path}{Environment.NewLine}";
            tmp += $"\tHost: {this.Host}{Environment.NewLine}";
            tmp += $"\tHeaders: {Environment.NewLine}";
            foreach (KeyValuePair<string, string> header in this.Headers)
            {
                tmp += $"\t\t {header.Key}:{header.Value}{Environment.NewLine}";
            }
            return tmp;
        }
    }
}
