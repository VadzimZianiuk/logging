using System.Collections.Generic;

namespace LogSender.Models
{
    public class EmailSenders
    {
        public IList<Sender> Senders { get; set; } = new List<Sender>();
    }
}
