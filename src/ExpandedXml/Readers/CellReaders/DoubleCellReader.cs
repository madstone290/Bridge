using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    public class DoubleCellReader : CellReader<double>
    {
        public ICellTextConverter<double>? CustomReader { get; set; }

        public override bool TryRead(IXLCell cell, out double value)
        {
            if (cell.DataType == XLDataType.Number)
                return cell.TryGetValue(out value);

            cell.DataType = XLDataType.Text;
            var text = cell.GetValue<string>();

            if (CustomReader != null && CustomReader.TryConvert(text, out value))
                return true;

            return cell.TryGetValue(out value);
        }

    }
}
