using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TigTag.WebApi
{
    public class Mapper<MODEL,DTO> where MODEL:class where DTO:class
    {
        public static MODEL convertToModel(DTO dto)
        {
            if (dto == null) return null;
            AutoMapper.Mapper.CreateMap<DTO, MODEL>();
            MODEL model = AutoMapper.Mapper.Map<MODEL>(dto);
            return model;
        }

        internal static DTO convertToDto<MODEL>(MODEL model)
        {
            if (model == null) return null;
            AutoMapper.Mapper.CreateMap<MODEL, DTO>();
            DTO dto = AutoMapper.Mapper.Map<DTO>(model);
            return dto;
        }
    }
}