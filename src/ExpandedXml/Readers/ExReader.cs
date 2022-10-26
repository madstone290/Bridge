using ClosedXML.Excel;
using ExpandedXml.Readers.CellReaders;

namespace ExpandedXml.Readers
{
    /// <summary>
    /// 엑셀시트를  읽는다
    /// </summary>
    public class ExReader : IExReader
    {
        public BlockReader BlockReader { get; } = new();

        public void AddCellReader(Type type, ICellReader cellReader)
        {
            BlockReader.AddCellReader(type, cellReader);
        }

        public void AddCellTextConverter(Type type, ICellTextConverter cellTextConverter)
        {
            BlockReader.AddCellTextConverter(type, cellTextConverter);
        }

        public IEnumerable<T> Read<T>(Stream stream, IEnumerable<ExColumnHeader> exHeaders, int sheetNumber = 1)
        {
            var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(sheetNumber);

            return Read<T>(worksheet, exHeaders);
        }

        public IEnumerable<T> Read<T>(byte[] data, IEnumerable<ExColumnHeader> exHeaders, int sheetNumber = 1)
        {
            using var stream = new MemoryStream(data);

            return Read<T>(stream, exHeaders, sheetNumber);
        }

        public IEnumerable<T> Read<T>(IXLWorksheet worksheet, IEnumerable<ExColumnHeader> exHeaders)
        {
            var xlRows = worksheet.RowsUsed();
            var xlColumns = worksheet.ColumnsUsed();

            return Read<T>(xlRows, xlColumns, exHeaders);
        }

        public IEnumerable<T> Read<T>(IEnumerable<IXLRow> xlRows, IEnumerable<IXLColumn> xlColumns, IEnumerable<ExColumnHeader> exHeaders)
        {

            var exColumns = new List<ExColumn>();
            var headerRow = xlRows.First();
            foreach (var xlColumn in xlColumns)
            {
                var caption = headerRow.Cell(xlColumn.ColumnNumber()).GetString();
                var exColumnHeader = exHeaders.FirstOrDefault(x => x.Caption == caption);
                if (exColumnHeader != null)
                {
                    var exColumn = new ExColumn(xlColumn.ColumnNumber(), exColumnHeader.Type);
                    exColumn.SetHeader(exColumnHeader);
                    exColumns.Add(exColumn);
                }
            }
            var exBlock = BlockReader.Read(xlRows.Skip(1), exColumns);

            var properties = typeof(T).GetProperties().ToDictionary(x => x.Name, x => x);
            var underlyingTypes = new Dictionary<string, Type>();

            var list = new List<T>();
            foreach(var row in exBlock.Rows)
            {
                var instance = Activator.CreateInstance<T>();

                foreach(var cell in row.Cells)
                {
                    var columnHeader = cell.Column.Header;
                    if (columnHeader == null)
                        continue;

                    if (!properties.TryGetValue(columnHeader.Name, out var property))
                        continue;

                    if(!underlyingTypes.TryGetValue(property.Name, out var underlyingType))
                    {
                        underlyingType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        underlyingTypes[property.Name] = underlyingType;
                    }

                    if (cell.Value != null)
                        property.SetValue(instance, Convert.ChangeType(cell.Value, underlyingType));
                }
                list.Add(instance);
            }
            return list;
        }

       
    }
}
