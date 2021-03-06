﻿using System;
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
        public RequestMethod Method { get; set; }
        public string Path { get; set; }
        public string Host { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        private RequestContext(RequestMethod method, string path, string host, Dictionary<string, string> headers, string body = "")
        {
            this.Method = method;
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
            request = request.Replace("\r", string.Empty);
            string[] content = request.Split('\n');

            string methodString = content[0].Split(' ')[0];
            RequestMethod requestMethod;
            string path = content[0].Split(' ')[1];
            Dictionary<string, string> headers = new Dictionary<string, string>();
            int bodyStartIdx = -1;
            string body = string.Empty;
            // Parse String to Enum
            try
            {
                requestMethod = (RequestMethod)Enum.Parse(typeof(RequestMethod), methodString, true);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ERROR: {methodString} is not a member of RequestMethod enum. Exception: ${ex}");
                return null;
            }
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
                    if (!string.IsNullOrWhiteSpace(content[i]))
                    {
                        body += content[i] + Environment.NewLine;
                    }
                }
            }
            if (headers.ContainsKey("host"))
            {
                return new RequestContext(requestMethod, path, headers["host"], headers, body);
            }
            return null;
        }

        public override string ToString()
        {

            string tmp = $"----------------------------------------------------------{Environment.NewLine}";
            tmp += $"RequestContext:{Environment.NewLine}";
            tmp += $"\tType: {this.Method}{Environment.NewLine}";
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
