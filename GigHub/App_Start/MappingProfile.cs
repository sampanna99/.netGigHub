using AutoMapper;
using GigHub.Controllers.Api;
using GigHub.Dtos;
using GigHub.Models;

namespace GigHub.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var c1 = new MapperConfiguration(a => a.CreateMap<ApplicationUser, UserDto>());
            var c2 = new MapperConfiguration(a => a.CreateMap<Gig, GigDto>());
            var c3 = new MapperConfiguration(a => a.CreateMap<Notification, NotificationDto>());
        }


    }
}