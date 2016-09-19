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

        public override IGenericRepository<ContactInfo> getRepository()
        {
            return ContactInfoRepo;
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
}