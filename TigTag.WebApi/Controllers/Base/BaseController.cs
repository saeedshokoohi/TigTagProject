using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository;
using TigTag.Repository.ModelRepository;
using TiTag.Repository;
using TiTag.Repository.Base;

namespace TigTag.WebApi.Controllers
{

    [Authorize]
    public abstract class BaseController<MODEL,DTO>  : ApiController , IBaseController<MODEL> where MODEL : class where DTO: class
    {

        public abstract IGenericRepository<MODEL> getRepository();
        public EventsLogRepository eventLogRepo = new EventsLogRepository();

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
        public DTO getById(Guid id)
        {
           var model= getRepository().GetSingle(id);
            
           return Mapper<MODEL, DTO>.convertToDto(model);
             
        }
        public ResultDto deleteById(Guid id)
        {
            try
            {
                var model = getRepository().GetSingle(id);
                getRepository().Delete(model);
            }catch(Exception ex)
            {
                ResultDto.exceptionResult(ex);
            }
            return ResultDto.successResult(id.ToString(), "entity with given id deleted successfully...");

        }
        public List<DTO> getAll()
        {
            List<DTO> returnList = new List<DTO>();
            var entityList= getRepository().GetAll().ToList();
            foreach (var item in entityList)
            {
                returnList.Add(Mapper<MODEL, DTO>.convertToDto(item));
            }
            return returnList;
        }
        public Guid getCurrentProfileId()
        {
            
            PageRepository pageRepo = new PageRepository();
            Page p= pageRepo.findByUserName(User.Identity.Name);
            if (p != null) return p.Id;
            else return Guid.Empty;
            
            ;
        }

    }
}
