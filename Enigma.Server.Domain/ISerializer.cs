namespace Enigma.Server.Domain
{
    public interface ISerializer
    {
        T Deserialize<T>(string val);
        string Serialize(object obj);
    }
}
