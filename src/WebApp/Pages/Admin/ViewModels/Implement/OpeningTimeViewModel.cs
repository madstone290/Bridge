using Bridge.WebApp.Pages.Admin.Models;

namespace Bridge.WebApp.Pages.Admin.ViewModels.Implement
{
    public class OpeningTimeViewModel : IOpeningTimeViewModel
    {
        public OpeningTimeModel OpeningTime { get; set; } = null!;

        public string OpenDay => OpeningTime.DayString;

        public TimeSpan? OpenTime
        {
            get => OpeningTime.OpenTime;
            set => OpeningTime.OpenTime = value;
        }

        public TimeSpan? CloseTime
        {
            get => OpeningTime.CloseTime;
            set => OpeningTime.CloseTime = value;
        }

        public TimeSpan? BreakStartTime
        {
            get => OpeningTime.BreakStartTime;
            set => OpeningTime.BreakStartTime = value;
        }

        public TimeSpan? BreakEndTime
        {
            get => OpeningTime.BreakEndTime;
            set => OpeningTime.BreakEndTime = value;
        }

        public bool IsDayoff
        {
            get => OpeningTime.IsDayoff;
            set
            {
                OpeningTime.IsDayoff = value;
                OpeningTime.SyncIsDayoff();
            }
        }

        public bool Is24Hours
        {
            get => OpeningTime.Is24Hours;
            set
            {
                OpeningTime.Is24Hours = value;
                OpeningTime.SyncIs24Hours();
            }
        }

        public bool IsReadOnly { get; set; }
    }
}
