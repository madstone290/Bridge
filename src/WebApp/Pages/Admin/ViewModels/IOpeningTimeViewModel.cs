using Bridge.WebApp.Pages.Admin.Models;

namespace Bridge.WebApp.Pages.Admin.ViewModels
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
