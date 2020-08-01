using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitMq.Demo.Client
{
    class Program
    {
        static void Main(string[] args)
        {  
            //1.1.实例化连接工厂
            var factory = new ConnectionFactory() { HostName = "www.coderzi.com", Password = "123abc", UserName = "test" };
            IRuning runing = new Rpc_Sender();
            runing.Run(factory);
            Console.ReadKey();
            ////2. 建立连接
            //using (var connection = factory.CreateConnection())
            //{
            //    //3. 创建信道
            //    using (var channel = connection.CreateModel())
            //    {
            //        ////4. 申明队列
            //        //channel.QueueDeclare(queue: "test", durable: false, exclusive: false, autoDelete: false,
            //        //    arguments: null);

            //        // 生成随机队列名称
            //        var queueName = channel.QueueDeclare().QueueName;
            //        //使用fanout exchange type，指定exchange名称
            //        channel.ExchangeDeclare(exchange: "fanoutEC", type: "fanout");

            //        var index = 0;
            //        do
            //        {
            //            //5. 构建byte消息数据包
            //            string message = args.Length > 0 ? args[0] : index + "-Hello RabbitMQ!" + DateTime.Now;
            //            var body = Encoding.UTF8.GetBytes(message);
            //            ////6. 发送数据包
            //            //channel.BasicPublish(exchange: "", routingKey: "test", basicProperties: null, body: body);

            //            //发布到指定exchange，fanout类型无需指定routingKey
            //            channel.BasicPublish(exchange: "fanoutEC", routingKey: "", basicProperties: null, body: body);

            //            Console.WriteLine(" [x] Sent {0}", message);
            //            Thread.Sleep(1 * 500);
            //            index++;

            //        } while (true);
            //        Console.ReadKey();
            //    }
            //}
        }
    }
}
