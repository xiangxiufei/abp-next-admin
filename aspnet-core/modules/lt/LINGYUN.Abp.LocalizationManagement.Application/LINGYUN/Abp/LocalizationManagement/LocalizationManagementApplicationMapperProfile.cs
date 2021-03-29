﻿using AutoMapper;

namespace LINGYUN.Abp.LocalizationManagement
{
    public class LocalizationManagementApplicationMapperProfile : Profile
    {
        public LocalizationManagementApplicationMapperProfile()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<Resource, ResourceDto>();
            CreateMap<Text, TextDto>();
            CreateMap<TextDifference, TextDifferenceDto>();
        }
    }
}
