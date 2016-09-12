using GigHub.Controllers.Api;
using GigHub.Models;
using System;

namespace GigHub.Dtos
{
    public class NotificationDto
    {
        public DateTime Datetime { get; set; }
        public NotificationType Type { get; set; }
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }
        public GigDto Gig { get; set; }
    }
}