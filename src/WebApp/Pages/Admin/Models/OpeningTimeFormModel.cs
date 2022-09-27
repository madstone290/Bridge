﻿using FluentValidation;
using System.ComponentModel;

namespace Bridge.WebApp.Pages.Admin.Models
{
    public class OpeningTimeFormModel : INotifyPropertyChanged
    {
        private bool _dayoff;
        private bool _twentyFourHours;

        public OpeningTimeFormModel() { }
        public OpeningTimeFormModel(DayOfWeek day)
        {
            Day = day;
        }

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
        public bool Dayoff 
        {
            get => _dayoff;
            set
            {
                if (_dayoff == value)
                    return;
                _dayoff = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Dayoff)));

                if (value)
                {
                    OpenTime = null;
                    CloseTime = null;

                    TwentyFourHours = false;
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 24시간 영업
        /// </summary>
        public bool TwentyFourHours
        {
            get => _twentyFourHours;
            set
            {
                if (_twentyFourHours == value)
                    return;
                _twentyFourHours = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TwentyFourHours)));

                if (value)
                {
                    OpenTime = new TimeSpan(0, 0, 0);
                    CloseTime = new TimeSpan(23, 59, 0);

                    Dayoff = false;
                }
                
            }
        }

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