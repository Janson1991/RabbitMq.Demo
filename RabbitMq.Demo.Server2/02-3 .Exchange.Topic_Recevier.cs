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
    /// 手动确认
    /// </summary>
    public class Exchange_Topic_Recevier : IRuning
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
                        //申明fanout类型exchange
                        channel.ExchangeDeclare(exchange: "demo.topic", type: "topic");
                        //申明随机队列名称
                        var queuename = channel.QueueDeclare().QueueName;
                        //绑定队列到topic类型exchange，需指定路由键routingKey
                        channel.QueueBind(queue: queuename, exchange: "demo.topic", routingKey: "second.*.fast");
                        //5. 构造消费者实例
                        var consumer = new EventingBasicConsumer(channel);
                        //6. 绑定消息接收后的事件委托
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" second.*.fast.Received {0}", message);
                        };
                        //启动消费者
                        channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);
                        while (true)
                        {
                        }
                    }
                }
            });
        }
    }
}
