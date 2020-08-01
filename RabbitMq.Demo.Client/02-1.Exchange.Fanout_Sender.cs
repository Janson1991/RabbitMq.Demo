using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Client
{
    /// <summary>
    /// （消息广播，将消息分发到exchange上绑定的所有队列上）
    /// </summary>
    public class Exchange_Fanout_Sender : IRuning
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
                        //使用fanout exchange type，指定exchange名称
                        channel.ExchangeDeclare(exchange: "demo.fanout", type: "fanout");

                        var index = 0;
                        do
                        {
                            //5. 构建byte消息数据包
                            string message = index + "-fanout-" + DateTime.Now;
                            var body = Encoding.UTF8.GetBytes(message);
                            //6. 发送数据包
                            channel.BasicPublish(exchange: "demo.fanout", routingKey: "", basicProperties: null, body: body);
                            Console.WriteLine(" Sent {0}", message);
                            Thread.Sleep(1 * 500);
                            index++;
                        } while (true);
                    }
                }
            });
        }
    }
}
