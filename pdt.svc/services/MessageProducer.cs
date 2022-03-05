using Confluent.Kafka;


namespace pdt.svc.services
{
    public class MessageProducer : IMessageProducer
    {
        private ProducerConfig producerConfig;
        private string topic;
        public MessageProducer(string bootstrapServers, string saslUsername, string saslPassword)
        {
            producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                SaslMechanism = SaslMechanism.Plain,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslUsername = saslUsername,
                SaslPassword = saslPassword
            };
            topic = "prod.search-results";
        }
        public void Write(string jsonData)
        {
            using (var p = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                Message<Null, string> message = new Message<Null, string> { Value = jsonData };
                p.Produce(topic, message, handler);
                p.Flush(TimeSpan.FromSeconds(10));
            }

        }
        Action<DeliveryReport<Null, string>> handler = r => Console.WriteLine(!r.Error.IsError ? $"Delivered message to {r.TopicPartitionOffset}" : $"Delivery Error: {r.Error.Reason}");
    }
}
