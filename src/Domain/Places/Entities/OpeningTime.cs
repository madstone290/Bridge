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
        private OpeningTime() { }
        private OpeningTime(DayOfWeek day)
        {
            Day = day;
        }

        /// <summary>
        /// 주어진 요일의 영업시간을 생성한다.
        /// </summary>
        /// <param name="day">영업요일</param>
        /// <returns></returns>
        public static OpeningTime Create(DayOfWeek day)
        {
            return new OpeningTime(day);
        }

        /// <summary>
        /// 24시간 영업
        /// </summary>
        /// <returns></returns>
        public static OpeningTime CreateTwentyFourHours(DayOfWeek day)
        {
            return new OpeningTime(day)
            {
                Is24Hours = true
            };
        }

        public static OpeningTime CreateDayoff(DayOfWeek day)
        {
            return new OpeningTime(day)
            {
                IsDayoff = true
            };
        }

        /// <summary>
        /// 영업요일
        /// </summary>
        public DayOfWeek Day { get; private set; }

        /// <summary>
        /// 휴무일
        /// </summary>
        public bool IsDayoff { get; private set;  }

        /// <summary>
        /// 24시간 영업 여부
        /// </summary>
        public bool Is24Hours { get; private set; }

        /// <summary>
        /// 개점 시간
        /// </summary>
        public TimeSpan? OpenTime { get; private set; }

        /// <summary>
        /// 폐점 시간
        /// </summary>
        public TimeSpan? CloseTime { get; private set; }

        /// <summary>
        /// 휴식 시작시간
        /// </summary>
        public TimeSpan? BreakStartTime { get; private set; }

        /// <summary>
        /// 휴식 종료시간
        /// </summary>
        public TimeSpan? BreakEndTime { get; private set; }


        /// <summary>
        /// 영업시간을 설정한다.
        /// </summary>
        /// <param name="openTime"></param>
        /// <param name="closeTime"></param>
        internal void SetOpenCloseTime(TimeSpan openTime, TimeSpan closeTime)
        {
            OpenTime = openTime;
            CloseTime = closeTime;

            IsDayoff = false;
            Is24Hours = false;
        }


        /// <summary>
        /// 휴식시간을 설정한다.
        /// </summary>
        /// <param name="breakStartTime"></param>
        /// <param name="breakEndTime"></param>
        internal void SetBreakTime(TimeSpan breakStartTime, TimeSpan breakEndTime)
        {
            BreakStartTime = breakStartTime;
            BreakEndTime = breakEndTime;

            IsDayoff = false;
        }

        /// <summary>
        /// 휴식시간을 제거한다.
        /// </summary>
        internal void ClearBreakTime()
        {
            BreakStartTime = null;
            BreakEndTime = null;
        }

        /// <summary>
        /// 휴무일 설정
        /// </summary>
        internal void SetDayoff()
        {
            IsDayoff = true;
            Is24Hours = false;
            OpenTime = null;
            CloseTime = null;
            BreakStartTime = null;
            BreakEndTime = null;
        }

        /// <summary>
        /// 휴무일 제거
        /// </summary>
        internal void ClearDayoff()
        {
            IsDayoff = false;
        }

        /// <summary>
        /// 24시간 영업설정
        /// </summary>
        internal void SetTwentyFourHours()
        {
            Is24Hours = true;
            IsDayoff = false;
            OpenTime = null;
            CloseTime = null;
        }

        /// <summary>
        /// 24시간 영업 취소
        /// </summary>
        internal void ClearTwentyFourHours()
        {
            Is24Hours = false;
        }
    }
}
