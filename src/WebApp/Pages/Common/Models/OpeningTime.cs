using Bridge.Application.Places.Dtos;

namespace Bridge.WebApp.Pages.Common.Models
{
    public class OpeningTime
    {
        public OpeningTime() { }
        public OpeningTime(DayOfWeek day) { Day = day; }

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

        public static OpeningTime Create(OpeningTimeDto t)
        {
            return new OpeningTime()
            {
                Day = t.Day,
                IsDayoff = t.IsDayoff,
                Is24Hours = t.Is24Hours,
                OpenTime = t.OpenTime,
                CloseTime = t.CloseTime,
                BreakStartTime = t.BreakStartTime,
                BreakEndTime = t.BreakEndTime
            };
        }

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

        /// <summary>
        /// 영업요일
        /// </summary>
        public string DayString => _dayStrings[Day];

        public string OpenTimeString => OpenTime.HasValue
            ? $"{OpenTime:hh\\:mm}"
            : "??";

        public string CloseTimeString => CloseTime.HasValue
            ? $"{CloseTime:hh\\:mm}"
            : "??";

        public string BreakStartTimeString => BreakStartTime.HasValue
           ? $"{BreakStartTime:hh\\:mm}"
           : string.Empty;

        public string BreakEndTimeString => BreakEndTime.HasValue
            ? $"{BreakEndTime:hh\\:mm}"
            : string.Empty;

        private string? GetBreakTimeString()
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
            if (IsDayoff)
                return $"{DayString} 휴무일";
            if (Is24Hours)
                return $"{DayString} 24시간 영업" + GetBreakTimeString();

            return $"{DayString} {OpenTimeString}~{CloseTimeString}" + GetBreakTimeString();
        }

        public void SyncIs24Hours()
        {
            if (Is24Hours)
            {
                OpenTime = null;
                CloseTime = null;
                IsDayoff = false;
            }
        }

        public void SyncIsDayoff()
        {
            if (IsDayoff)
            {
                OpenTime = null;
                CloseTime = null;
                Is24Hours = false;
            }
        }


    }
}
