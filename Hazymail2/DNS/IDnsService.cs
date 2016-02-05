namespace Hazymail.DNS
{
    public interface IDnsService
    {
        string[] GetMxRecords(string domain);
    }
}