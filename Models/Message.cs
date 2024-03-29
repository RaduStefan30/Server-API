using System;

namespace StudentApp.API.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; } 

        public User Sender { get; set; }

        public int ReceiverId { get; set; }

        public User Receiver { get; set; }

        public bool HasRead { get; set; }

        public string Text {get; set; }
        
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool ReceiverDeleted { get; set; }
    }
}