namespace PhotoContest.Web.App_Start
{
    using System.Linq;

    using AutoMapper;

    using PhotoContest.Models;
    using PhotoContest.Web.Models.ViewModels;
    using System.Collections.Generic;

    public static class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Image, PagedImageViewModel>().ForMember(i => i.VotesCount,
            map => map.MapFrom(img => img.Votes.Count())).ForMember(i => i.AuthorUsername,
            map => map.MapFrom(img => img.User.UserName));

            Mapper.CreateMap<Image, EditImageViewModel>().ForMember(i => i.Author,
            map => map.MapFrom(img => img.User.UserName)).ForMember(i => i.VotesCount,
            map => map.MapFrom(img => img.Votes.Count())).ForMember(i => i.PostedOn,
            map => map.MapFrom(img => img.CreatedOn)).ForMember(i => i.WinnerPlace,
            map => map.MapFrom(img => img.Prize.ForPlace));

            Mapper.CreateMap<Notification, PagedNotificationViewModel>();
        }
    }
}