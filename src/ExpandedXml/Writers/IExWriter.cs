namespace ExpandedXml.Writers
{
    public interface IExWriter
    {

        void Write<T>(string filePath, IEnumerable<T> list, IEnumerable<ExColumnHeader> exHeaders);

        byte[] Write<T>(IEnumerable<T> list, IEnumerable<ExColumnHeader> exHeaders);
    }
}
