using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages
{
    /// <summary>
    /// 콜백 이벤트를 수신한다
    /// </summary>
    public interface IEventReceiver
    {
        IHandleEvent Receiver { get; set; }
    }
}
