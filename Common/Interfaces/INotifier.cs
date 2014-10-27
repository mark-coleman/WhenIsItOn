using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface INotifier
    {
        void SendNotification(string title, string content, IList<string> recipients);
    }
}
