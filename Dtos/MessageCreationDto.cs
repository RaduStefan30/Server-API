using System;

namespace StudentApp.API.Dtos
{
    public class MessageCreationDto
    {
         public int SenderId { get; set; }
        public int ReceiverId {get; set;}
        public DateTime MessageSent { get; set; }
        public string Text {get; set; }

        public MessageCreationDto()
        {
            MessageSent=DateTime.Now;
        }
    }
}