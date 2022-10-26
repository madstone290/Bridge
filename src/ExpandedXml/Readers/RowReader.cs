using ClosedXML.Excel;
using ExpandedXml.Readers.CellReaders;

namespace ExpandedXml.Readers
{
    /// <summary>
    /// 시트의 행을 읽는다
    /// </summary>
    public class RowReader : IRowReader
    {
        private readonly Dictionary<Type, ICellReader> _cellReaders = new()
        {
            { typeof(bool), new BoolCellReader() },

            { typeof(double), new DoubleCellReader() },
            { typeof(ushort), new DoubleCellReader() },
            { typeof(short), new DoubleCellReader() },
            { typeof(uint), new DoubleCellReader() },
            { typeof(int), new DoubleCellReader() },
            { typeof(ulong), new DoubleCellReader() },
            { typeof(long), new DoubleCellReader() },
            { typeof(float), new DoubleCellReader() },
            { typeof(decimal), new DoubleCellReader() },
            { typeof(byte), new DoubleCellReader() },

            { typeof(string), new StringCellReader() },

            { typeof(TimeSpan), new TimeSpanCellReader() },

            { typeof(DateTime), new DateTimeCellReader() },
        };

        /// <summary>
        /// 주어진 타입의 셀에 텍스트 변환기를 추가한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cellTextConverter"></param>
        public void AddCellTextConverter(Type type, ICellTextConverter cellTextConverter)
        {
            if (_cellReaders.TryGetValue(type, out ICellReader? cellReader))
                cellReader.CellTextConverter = cellTextConverter;
        }

        /// <summary>
        /// 셀 읽기 객체를 추가한다
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cellReader"></param>
        public void AddCellReader(Type type, ICellReader cellReader)
        {
            _cellReaders[type] = cellReader;
        }

        public ExRow Read(int exRowNumber, IXLRow xlRow, IEnumerable<ExColumn> exColumns)
        {
            var exRow = new ExRow(exRowNumber);
            foreach (var exColumn in exColumns)
            {
                var cell = xlRow.Cell(exColumn.Number);

                if (_cellReaders.TryGetValue(exColumn.Type, out var cellReader) && cellReader.TryRead(cell,  out object? value))
                {
                    exRow.AddCell(exColumn, Convert.ChangeType(value, exColumn.Type));
                }
                else
                {
                    exRow.AddCell(exColumn, null);
                }
            }
            return exRow;
        }

    }
}
