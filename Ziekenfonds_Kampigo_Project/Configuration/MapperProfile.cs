using AutoMapper;
using Models;
using Ziekenfonds_Kampigo_Project.ViewModels.Activiteit;
using Ziekenfonds_Kampigo_Project.ViewModels.Bestemming;
using Ziekenfonds_Kampigo_Project.ViewModels.Groepsreis;

namespace Ziekenfonds_Kampigo_Project.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Groepsreis, GroepsreisBestemmingActiviteitenViewModel>()
               .ForMember(dest => dest.Bestemming, opt => opt.MapFrom(src => src.Bestemming))
               .ForMember(dest => dest.Activiteiten, opt =>
                   opt.MapFrom(src => src.Programmas.Select(p => p.Activiteit).ToList()));
            CreateMap<Activiteit, ActiviteitViewModel>();
            CreateMap<Bestemming, BestemmingViewModel>();
            CreateMap<Groepsreis, GroepsreisDetailViewModel>()
          .ForMember(dest => dest.Naam, opt => opt.MapFrom(src => src.Bestemming.Naam));

        }
    }
}
