using AutoMapper;
using WebApi.Business.Models;
using WebApi.Data.Models;

namespace WebApi.Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Port, PortDto>().ReverseMap();
            CreateMap<PortDto, PortVM>().ReverseMap();
            CreateMap<Terminal, TerminalDto>().ReverseMap();
            CreateMap<TerminalDto, TerminalVM>().ReverseMap();
        }
    }
}
