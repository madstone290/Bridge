using ClosedXML.Excel;
using System.Reflection;

namespace ExpandedXml.Writers
{
    public class ExWriter : IExWriter
    {
        public void Write<T>(string filePath, IEnumerable<T> list, IEnumerable<ExColumnHeader> exHeaders)
        {
            var data = Write<T>(list, exHeaders);
            File.WriteAllBytes(filePath, data);
        }

        public byte[] Write<T>(IEnumerable<T> list, IEnumerable<ExColumnHeader> exHeaders)
        {
            var wbook = new XLWorkbook();
            var worksheet = wbook.AddWorksheet();

            var properties = typeof(T).GetProperties().Where(x => x.CanWrite);
            var columnNumberProperties = new Dictionary<ExColumnHeader, PropertyInfo>();

            var rowNumber = 1;
            var columnNumber = 1;
            // write header row
            foreach (var header in exHeaders)
            {
                worksheet.Cell(rowNumber, columnNumber).Value = header.Caption;

                var property = properties.FirstOrDefault(x => string.Equals(x.Name, header.Name, StringComparison.OrdinalIgnoreCase));
                if (property != null)
                    columnNumberProperties.Add(header, property);

                columnNumber++;
            }
            rowNumber++;

            // write data rows
            foreach (var instance in list)
            {
                columnNumber = 1;
                foreach (var header in exHeaders)
                {
                    if (columnNumberProperties.ContainsKey(header))
                    {
                        var property = columnNumberProperties[header];
                        var cell = worksheet.Cell(rowNumber, columnNumber);
                        // TODO Use CellWriters
                        var value = property.GetValue(instance);

                        cell.SetValue(value);
                    }
                    columnNumber++;
                }
                rowNumber++;
            }
            var stream = new MemoryStream();
            wbook.SaveAs(stream);
            return stream.ToArray();
        }

    }
}
