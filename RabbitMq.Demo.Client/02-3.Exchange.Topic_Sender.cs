using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMq.Demo.Client
{
    /// <summary>
    /// （模式匹配的路由规则：支持通配符） 
    /// </summary>
    public class Exchange_Topic_Sender : IRuning
    {
        /*
         * topic是direct的升级版，是一种模式匹配的路由机制。它支持使用两种通配符来进行模式匹配：符号#和符号*。其中*匹配一个单词， #则表示匹配0个或多个单词，单词之间用.分割
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
                       channel.ExchangeDeclare(exchange: "demo.topic", type: "topic");

                       var index = 0;
                       do
                       {
                           //5. 构建byte消息数据包
                           string message = index + "- first.topic.fast -" + DateTime.Now;
                           var body = Encoding.UTF8.GetBytes(message);
                           //6. 发送数据包
                           //发布到topic类型exchange，必须指定routingKey
                           channel.BasicPublish(exchange: "demo.topic", routingKey: "first.topic.fast", basicProperties: null, body: body);
                           Console.WriteLine(" first.topic.fast Sent {0}", message);
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
                        channel.ExchangeDeclare(exchange: "demo.topic", type: "topic");

                        var index = 0;
                        do
                        {
                            //5. 构建byte消息数据包
                            string message = index + "- second.topic.fast -" + DateTime.Now;
                            var body = Encoding.UTF8.GetBytes(message);
                            //6. 发送数据包
                            //发布到topic类型exchange，必须指定routingKey
                            channel.BasicPublish(exchange: "demo.topic", routingKey: "second.topic.fast", basicProperties: null, body: body);
                            Console.WriteLine(" second.topic.fast Sent {0}", message);
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
