using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public interface IPrintableEventConvertible
    {
        PrintableEvent AsPrintableEvent();
    }
    public class PrintableEvent : Event
    {
        public string Message;
    }

    public interface INotifyChangeConvertible
    {
        NotifyChangeEvent AsNotifyChangeEvent();
    }
    public class NotifyChangeEvent : Event
    {
        public string User;
        public string Target;
        public string Description;
        public DateTime Time;
    }
}
