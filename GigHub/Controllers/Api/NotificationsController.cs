using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications.Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist)
                .ToList();

            //CODE BELOW IS IN MappingProfile IN App_Start THAT IS WHERE WE PUT OUR MAPPING THINGS.
            //var c1 = new MapperConfiguration(a => a.CreateMap<ApplicationUser, UserDto>());
            //var c2 = new MapperConfiguration(a => a.CreateMap<Gig, GigDto>());
            //var c3 = new MapperConfiguration(a => a.CreateMap<Notification, NotificationDto>());
            //used the one above than the below because it has changed since the course I took.
            //Mapper.CreateMap<ApplicationUser, UserDto>();
            //Mapper.CreateMap<Gig, GigDto>();
            //Mapper.CreateMap<Notification, NotificationDto>();
            //the upper one is the one taught to us. the below one is a longer process.

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);

            //return notifications.Select(n => new NotificationDto()
            //{
            //    Datetime = n.Datetime,
            //    Gig = new GigDto
            //    {
            //        Artist = new UserDto
            //        {
            //            Id = n.Gig.Artist.Id,
            //            Name = n.Gig.Artist.Name
            //        },
            //        DateTime = n.Gig.DateTime,
            //        ID = n.Gig.ID,
            //        IsCanceled = n.Gig.IsCanceled,
            //        Venue = n.Gig.Venue
            //    },
            //    OriginalDateTime = n.OriginalDateTime,
            //    OriginalVenue = n.OriginalVenue,
            //    Type = n.Type


            //}

            //);
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToList();

            notifications.ForEach(n => n.Read());
            _context.SaveChanges();
            return Ok();

        }


    }
}
