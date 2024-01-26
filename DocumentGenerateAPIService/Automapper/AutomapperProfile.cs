using DocumentGenerateAPIService.DBModels;
using AutoMapper;
using DocumentGenerateAPIService.Models;

namespace DocumentGenerateAPIService.Automapper;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<FileModel, Models.FileInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        CreateMap<FileModel, ConvertedFile>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.PdfContent));
    }
}