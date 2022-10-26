using ClosedXML.Excel;
using ExpandedXml.Readers.CellReaders;

namespace ExpandedXml.Readers
{
    /// <summary>
    /// 시트를 읽고 블록을 반환한다.
    /// </summary>
    public class BlockReader : ICustomize
    {
        public RowReader RowReader { get; } = new();

        public void AddCellReader(Type type, ICellReader cellReader)
        {
            RowReader.AddCellReader(type, cellReader);
        }

        public void AddCellTextConverter(Type type, ICellTextConverter cellTextConverter)
        {
            RowReader.AddCellTextConverter(type, cellTextConverter);
        }

        public ExBlock Read(IEnumerable<IXLRow> rows, IEnumerable<ExColumn> exColumns)
        {
            var exRows = new List<ExRow>();
            int exRowNumber = 1;
            foreach (var row in rows)
            {
                var exRow = RowReader.Read(exRowNumber++, row, exColumns);
                exRows.Add(exRow);
            }

            return new ExBlock(exColumns.ToArray(), exRows.ToArray());
        }
    }
}
