using Bridge.Domain.Places.Enums;
using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Pages.Common.Models;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IRestroomFormViewModel : IFormValidation<Place>, IModal
    {
        Guid RestroomId { get; set; }

        Restroom Restroom { get; }

        bool IsRestroomValid { get; set; }

        string GetDiaperTableLocationText(DiaperTableLocation? location);

        Task Initialize();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}
