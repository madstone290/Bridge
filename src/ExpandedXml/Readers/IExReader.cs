using ClosedXML.Excel;

namespace ExpandedXml.Readers
{
    public interface IExReader : ICustomize
    {
        IEnumerable<T> Read<T>(Stream stream, IEnumerable<ExColumnHeader> exHeaders, int sheetNumber = 1);

        IEnumerable<T> Read<T>(byte[] data, IEnumerable<ExColumnHeader> exHeaders, int sheetNumber = 1);

        IEnumerable<T> Read<T>(IXLWorksheet worksheet, IEnumerable<ExColumnHeader> exHeaders);

    }
}
