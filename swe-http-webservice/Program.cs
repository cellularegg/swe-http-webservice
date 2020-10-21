// "Inspired" by: https://youtu.be/HFnJLv2Q1go
using System;
using System.Diagnostics;
using HTTPServerLib;

namespace swe_http_webservice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Port on 8080");
            HTTPServer server = new HTTPServer(8080);
            server.Start();
        }
    }
}
