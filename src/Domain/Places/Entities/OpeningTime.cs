using Bridge.Domain.Common;
using Bridge.Domain.Places.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Places.Entities
{
    /// <summary>
    /// 영업시간
    /// </summary>
    public class OpeningTime : Entity
    {
        /// <summary>
        /// 24시간 영업의 시작시간
        /// </summary>
        private static readonly TimeSpan StartTimeOf24Hours = TimeSpan.Zero;

        /// <summary>
        /// 24시간 영업의 종료시간
        /// </summary>
        private static readonly TimeSpan CloseTimeOf24Hours = TimeSpan.FromHours(24);

        private OpeningTime() { }
        private OpeningTime(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
        {
            ValidateTime(openTime, closeTime);

            Day = day;
            OpenTime = openTime;
            CloseTime = closeTime;
        }

        /// <summary>
        /// 시작/종료 시간으로 영업시간 생성
        /// </summary>
        /// <param name="openTime"></param>
        /// <param name="closeTime"></param>
        /// <returns></returns>
        public static OpeningTime Between(DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
        {
            return new OpeningTime(day, openTime, closeTime);
        }

        /// <summary>
        /// 24시간 영업
        /// </summary>
        /// <returns></returns>
        public static OpeningTime TwentyFourHours(DayOfWeek day)
        {
            return new OpeningTime(day, StartTimeOf24Hours, CloseTimeOf24Hours);
        }

        /// <summary>
        /// 주어진 시간이 유효한지 검사한다
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="InvalidTimeException"></exception>
        private static void ValidateTime(TimeSpan start, TimeSpan end)
        {
            if (start < StartTimeOf24Hours || CloseTimeOf24Hours < start)
                throw new InvalidTimeException();

            if (end < StartTimeOf24Hours || CloseTimeOf24Hours < end)
                throw new InvalidTimeException();

            if (end < start)
                throw new InvalidTimeException();
        }

        /// <summary>
        /// 영업요일
        /// </summary>
        public DayOfWeek Day { get; private set; }

        /// <summary>
        /// 개점 시간
        /// </summary>
        public TimeSpan OpenTime { get; private set; }

        /// <summary>
        /// 폐점 시간
        /// </summary>
        public TimeSpan CloseTime { get; private set; }

        /// <summary>
        /// 휴식 시작시간
        /// </summary>
        public TimeSpan? BreakStartTime { get; private set; }

        /// <summary>
        /// 휴식 종료시간
        /// </summary>
        public TimeSpan? BreakEndTime { get; private set; }

        /// <summary>
        /// 24시간 영업 여부
        /// </summary>
        public bool Is24Hours => OpenTime == TimeSpan.Zero && CloseTime == TimeSpan.FromDays(1);

        /// <summary>
        /// 휴식시간을 설정한다.
        /// </summary>
        /// <param name="breakStartTime"></param>
        /// <param name="breakEndTime"></param>
        internal void SetBreakTime(TimeSpan breakStartTime, TimeSpan breakEndTime)
        {
            ValidateTime(breakStartTime, breakEndTime);

            BreakStartTime = breakStartTime;
            BreakEndTime = breakEndTime;
        }

        /// <summary>
        /// 휴식시간을 제거한다.
        /// </summary>
        internal void ClearBreakTime()
        {
            BreakStartTime = null;
            BreakEndTime = null;
        }

    }
}
