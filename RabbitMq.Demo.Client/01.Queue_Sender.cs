using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Client
{
    /// <summary>
    /// 队列
    /// </summary>
    public class Queue_Sender : IRuning
    {
        public Task Run(ConnectionFactory factory)
        {
            var queueName = "demo.queue";
            return Task.Run(() =>
             {
                //2. 建立连接
                using (var connection = factory.CreateConnection())
                 {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                     {
                        //4. 申明队列
                        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                             arguments: null);
                         var index = 0;
                         do
                         {
                            //5. 构建byte消息数据包
                            string message = index + "-Hello RabbitMQ!" + DateTime.Now;
                             var body = Encoding.UTF8.GetBytes(message);
                            //6. 发送数据包
                            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                             Console.WriteLine(" [x] Sent {0}", message);
                             Thread.Sleep(1 * 500);
                             index++;
                         } while (true);
                     }
                 }
             });
        }
    }
}
