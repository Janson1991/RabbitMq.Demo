using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Server2
{
    /// <summary>
    /// 自动确认
    /// </summary>
    public class Queue_Recevier : IRuning
    {
        public Task Run(ConnectionFactory factory)
        {
            var queueName = "demo.queue";
            return Task.Run(() =>
            {
                using (var connection = factory.CreateConnection())
                {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                    {
                        //4. 申明队列
                        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                            arguments: null);
                        //5. 构造消费者实例
                        var consumer = new EventingBasicConsumer(channel);
                        //6. 绑定消息接收后的事件委托
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" Received {0}", message);
                        };
                        //启动消费者
                        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                        while (true)
                        {
                        }
                    }
                }
            });
        }
    }
}
