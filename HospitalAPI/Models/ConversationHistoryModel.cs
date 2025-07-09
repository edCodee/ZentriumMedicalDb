using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Models
{
    public class ConversationHistoryModel
    {
        [Key]
        public int Id { get; set; }
        public string? UserNumber { get; set; }
        public string? MessageText { get; set; }
        public string? BotResponse { get; set; }
        public string? ConversationState { get; set; } 
        public DateTime? CreatedAt { get; set; }
        public string? ImageBase64 { get; set; } 
        public ConversationHistoryModel() { }

        public ConversationHistoryModel(int id, string? userNumber, string? messageText, string? botResponse, string? conversationState, DateTime? createdAt, string? imageBase64)
        {
            Id = id;
            UserNumber = userNumber;
            MessageText = messageText;
            BotResponse = botResponse;
            ConversationState = conversationState;
            CreatedAt = createdAt;
            ImageBase64 = imageBase64;
        }
    }
}
