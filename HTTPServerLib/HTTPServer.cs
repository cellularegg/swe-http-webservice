using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HTTPServerLib
{
    public class HTTPServer
    {
        public const string VERSION = "HTTP/1.1";
        public int port { get; private set; }
        public bool isRunning { get; private set; }
        private TcpListener _tcpListener;

        public HTTPServer(int pPort)
        {
            // TODO Check for valid Port
            port = pPort;
            _tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            isRunning = true;
            _tcpListener.Start();
            while (isRunning)
            {
                Console.WriteLine("Waiting for connection.");
                TcpClient client = _tcpListener.AcceptTcpClient();
                Console.WriteLine("Client connected.");
                HandleClient(client);
                client.Close();
            }
            isRunning = false;
            _tcpListener.Stop();

        }

        private void HandleClient(TcpClient client)
        {
            StreamReader sr = new StreamReader(client.GetStream());
            string msg = "";
            while (sr.Peek() != -1)
            {
                // sr.ReadLine() it gets stuck when receiving POST requests
                msg += (char)sr.Read();
            }

            RequestContext req = RequestContext.GetRequestContext(msg);
            Console.WriteLine(req);
            ResponseContext rsp = ResponseContext.From(req);
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            Console.WriteLine(rsp.GetAsString(true));
            writer.Write(rsp.GetAsString(false));
        }
    }
}
