using AutoMapper;
using StudentApp.API.Dtos;
using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                  src.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                  src.Photos.FirstOrDefault(p => p.IsMain).Url));
            CreateMap<Photo, PhotoForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<UserForRegistrationDto, User>();

            CreateMap<AttendanceCreationDto, Attendance>().ReverseMap();

            CreateMap<MessageCreationDto, Message>().ReverseMap();

            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m=>m.SenderPhotoUrl, loc=> loc.MapFrom(u=>u.Sender.Photos.FirstOrDefault(p=>p.IsMain).Url))
                .ForMember(m=>m.ReceiverPhotoUrl, loc=> loc.MapFrom(u=>u.Receiver.Photos.FirstOrDefault(p=>p.IsMain).Url));

            CreateMap<CourseToCreateDto, Course>().ReverseMap();
            CreateMap<Course, CourseToReturnDto>().ReverseMap();
        }
    }
}
