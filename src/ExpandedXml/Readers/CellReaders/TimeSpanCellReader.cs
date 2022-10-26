using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    public class TimeSpanCellReader : CellReader<TimeSpan>
    {
        public ICellTextConverter<TimeSpan>? CustomReader { get; set; }

        public override bool TryRead(IXLCell cell, out TimeSpan value)
        {
            if (cell.DataType == XLDataType.TimeSpan)
                return cell.TryGetValue(out value);

            if (cell.DataType == XLDataType.DateTime)
            {
                value = cell.GetValue<DateTime>().TimeOfDay;
                return true;
            }

            cell.DataType = XLDataType.Text;
            var text = cell.GetValue<string>();

            if (CustomReader != null && CustomReader.TryConvert(text, out value))
                return true;

            return cell.TryGetValue(out value);
        }
    }
}
