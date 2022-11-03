using Bridge.Domain.Places.Enums;
using Bridge.WebApp.Pages.Admin.Models;
using MudBlazor;
using System.Linq.Expressions;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IRestroomFormViewModel
    {
        MudDialogInstance MudDialog { get; set; }

        long RestroomId { get; set; }

        RestroomModel Restroom { get; }

        FormMode FormMode { get; set; }

        bool IsRestroomValid { get; set; }

        Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(Expression<Func<PlaceModel, TProperty>> expression);

        string GetDiaperTableLocationText(DiaperTableLocation? location);

        Task Initialize();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}
