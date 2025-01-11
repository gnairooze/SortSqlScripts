using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptsManagement.Contracts
{
    public class Message
    {
        public string Text { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public MessageTypes Type { get; set; } = MessageTypes.NotSet;

        public enum MessageTypes
        {
            NotSet = 0,
            Debug = 1,
            Info = 2,
            Warning = 4,
            Error = 8
        }
    }
}
