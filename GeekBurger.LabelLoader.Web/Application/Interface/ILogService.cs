using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.LabelLoader.Web.Application.Interface
{
    public interface ILogService
    {
        void SendMessagesAsync(string message);
    }
}
