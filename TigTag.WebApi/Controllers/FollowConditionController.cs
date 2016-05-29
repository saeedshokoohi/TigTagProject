using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.DTO.ModelDTO;
using TigTag.Repository.ModelRepository;

using TiTag.Repository;

namespace TigTag.WebApi.Controllers
{
    public class FollowConditionController : BaseController<FollowCondition,FollowConditionDto>
    {
 
        FollowConditionRepository FollowConditionRepo = new FollowConditionRepository();

        public override IGenericRepository<FollowCondition> getRepository()
        {
            return FollowConditionRepo;
        }
    }
}