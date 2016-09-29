using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Models
{
    public class UserNotification
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; }

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; }

        public ApplicationUser User { get; private set; }    //this and the bottom notification are navigation properties for navigation reasons

        public Notification Notification { get; private set; } // we set the set to private since we always want the usernotification in valid state

        public bool IsRead { get; private set; }

        protected UserNotification() // we changed it to protected since we don't want to use it anywhere except for entity framework at runtime
        {

        }

        public UserNotification(ApplicationUser user, Notification notification)
        {
            if (user == null)
                throw new ArgumentNullException("user");



            if (notification == null)
                throw new ArgumentNullException("notification");

            User = user;
            Notification = notification;
        }
        public void Read()
        {
            IsRead = true;
        }

    }
}