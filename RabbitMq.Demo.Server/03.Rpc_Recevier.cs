using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Server
{
    /// <summary>
    /// 手动确认
    /// </summary>
    public class Rpc_Recevier : IRuning
    {
        public Task Run(ConnectionFactory factory)
        {
            return Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                    {
                        //申明队列接收远程调用请求
                        channel.QueueDeclare(queue: "rpc_queue", durable: false,
                            exclusive: false, autoDelete: false, arguments: null);
                        var consumer = new EventingBasicConsumer(channel);
                        Console.WriteLine("[*] Waiting for message.");
                        //请求处理逻辑
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            int n = int.Parse(message);
                            Console.WriteLine($"Receive request of Fib({n})");
                            int result = Fib(n);
                            //从请求的参数中获取请求的唯一标识，在消息回传时同样绑定
                            var properties = ea.BasicProperties;
                            var replyProerties = channel.CreateBasicProperties();
                            replyProerties.CorrelationId = properties.CorrelationId;
                            //将远程调用结果发送到客户端监听的队列上
                            channel.BasicPublish(exchange: "", routingKey: properties.ReplyTo,
                                basicProperties: replyProerties, body: Encoding.UTF8.GetBytes(result.ToString()));
                            //手动发回消息确认
                            channel.BasicAck(ea.DeliveryTag, false);
                            Console.WriteLine($"Return result: Fib({n})= {result}");
                        };
                        channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
                        while (true)
                        {
                        }
                    }
                }
            });
        }

        public int Fib(int n)
        {
            return n * n;
        }
    }
}
