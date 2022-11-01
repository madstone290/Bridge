namespace Bridge.WebApp.Pages.Home.Models
{
    public class OpeningTime
    {

        private static readonly Dictionary<DayOfWeek, string> _dayStrings = new()
        {
            { DayOfWeek.Monday, "월요일" },
            { DayOfWeek.Tuesday, "화요일" },
            { DayOfWeek.Wednesday, "수요일" },
            { DayOfWeek.Thursday, "목요일" },
            { DayOfWeek.Friday, "금요일" },
            { DayOfWeek.Saturday, "토요일" },
            { DayOfWeek.Sunday, "일요일" },
        };

        /// <summary>
        /// 영업요일
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// 영업요일
        /// </summary>
        public string DayString => _dayStrings[Day];

        /// <summary>
        /// 휴무일
        /// </summary>
        public bool Dayoff { get; set; }

        /// <summary>
        /// 24시간 영업
        /// </summary>
        public bool TwentyFourHours { get; set; }

        /// <summary>
        /// 개점 시간
        /// </summary>
        public TimeSpan? OpenTime { get; set; }

        public string OpenTimeString => OpenTime.HasValue
            ? $"{OpenTime:hh\\:mm}"
            : "??";

        /// <summary>
        /// 폐점 시간
        /// </summary>
        public TimeSpan? CloseTime { get; set; }

        public string CloseTimeString => CloseTime.HasValue
            ? $"{CloseTime:hh\\:mm}"
            : "??";

        /// <summary>
        /// 휴식 시작시간
        /// </summary>
        public TimeSpan? BreakStartTime { get; set; }

        public string BreakStartTimeString => BreakStartTime.HasValue
           ? $"{BreakStartTime:hh\\:mm}"
           : string.Empty;

        /// <summary>
        /// 휴식 종료시간
        /// </summary>
        public TimeSpan? BreakEndTime { get; set; }

        public string BreakEndTimeString => BreakEndTime.HasValue
            ? $"{BreakEndTime:hh\\:mm}"
            : string.Empty;

        string? BreakTimeString()
        {
            if (BreakStartTime.HasValue && BreakEndTime.HasValue)
                return $"  휴식 {BreakStartTimeString}~{BreakEndTimeString}";
            return null;
        }

        /// <summary>
        /// 문자열 표현 반환
        /// </summary>
        /// <returns></returns>
        public string ToSingleLineString()
        {
            if (Dayoff)
                return $"{DayString} 휴무일";
            if (TwentyFourHours)
                return $"{DayString} 24시간 영업" + BreakTimeString();

            return $"{DayString} {OpenTimeString}~{CloseTimeString}" + BreakTimeString();
        }
    }
}
