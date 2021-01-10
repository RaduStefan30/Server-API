using System;

namespace StudentApp.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; } 

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }
        
        public string SenderPhotoUrl { get; set; }

        public int ReceiverId { get; set; }

        public string ReceiverFirstName { get; set; }

        public string ReceiverLastName { get; set; }
        
        public string ReceiverPhotoUrl { get; set; }

        public bool HasRead { get; set; }

        public string Text {get; set; }
        public DateTime MessageSent { get; set; }

        public DateTime? DateRead { get; set; }
    }
}