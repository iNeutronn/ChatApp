using AutoMapper;
using ChatAppBAL.Models;
using ChatAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.JoinedChatsIds, opt => opt.MapFrom(src => src.JoinedChats.Select(chat => chat.Id)))
                .ReverseMap();
        }
    }
}
