namespace ExpandedXml
{
    /// <summary>
    /// 시트 블록. 행 목록 및 열 목록을 가진다.
    /// </summary>
    public class ExBlock
    {
        public ExBlock(IEnumerable<ExColumn> columns, IEnumerable<ExRow> rows)
        {
            Columns = columns;
            Rows = rows;
        }

        public IEnumerable<ExColumn> Columns { get; }

        public IEnumerable<ExRow> Rows { get; }
    }
}
