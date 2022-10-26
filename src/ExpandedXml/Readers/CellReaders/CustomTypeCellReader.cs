using ClosedXML.Excel;

namespace ExpandedXml.Readers.CellReaders
{
    /// <summary>
    /// 사용자 정의 타입을 사용하는 셀 값을 읽는다
    /// </summary>
    public class CustomTypeCellReader<TValueType> : CellReader<TValueType>
    {
        private readonly Func<string, TValueType> _func;

        public CustomTypeCellReader(Func<string, TValueType> func)
        {
            _func = func;
        }

        public override bool TryRead(IXLCell cell, out TValueType value)
        {
            cell.DataType = XLDataType.Text;
            var text = cell.GetString();

            value = _func.Invoke(text);
            return true;
        }
    }
}
