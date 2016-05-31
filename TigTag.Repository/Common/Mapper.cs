using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;

namespace TigTag.Repository
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

        public static DTO convertToDto<MODEL>(MODEL model)
        {
            if (model == null) return null;
            AutoMapper.Mapper.CreateMap<MODEL, DTO>();
            DTO dto = AutoMapper.Mapper.Map<DTO>(model);
            return dto;
        }

        public static List<DTO> convertListToDto(List<MODEL> models)
        {
            List<DTO> retList = new List<DTO>();
            if (models == null) return null;
            foreach (var item in models)
            {
                retList.Add(Mapper<MODEL, DTO>.convertToDto(item));
            }
            return retList;
        }
    }
}