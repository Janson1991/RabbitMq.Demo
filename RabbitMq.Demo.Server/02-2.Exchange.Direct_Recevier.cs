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
    public class Exchange_Direct_Recevier : IRuning
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
                        channel.ExchangeDeclare(exchange: "demo.direct", type: "direct");
                        //申明随机队列名称
                        var queuename = channel.QueueDeclare().QueueName;
                        //绑定队列到指定fanout类型exchange，无需指定路由键
                        channel.QueueBind(queue: queuename, exchange: "demo.direct", routingKey: "direct-1");

                        //设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时
                        channel.BasicQos(0, 1, false);
                        //5. 构造消费者实例
                        var consumer = new EventingBasicConsumer(channel);
                        //6. 绑定消息接收后的事件委托
                        consumer.Received += (model, ea) =>
                        {
                            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                            Console.WriteLine(" Direct-1.Received {0}", message);
                            Thread.Sleep(5 * 1000); //模拟耗时
                            // 7. 发送消息确认信号（手动消息确认）
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        };
                        //启动消费者
                        channel.BasicConsume(queue: queuename, autoAck: false, consumer: consumer);
                        while (true)
                        {
                        }
                    }
                }
            });
        }
    }
}
