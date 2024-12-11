using Ecology.Data.Interface.Models;

namespace Ecology.Data.Models
{
    public class ChatMessageData : BaseModel, IChatMessageData
    {
        public DateTime CreationTime { get; set; }
        public string Message { get; set; } 
        public virtual UserData? User { get; set; }
    }
}
