using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    public class DateTimeCellReader : CellReader<DateTime>
    {
        public ICellTextConverter<DateTime>? CustomReader { get; set; }

        public override bool TryRead(IXLCell cell, out DateTime value)
        {
            if (cell.DataType == XLDataType.DateTime)
                return cell.TryGetValue(out value);

            cell.DataType = XLDataType.Text;
            var text = cell.GetValue<string>();

            if (CustomReader != null && CustomReader.TryConvert(text, out value))
                return true;

            return cell.TryGetValue(out value);
        }
    }
}
