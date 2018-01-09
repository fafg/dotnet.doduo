namespace dotnet.doduo.MessageBroker.Model
{
    public enum ProducerResponseType
    {
        Ok = System.Net.HttpStatusCode.OK,
        Faiure = System.Net.HttpStatusCode.InternalServerError,
        Running = 0
    }
}