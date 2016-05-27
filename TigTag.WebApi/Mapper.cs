using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TigTag.WebApi
{
    public class Mapper<MODEL,DTO>
    {
        public static MODEL convertToModel(DTO dto)
        {
            AutoMapper.Mapper.CreateMap<DTO, MODEL>();
            MODEL model = AutoMapper.Mapper.Map<MODEL>(dto);
            return model;
        }
    }
}