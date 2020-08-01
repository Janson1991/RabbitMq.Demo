using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Client
{
    /// <summary>
    /// （明确的路由规则：消费端绑定的队列名称必须和消息发布时指定的路由名称一致）
    /// </summary>
    public class Exchange_Direct_Sender : IRuning
    {
        /*
         *direct相对于fanout就属于完全匹配、单播的模式
         */
        public Task Run(ConnectionFactory factory)
        {
             Task.Run(() =>
            {
                //2. 建立连接
                using (var connection = factory.CreateConnection())
                {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                    {
                        //使用fanout exchange type，指定exchange名称
                        channel.ExchangeDeclare(exchange: "demo.direct", type: "direct");

                        var index = 0;
                        do
                        {
                            //5. 构建byte消息数据包
                            string message = index + "-direct-1-" + DateTime.Now;
                            var body = Encoding.UTF8.GetBytes(message);
                            //6. 发送数据包
                            channel.BasicPublish(exchange: "demo.direct", routingKey: "direct-1", basicProperties: null, body: body);
                            Console.WriteLine(" [x] Sent {0}", message);
                            Thread.Sleep(1 * 500);
                            index++;
                        } while (true);
                    }
                }
            });

             Task.Run(() =>
            {
                //2. 建立连接
                using (var connection = factory.CreateConnection())
                {
                    //3. 创建信道
                    using (var channel = connection.CreateModel())
                    {
                        //使用fanout exchange type，指定exchange名称
                        channel.ExchangeDeclare(exchange: "demo.direct", type: "direct");

                        var index = 0;
                        do
                        {
                            //5. 构建byte消息数据包
                            string message = index + "-direct-2-" + DateTime.Now;
                            var body = Encoding.UTF8.GetBytes(message);
                            //6. 发送数据包
                            channel.BasicPublish(exchange: "demo.direct", routingKey: "direct-2", basicProperties: null, body: body);
                            Console.WriteLine(" [x] Sent {0}", message);
                            Thread.Sleep(1 * 500);
                            index++;
                        } while (true);
                    }
                }
            });
             return Task.CompletedTask;
        }
    }
}
