using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HTTPServerLib
{
    public class RequestContext
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }
        // TODO Change Headers to Dict
        public Dictionary<string, string> Headers { get; set; }
        // TODO Add body for POST request.

        private RequestContext(string type, string path, string host, Dictionary<string, string> headers)
        {
            this.Type = type;
            this.Path = path;
            this.Host = host;
            this.Headers = headers;
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
            for (int i = 1; i < content.Length; i++)
            {
                if (string.IsNullOrEmpty(content[i]))
                {
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
            return new RequestContext(type, path, headers["host"], headers);
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
