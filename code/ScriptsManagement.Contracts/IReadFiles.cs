namespace ScriptsManagement.Contracts
{
    public interface IReadFiles
    {
        List<string> GetFileNames();
        string ReadFile(string fileName);
    }
}
