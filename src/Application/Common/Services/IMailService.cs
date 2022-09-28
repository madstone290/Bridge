using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Application.Common.Services
{
    public interface IMailService
    {
        /// <summary>
        /// 이메일을 전송한다.
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task SendAsync(string emailTo, string subject, string body);
    }
}
