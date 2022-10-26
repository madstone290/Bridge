using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    /// <summary>
    /// 셀 값을 읽는다
    /// </summary>
    public interface ICellReader
    {
        ICellTextConverter? CellTextConverter { get; set; }

        bool TryRead(IXLCell cell, out object? value);
    }

    public interface ICellReader<TValueType> : ICellReader
    {
        bool TryRead(IXLCell cell, out TValueType value);
    }
}
