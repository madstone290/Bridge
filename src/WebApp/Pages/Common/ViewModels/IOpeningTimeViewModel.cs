using Bridge.WebApp.Pages.Common.Models;

namespace Bridge.WebApp.Pages.Common.ViewModels
{
    public interface IOpeningTimeViewModel
    {
        OpeningTime OpeningTime { get; set; }
        bool IsReadOnly { get; set; }
        string OpenDay { get; }
        TimeSpan? OpenTime { get; set; }
        TimeSpan? CloseTime { get; set; }
        TimeSpan? BreakStartTime { get; set; }
        TimeSpan? BreakEndTime { get; set; }
        bool IsDayoff { get; set; }
        bool Is24Hours { get; set; }
    }
}
