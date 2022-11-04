namespace Bridge.Application.Places.Dtos
{
    public record OpeningTimeDto
    {
        /// <summary>
        /// 영업요일
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// 휴무일
        /// </summary>
        public bool IsDayoff { get; set; }

        /// <summary>
        /// 24시간 영업
        /// </summary>
        public bool Is24Hours { get; set; }

        /// <summary>
        /// 개점 시간
        /// </summary>
        public TimeSpan? OpenTime { get; set; }

        /// <summary>
        /// 폐점 시간
        /// </summary>
        public TimeSpan? CloseTime { get; set; }

        /// <summary>
        /// 휴식 시작시간
        /// </summary>
        public TimeSpan? BreakStartTime { get; set; }

        /// <summary>
        /// 휴식 종료시간
        /// </summary>
        public TimeSpan? BreakEndTime { get; set; }
    }
}
