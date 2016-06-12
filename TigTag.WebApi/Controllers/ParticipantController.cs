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
    public class ParticipantController : BaseController<Participant,ParticipantDto>
    {
 
        ParticipantRepository ParticipantRepo = new ParticipantRepository();

        public override IGenericRepository<Participant> getRepository()
        {
            return ParticipantRepo;
        }
        public ResultDto addParticipant(ParticipantDto participantModelDto)
        {
            if (participantModelDto == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Participant paticipantMOdel = Mapper<Participant, ParticipantDto>.convertToModel(participantModelDto);
                paticipantMOdel.Id = Guid.NewGuid();
                paticipantMOdel.CreateDate = DateTime.Now;
                     returnResult = ParticipantRepo.validateParticipant(paticipantMOdel);
                if (returnResult.isDone)
                {
                    try
                    {
                        ParticipantRepo.Add(paticipantMOdel);
                        ParticipantRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new participant created successfully";
                        returnResult.returnId = paticipantMOdel.Id.ToString();
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }
                return returnResult;
            }
        }

     
    }
}