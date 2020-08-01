using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "www.coderzi.com", Password = "123abc", UserName = "test" };
            IRuning runing = new Rpc_Recevier();
            runing.Run(factory);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
