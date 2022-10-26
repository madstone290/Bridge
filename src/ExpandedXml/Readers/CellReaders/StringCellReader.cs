using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    public class StringCellReader : CellReader<string>
    {
        public override bool TryRead(IXLCell cell, out string value)
        {
            if (cell.DataType == XLDataType.Text)
            {
                value = cell.GetValue<string>();
                return true;
            }

            cell.DataType = XLDataType.Text;
            return cell.TryGetValue(out value);
        }
    }
}
