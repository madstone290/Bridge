using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    public class BoolCellReader : CellReader<bool>
    {
        public ICellTextConverter<bool>? CustomBoolReader => (ICellTextConverter<bool>?) CellTextConverter;

        public override bool TryRead(IXLCell cell, out bool value)
        {
            if (cell.DataType == XLDataType.Boolean)
                return cell.TryGetValue(out value);

            cell.DataType = XLDataType.Text;
            var text = cell.GetValue<string>();

            if (CustomBoolReader != null && CustomBoolReader.TryConvert(text, out value))
                return true;

            return cell.TryGetValue(out value);
        }
     
    }
}
