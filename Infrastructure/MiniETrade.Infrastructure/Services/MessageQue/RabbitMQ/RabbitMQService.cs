using Microsoft.Extensions.Configuration;
using MiniETrade.Application.Common.Abstractions.MessageQue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services.MessageQue.RabbitMQ
{
    public class RabbitMQService : IMQPublisherService, IMQConsumerService
    {
        private readonly IConfiguration _configuration;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConnection GetConnection()
        {
            ConnectionFactory factory = new()
            {
                Uri = new(_configuration["RabbitMQ:Uri"])
            };

            return factory.CreateConnection();
        }

        public void Publish()
        {
            using IConnection connection= GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue:"example-queue", exclusive: false, durable : true);
            //durable true ise : kuyruk diske kaydedilir ve olası bir RabbitMQ break-down sonrası hala kuyruk mevcut olur. false olursa diske yazılmaz. Olası bir restart sonrası kuyruklar uçar.
            //exclusive true ise: sadece bu connection ile bu kuyruk consume edilebilir. Connection kapanınca kuyruk silinir.Genelde request-response iletişiminde kullanılan geçici kuyruklar için kullanılır
            //autoDelete true ise : kuyruğa subscribe olan consumer kalmayınca kuyruk silinir.


            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true; 
            //Bu configler şart değildir.
            //Burası tabi yine mesajın kalıcılığı ile ilgili bir mesele. Mesaj silinmesin istiyoruz. Aşağıya parametre olarak gönderdik. Yukarıda ise durable true olmalı.
            //!! Bunlar güzel önlemler olsa da unutmayalım ki mesajların kaybolmayacağının garantisini vermez. Outbox-Inbox design pattern ile garantiye almalıyız.

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            //Bu configler şart değildir.
            //prefetchSize : consumer tarafından alınabileek byte cinsinden en büyük mesaj boyutu. 0 sınırsız demek
            //prefetchCount : consumer tarafından aynı anda alınabilecek mesaj sayısı, 1 dedik böylece 1 işlenmeden başka mesaj consumera gönderilmeyecek.
            //global : Bu configürasyon tüm consumerlar için mi geçerli olsun, yoksa çağrı yapılan consumer için mi.
            //Yukarıdaki configürasyon ile Fair Dispatch yapmış oluyoruz.

            byte[] message = Encoding.UTF8.GetBytes("Hello Rabbit MQ");
            channel.BasicPublish(exchange: "", routingKey: "example-queue", body : message, basicProperties: properties);
            //Exchange parametresi boş olunca default olan direct-exchange çalışacak. O da direct routingKey ile çalışacak.

            #region DirectExchange
            //Direct-Exchange örneğidir. Direct olarak routing-key kuyruğuna ekleme yapar. Bu kuyruğu consume eden tüm consumerlara mesaj iletişmiş olur. 4 consumer da bu routingkeye bakıyorsa 4ü de consume edecektir.
            //channel.ExchangeDeclare(exchange: "direct-exchange-hus", type: ExchangeType.Direct); //Normalde default bu exchange var.
            //channel.BasicPublish(exchange: "direct-exchange-hus", routingKey : "direct-queue-example", body: message);
            #endregion

            #region FanoutExchange
            //Bu exchange modelinde kuyruk adı farketmeksizin bu exchange'e bind olmuş tüm kuyruklara mesaj gönderilecektir.
            //channel.ExchangeDeclare(
            //    exchange : "fanout-exchange-example", 
            //    type: ExchangeType.Fanout, 
            //    durable: true,
            //    autoDelete: false);
            //channel.BasicPublish(
            //    exchange: "fanout-exchange-example", 
            //    routingKey: String.Empty,
            //    body: message);
            #endregion

            #region TopicExchange
            //channel.ExchangeDeclare(
            //    exchange: "topic-exchange-example",
            //    type: ExchangeType.Topic
            //    );
            //channel.BasicPublish(exchange: "topic-exchange-example",
            //    routingKey: "log.error",
            //    body: message
            //    );
            #endregion

            #region HeaderExchange
            //channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);
            //var basicProperties = channel.CreateBasicProperties();
            //basicProperties.Headers = new Dictionary<string, object>()
            //{
            //    ["x-match"] = "all" //Varsayılan olarak x-match "any"dir. All olunca tüm key-value'lerin eşleşmesi beklenir. Any deyince herhangi biri eşleşse yeterlidir.
            //    ["no"] = "256"
            //};
            //channel.BasicPublish(exchange: "header-exchange-example", routingKey: string.Empty, body: message, basicProperties: basicProperties);
            #endregion

            #region P2P (Point-to-Point) Tasarımı
            //string queueName = "example-p2p-queue";

            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //byte[] message = Encoding.UTF8.GetBytes("merhaba");
            //channel.BasicPublish(
            //    exchange: string.Empty,
            //    routingKey: queueName,
            //    body: message);
            #endregion

            #region Publish/Subscribe (Pub/Sub) Tasarımı
            //string exchangeName = "example-pub-sub-exchange";

            //channel.ExchangeDeclare(
            //    exchange: exchangeName,
            //    type: ExchangeType.Fanout);

            //for (int i = 0; i < 100; i++)
            //{
            //    await Task.Delay(200);

            //    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);

            //    channel.BasicPublish(
            //        exchange: exchangeName,
            //        routingKey: string.Empty,
            //        body: message);
            //}

            #endregion

            #region Work Queue(İş Kuyruğu) Tasarımı​
            //string queueName = "example-work-queue";

            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //for (int i = 0; i < 100; i++)
            //{
            //    await Task.Delay(200);

            //    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);

            //    channel.BasicPublish(
            //        exchange: string.Empty,
            //        routingKey: queueName,
            //        body: message);
            //}

            #endregion

            #region Request/Response Tasarımı​
            //string requestQueueName = "example-request-response-queue";
            //channel.QueueDeclare(
            //    queue: requestQueueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //string replyQueueName = channel.QueueDeclare().QueueName;

            //string correlationId = Guid.NewGuid().ToString();

            //#region Request Mesajını Oluşturma ve Gönderme
            //IBasicProperties properties = channel.CreateBasicProperties();
            //properties.CorrelationId = correlationId;
            //properties.ReplyTo = replyQueueName;

            //for (int i = 0; i < 10; i++)
            //{
            //    byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
            //    channel.BasicPublish(
            //        exchange: string.Empty,
            //        routingKey: requestQueueName,
            //        body: message,
            //        basicProperties: properties);
            //}
            //#endregion
            //#region Response Kuyruğu Dinleme
            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue: replyQueueName,
            //    autoAck: true,
            //    consumer: consumer);

            //consumer.Received += (sender, e) =>
            //{
            //    if (e.BasicProperties.CorrelationId == correlationId)
            //    {
            //        //....
            //        Console.WriteLine($"Response : {Encoding.UTF8.GetString(e.Body.Span)}");
            //    }
            //};
            #endregion

        }

        public void Consume()
        {
            using IConnection connection = GetConnection();
            using IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true); //Publisher ile birebir aynı olmalı.
            EventingBasicConsumer consumer = new(channel);
            channel.BasicConsume(queue : "example-queue", false, consumer);//autoArk için false dedik, böylece message acknowledgement sistemini aktif etmiş olduk. Bu da bir mesajın kuyruktan silinmesi için bizim onayımızın olması gerektiği sistemdir. 

            #region DirectExchange
            //Direct-Exchange örneğidir.
            //channel.ExchangeDeclare(exchange: "direct-exchange-hus", type: ExchangeType.Direct);
            //string queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(queue: queueName, exchange: "direct-exchange-hus", routingKey: "direct-queue-example");
            //EventingBasicConsumer consumer2 = new(channel);
            //channel.BasicConsume(queue: queueName, autoAck : false, consumer: consumer2);
            #endregion

            #region FanoutExchange
            //channel.ExchangeDeclare(
            //    exchange: "fanout-exchange-example",
            //    type: ExchangeType.Fanout,
            //    durable: true,
            //    autoDelete: false);
            //channel.QueueDeclare(queue: "fanout-queue-example1");
            //channel.QueueBind(queue: "fanout-queue-example", exchange: "fanout-exchange-example1", routingKey : string.Empty);
            //channel.QueueDeclare(queue: "fanout-queue-example2");
            //channel.QueueBind(queue: "fanout-queue-example", exchange: "fanout-exchange-example2", routingKey: string.Empty);
            //EventingBasicConsumer consumer3 = new(channel);
            //channel.BasicConsume(queue: "fanout-queue-example1", autoAck : false, consumer: consumer3);
            //channel.BasicConsume(queue: "fanout-queue-example2", autoAck : false, consumer: consumer3);
            #endregion

            #region TopicExchange
            //channel.ExchangeDeclare(
            //    exchange: "topic-exchange-example",
            //    type: ExchangeType.Topic
            //    );
            //var queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(
            //    queue: queueName,
            //    exchange : "topic-exchange-example",
            //    routingKey: "log.error"
            //    );
            //EventingBasicConsumer consumer4 = new(channel);
            //channel.BasicConsume(queue: queueName, autoAck: true, consumer4);
            #endregion

            #region HeaderExchange
            //channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);
            //var queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(
            //    queue: queueName,
            //    exchange: "header-exchange-example",
            //    routingKey: string.Empty,
            //    new Dictionary<string, object>
            //    {
            //        ["x-match"] = "all",
            //        ["no"] = "256" //dictionary initializer C# 6.0
            //    });
            //EventingBasicConsumer consumer5 = new(channel);
            //channel.BasicConsume(queue: queueName, autoAck: true, consumer5);
            #endregion

            #region P2P (Point-to-Point) Tasarımı
            //string queueName = "example-p2p-queue";

            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue: queueName,
            //    autoAck: false,
            //    consumer: consumer);

            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};
            #endregion

            #region Publish/Subscribe (Pub/Sub) Tasarımı
            //string exchangeName = "example-pub-sub-exchange";

            //channel.ExchangeDeclare(
            //    exchange: exchangeName,
            //    type: ExchangeType.Fanout);

            //string queueName = channel.QueueDeclare().QueueName;
            //channel.QueueBind(
            //    queue: queueName,
            //    exchange: exchangeName,
            //    routingKey: string.Empty);

            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue: queueName,
            //    autoAck: false,
            //    consumer: consumer);

            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};


            #endregion

            #region Work Queue(İş Kuyruğu) Tasarımı​
            //string queueName = "example-work-queue";

            //channel.QueueDeclare(
            //    queue: queueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //EventingBasicConsumer consumer = new(channel);
            //channel.BasicConsume(
            //    queue: queueName,
            //    autoAck: true,
            //    consumer: consumer);

            //channel.BasicQos(
            //    prefetchCount: 1,
            //    prefetchSize: 0,
            //    global: false);

            //consumer.Received += (sender, e) =>
            //{
            //    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
            //};
            #endregion

            #region Request/Response Tasarımı​

            //string requestQueueName = "example-request-response-queue";
            //channel.QueueDeclare(
            //    queue: requestQueueName,
            //    durable: false,
            //    exclusive: false,
            //    autoDelete: false);

            //EventingBasicConsumer consumer7 = new(channel);
            //channel.BasicConsume(
            //    queue: requestQueueName,
            //    autoAck: true,
            //    consumer: consumer7);

            //consumer7.Received += (sender, e) =>
            //{
            //    string message = Encoding.UTF8.GetString(e.Body.Span);
            //    Console.WriteLine(message);
            //    //.....
            //    byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı. : {message}");
            //    IBasicProperties properties = channel.CreateBasicProperties();
            //    properties.CorrelationId = e.BasicProperties.CorrelationId;
            //    channel.BasicPublish(
            //        exchange: string.Empty,
            //        routingKey: e.BasicProperties.ReplyTo,
            //        basicProperties: properties,
            //        body: responseMessage);
            //};

            #endregion

            consumer.Received += (sender, e) =>
            {
                //Kuyruğa gelen mesajın işlendiği yerdir.
                //e.Body --> kuyruktaki mesajın verisini bütünsel olarak getirecektir.
                //e.Body.Span veya e.Body.ToArray() : Kuyruktaki mesajın byte verisini getirecektir.
                Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span) );
                bool isHusoHappy = true;
                channel.BasicAck(e.DeliveryTag, false); //Message Acknowledgement sistemine onay gönderiyoruz. multiple : true dersek bundan önceki tüm message'ları da kuyruktan çıkarır.
                if (!isHusoHappy)
                {
                    channel.BasicNack(e.DeliveryTag, false, true); //Mesajın başarısız olduğunu söylüyoruz. Ve son parametre re-queue ile tekrar kuyruğa sokulmasını sağlıyoruz.
                }
            };
        }
    }
}
