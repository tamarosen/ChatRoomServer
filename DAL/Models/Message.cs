using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
    }
}
