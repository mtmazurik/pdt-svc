namespace pdt.svc.services
{
    public interface IMessageProducer
    {
        void Write(string jsonData);
    }
}