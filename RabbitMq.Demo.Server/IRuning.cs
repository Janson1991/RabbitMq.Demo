using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Server
{
    public interface IRuning
    {
        Task Run(ConnectionFactory factory);
    }
}
