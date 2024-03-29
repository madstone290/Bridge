﻿using MudBlazor;

namespace Bridge.WebApp.Pages
{
    /// <summary>
    /// 모달창을 제공하는 뷰모델
    /// </summary>
    public interface IModal
    {
        MudDialogInstance MudDialog { get; set; }
    }
}
