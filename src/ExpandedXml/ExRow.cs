namespace ExpandedXml
{
    /// <summary>
    /// 시트 행. 셀 목록을 가진다.
    /// </summary>
    public class ExRow
    {
        private readonly List<ExCell> _cells = new();

        public ExRow(int number)
        {
            Number = number;
        }

        /// <summary>
        /// 행 번호
        /// </summary>
        public int Number { get; }

        public IEnumerable<ExCell> Cells => _cells;

        public void AddCell(ExColumn column, object? value)
        {
            var cell = new ExCell(this, column, value);
            _cells.Add(cell);
        }

        public override string ToString()
        {
            return $"{typeof(ExRow).Name}, {{Number: {Number}, Cells Count: {Cells.Count()}}}";
        }
    }
}
