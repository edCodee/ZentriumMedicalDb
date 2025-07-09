using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace HospitalAPI.DTOs.ConversationHistoryDTO
{
    public class ConversationHistoryReadDTO
    {
        public int MessId { get; set; }
        public string? MessUserNumber { get; set; }=string.Empty;
        public string? MessageTex { get; set; }=string.Empty;
        public string? MessBootResponse { get; set; } = string.Empty;
        public string? MessConversationState { get; set; } = string.Empty;
        public DateTime? MessCreatedAt { get; set; }
        public string?  MessImage { get; set; }
    }
}
