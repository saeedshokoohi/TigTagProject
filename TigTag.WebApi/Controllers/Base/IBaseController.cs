using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiTag.Repository;
using TiTag.Repository.Base;

namespace TigTag.WebApi.Controllers
{
    public interface IBaseController<MODEL> where MODEL : class
    {
        IGenericRepository<MODEL> getRepository(); 
    }
}