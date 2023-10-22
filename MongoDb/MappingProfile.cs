using AutoMapper;
using MongoDB.Bson;
using SocialNetwork.Models;

namespace SocialNetwork
{
    internal class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            CreateMap<BsonDocument, Post>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src["user"].AsString))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["title"].AsString))
                //.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src["date"].AsBsonDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src["content"].AsString));


            CreateMap<BsonDocument, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src["_id"].AsObjectId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src["FirstName"].AsString))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src["LastName"].AsString))
                .ForMember(dest => dest.Friends, opt => opt.MapFrom(src => src["friends"].AsBsonArray.Select(f => f.AsString).ToList()));
        }
    }
}
