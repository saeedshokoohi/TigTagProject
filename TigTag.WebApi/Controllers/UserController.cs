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
    public class UserController : BaseController<User>
    {
        UserRepository userRepo = new UserRepository();
        
        /// <summary>
        /// this service register a new user
        /// service url : user/registerUser
        /// </summary>
        /// <param name="user">user data to register</param>
        /// <returns>the result service</returns>
        public ResultDto RegisterUser([FromBody]UserDto user)
        {
            
            ResultDto returnResult = new ResultDto();
            User UserModel = AutoMapper.Mapper.Map<User>(user);
            UserModel.Id = Guid.NewGuid();
            try
            {
                userRepo.Add(UserModel);
                returnResult.isDone = true;
                returnResult.message = "new user added successfully";
                returnResult.returnId = UserModel.Id.ToString();
            }
            catch (Exception ex)
            {
                returnResult.isDone = false;
                returnResult.message = ex.Message;

            }

            return returnResult;
        }
        /// <summary>
        /// this service return the list of all registered users
        /// </summary>
        /// <returns> list of users </returns>
        public List<UserDto> getRegisteredUser()
        {
          List<User> users=  userRepo.GetAll().ToList();
          List<UserDto> UserdtoList = AutoMapper.Mapper.Map<List<UserDto>>(users);

          return UserdtoList;

            
        }

  
    }
}