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

namespace TigTag.WebApi.Controllers
{
    public class PageController : BaseController<User>
    {
        UserRepository userRepo = new UserRepository();
        PageRepository pageRepo = new PageRepository();

       
  
    }
}