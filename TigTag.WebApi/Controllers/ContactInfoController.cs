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
using TigTag.Common.Enumeration;
using TigTag.Common.util;
using System.Web.OData;

namespace TigTag.WebApi.Controllers
{
    public class ContactInfoController : BaseController<ContactInfo, ContactInfoDto>
    {

        ContactInfoRepository ContactInfoRepo = new ContactInfoRepository();
        MenuRepository menuRepo = new MenuRepository();
        PageRepository pageRepo = new PageRepository();
        public ContactInfoController()
        {
            eventLogRepo.Context = ContactInfoRepo.Context;
        }
        public override IGenericRepository<ContactInfo> getRepository()
        {
            return ContactInfoRepo;
        }
        public ResultDto editContactInfoList(ContactInfoListDto contactInfoListDto)
        {
            ResultDto result = new ResultDto();
         
            Page p = pageRepo.GetSingle(contactInfoListDto.pageId);
            pageRepo.Detach(p);
            if (p == null) throwException("pageid is not valid");
            if (p.UserId != getCurrentUserId()) throwException("current user is not the owner of pageid and can not edit pageAdmin");
            List<ContactInfo> currentList = ContactInfoRepo.getContactInfoByPage(p.Id);
            List<Guid> toDeleteList = new List<Guid>();
            List<ContactInfoDto> toAddList = new List<ContactInfoDto>();
            List<ContactInfoDto> toEditList = new List<ContactInfoDto>();

            foreach (var item in contactInfoListDto.contactInfoList)
            {
                if(item.Id!=Guid.Empty)
                {
                    ContactInfo temp = ContactInfoRepo.GetSingle(item.Id);
                    if (temp == null) throwException("contact info id:" + item.Id + "is not valid");
                    toEditList.Add(item);
                }
            }
            foreach (var item in contactInfoListDto.contactInfoList)
            {

                item.PageId = contactInfoListDto.pageId;
                if (!currentList.Any(pa => pa.Id == item.Id))
                    toAddList.Add(item);

            }
            foreach (var item in currentList)
            {
                ContactInfoRepo.Detach(item);
                if (!contactInfoListDto.contactInfoList.Any(ci=>ci.Id==item.Id))
                    toDeleteList.Add(item.Id);
            }
            foreach (var item in toEditList)
            {
                result = editContactInfoNotSave(item);
                if (!result.isDone) return result;
            }
            foreach (var item in toAddList)
            {
               result= addContactInfoNotSave(item);
                if (!result.isDone) return result;
            }
            foreach (var item in toDeleteList)
            {
                ContactInfo temp = ContactInfoRepo.GetSingle(item);
              
                eventLogRepo.RemoveContactInfoEvent(getCurrentProfileId(), temp);
                ContactInfoRepo.Delete(temp);

            }
            try
            {
                ContactInfoRepo.Save();


                return ResultDto.successResult("", String.Format("{0} item added and {1} item removed and {2} item edited ", 
                    toAddList.Count().ToString(), toDeleteList.Count().ToString(),toEditList.Count()));
            }
            catch (Exception ex)
            {
                return ResultDto.exceptionResult(ex);
            }


        }
        public ResultDto addContactInfo(ContactInfoDto contactInfo)
        {
            if (contactInfo == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                ContactInfo ContactInfoModel = Mapper<ContactInfo, ContactInfoDto>.convertToModel(contactInfo);
                ContactInfoModel.Id = Guid.NewGuid();

                returnResult = ContactInfoRepo.validateContactInfo(ContactInfoModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        ContactInfoRepo.Add(ContactInfoModel);
                        ContactInfoRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new ContactInfo created successfully";
                        returnResult.returnId = ContactInfoModel.Id.ToString();
                        if (contactInfo.ProfileId == Guid.Empty) contactInfo.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddContactInfoEvent(contactInfo.ProfileId, ContactInfoModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }
        public ResultDto addContactInfoNotSave(ContactInfoDto contactInfo)
        {
            if (contactInfo == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                ContactInfo ContactInfoModel = Mapper<ContactInfo, ContactInfoDto>.convertToModel(contactInfo);
                ContactInfoModel.Id = Guid.NewGuid();

                returnResult = ContactInfoRepo.validateContactInfo(ContactInfoModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        ContactInfoRepo.Add(ContactInfoModel);
                      
                        returnResult.isDone = true;
                        returnResult.message = "new ContactInfo created successfully";
                        returnResult.returnId = ContactInfoModel.Id.ToString();
                        if (contactInfo.ProfileId == Guid.Empty) contactInfo.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddContactInfoEvent(contactInfo.ProfileId, ContactInfoModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }
        public ResultDto editContactInfoNotSave(ContactInfoDto contactInfo)
        {
            if (contactInfo == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                ContactInfo ContactInfoModel = Mapper<ContactInfo, ContactInfoDto>.convertToModel(contactInfo);
                ContactInfoModel.ModifiedBy = getCurrentUserId();
                ContactInfoModel.ModifiedDate = DateTime.Now;

                  returnResult = ContactInfoRepo.validateContactInfo(ContactInfoModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        ContactInfoRepo.Edit(ContactInfoModel);

                        returnResult.isDone = true;
                        returnResult.message = "new ContactInfo edited successfully";
                        returnResult.returnId = ContactInfoModel.Id.ToString();
                        if (contactInfo.ProfileId == Guid.Empty) contactInfo.ProfileId = getCurrentProfileId();
                        eventLogRepo.EditContactInfoEvent(contactInfo.ProfileId, ContactInfoModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }

        public ResultDto AddContactInfoList(List<ContactInfoDto> contactInfoList)
        {
            ResultDto retResult = new ResultDto();
            try
            {
                foreach (var item in contactInfoList)
                {
                    ContactInfo itemModel = Mapper<ContactInfo, ContactInfoDto>.convertToModel(item);
                    retResult= ContactInfoRepo.validateContactInfo(itemModel);
                    if (retResult.isDone == false) return retResult;
                }
                foreach (var item in contactInfoList)
                {
                    addContactInfo(item);
                }
                retResult.message = contactInfoList.Count + " contactInfos Added Successfully! ";
            }
            catch(Exception ex)
            {
                retResult = ResultDto.exceptionResult(ex);
            }
            return retResult;
        }
    }

    public class ContactInfoListDto
    {
        public IEnumerable<ContactInfoDto> contactInfoList { get;  set; }
        public Guid pageId { get;  set; }
    }
}