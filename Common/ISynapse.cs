namespace AIkailo.Common
{
    public interface ISynapse
    {
        string Id { get; set; }
        INeuron Source { get; set; }
        INeuron Target { get; set; }
    }
}