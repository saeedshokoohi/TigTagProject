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
using TigTag.Repository;

namespace TigTag.WebApi.Controllers
{
    public class PageAdminController : BaseController<PageAdmin,PageAdminDto>
    {
 
        PageAdminRepository PageAdminRepo = new PageAdminRepository();

        public override IGenericRepository<PageAdmin> getRepository()
        {
            return PageAdminRepo;
        }

        public ResultDto editPageAdminList(PageAdminListDto pageAdminListDto)
        {
            ResultDto result = new ResultDto();
            PageRepository pageRepo = new PageRepository();
            Page p = pageRepo.GetSingle(pageAdminListDto.pageId);
            pageRepo.Detach(p);
            if (p == null) throwException("pageid is not valid");
            if(p.UserId!=getCurrentUserId()) throwException("current user is not the owner of pageid and can not edit pageAdmin");
            List<PageAdminDto> currentList= PageAdminRepo.getPageAdmins(p.Id);
            List<Guid> toDeletePageAdminList = new List<Guid>();
            List<Guid> toAddPageAdminList = new List<Guid>();
            foreach (var item in pageAdminListDto.adminList)
            {
                if (!currentList.Any(pa => pa.AdminProfileId == item))
                    toAddPageAdminList.Add(item);
                
            }
            foreach (var item in currentList)
            {
                if (!pageAdminListDto.adminList.Contains(item.AdminProfileId))
                    toDeletePageAdminList.Add(item.Id);
            }
           
            foreach (var item in toAddPageAdminList)
            {
                PageAdmin newPageAdmin = new PageAdmin();
                newPageAdmin.Id = Guid.NewGuid();
                newPageAdmin.CreateDate = DateTime.Now;
                newPageAdmin.IsActive = true;
                newPageAdmin.PageId = pageAdminListDto.pageId;
                newPageAdmin.AdminProfileId = item;
                result=PageAdminRepo.validatePageAdmin(newPageAdmin);
                if (!result.isDone) return result;
                PageAdminRepo.Add(newPageAdmin);
                eventLogRepo.addPageAdminEvent(getCurrentProfileId(), newPageAdmin);
            }
            foreach (var item in toDeletePageAdminList)
            {
                PageAdmin temp = PageAdminRepo.GetSingle(item);
                
                eventLogRepo.removePageAdminEvent(getCurrentProfileId(), temp);
                PageAdminRepo.Delete(temp);

            }
            try {
                PageAdminRepo.Save();
               
               
                return ResultDto.successResult("", String.Format("{0} item added and {1} item removed ",toAddPageAdminList.Count().ToString(),toDeletePageAdminList.Count().ToString()));
            }
            catch(Exception ex) {
                return ResultDto.exceptionResult(ex);
            }


        }
        public ResultDto addPageAdmin(PageAdminDto PageAdminModelDto)
        {
            if (PageAdminModelDto == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an in json object format  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageAdmin pageAdminModel = Mapper<PageAdmin, PageAdminDto>.convertToModel(PageAdminModelDto);
                pageAdminModel.Id = Guid.NewGuid();
                pageAdminModel.CreateDate = DateTime.Now;
                     returnResult = PageAdminRepo.validatePageAdmin(pageAdminModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        removePageAdmin(PageAdminModelDto);
                        PageAdminRepo.Add(pageAdminModel);
                        PageAdminRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new PageAdmin created successfully";
                        returnResult.returnId = pageAdminModel.Id.ToString();
                        eventLogRepo.addPageAdminEvent(PageAdminModelDto.ProfileId, pageAdminModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }
                return returnResult;
            }
        }

        public List<PageAdminDto> getPageAdmins(Guid pageId)
        {
          return  PageAdminRepo.getPageAdmins(pageId);
        }
        public ResultDto removePageAdmin(PageAdminDto pageAdminDto)
        {
            return PageAdminRepo.removePageAdmin(pageAdminDto.AdminProfileId, pageAdminDto.PageId);
        }
    }

    public class PageAdminListDto
    {
        public List<Guid> adminList { get;  set; }
        public Guid pageId { get;  set; }
    }
}