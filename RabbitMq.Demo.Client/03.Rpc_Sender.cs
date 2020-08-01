using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Client
{
    public class Rpc_Sender : IRuning
    {
        public Task Run(ConnectionFactory factory)
        {
           return Task.Run(() =>
            {
                //2. 建立连接
                using (var connection = factory.CreateConnection())
                {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                    {
        
                        var index = 0;
                        do
                        {
                            //申明唯一guid用来标识此次发送的远程调用请求
                            var correlationId = Guid.NewGuid().ToString();
                            //申明需要监听的回调队列
                            var replyQueue = channel.QueueDeclare().QueueName;
                            var properties = channel.CreateBasicProperties();
                            properties.ReplyTo = replyQueue;//指定回调队列
                            properties.CorrelationId = correlationId;//指定消息唯一标识
                            string number = index.ToString();
                            var body = Encoding.UTF8.GetBytes(number);
                            //发布消息
                            channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: properties, body: body);
                            Console.WriteLine($"[*] Request fib({number})");
                            // //创建消费者用于处理消息回调（远程调用返回结果）
                            var callbackConsumer = new EventingBasicConsumer(channel);
                            channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: callbackConsumer);
                            callbackConsumer.Received += (model, ea) =>
                            {
                                //仅当消息回调的ID与发送的ID一致时，说明远程调用结果正确返回。
                                if (ea.BasicProperties.CorrelationId == correlationId)
                                {
                                    var responseMsg = $"{number} - Get Response: {Encoding.UTF8.GetString(ea.Body.ToArray())}";
                                    Console.WriteLine($"[x]: {responseMsg}");
                                }
                            };

                            Thread.Sleep(3 * 1000);
                            index++;
                        } while (true);
                    }
                }
            });
        }
    }
}
