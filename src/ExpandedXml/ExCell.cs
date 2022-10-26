namespace ExpandedXml
{
    /// <summary>
    /// 시트 셀. 행과 열 및 값을 가진다.
    /// </summary>
    public class ExCell
    {
        public ExCell(ExRow row, ExColumn column, object? value)
        {
            Row = row;
            Column = column;
            Value = value;
        }

        public ExRow Row { get; }

        public ExColumn Column { get; }

        public object? Value { get; }

        public override string ToString()
        {
            return $"{{Value: {Value}}}";
        }
    }
}
