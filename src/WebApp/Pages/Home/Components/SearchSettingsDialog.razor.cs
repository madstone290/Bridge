using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.Components
{
    public partial class SearchSettingsDialog
    {
        [CascadingParameter]
        private MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter]
        public double Easting { get; set; }

        [Parameter]
        public double Northing { get; set; }

        [Parameter]
        public double Distance { get; set; }

        private void Cancel_Click()
        {
            MudDialog.Cancel();
        }

        private void Save_Click()
        {
            MudDialog.Close(new 
            {
                Easting,
                Northing,
                Distance
            });
        }
    }
}
