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
   
    public class UserController : BaseController<User,UserDto>
    {
        UserRepository userRepo = new UserRepository();
        public override IGenericRepository<User> getRepository()
        {
            return userRepo;
        }
        /// <summary>
        /// this service register a new user
        /// service url : user/registerUser
        /// </summary>
        /// <param name="user">user data to register</param>
        /// <returns>the result service</returns>
        public ResultDto RegisterUserWithotAccess([FromBody]UserDto user)
        {
            
            ResultDto returnResult = new ResultDto();
            AutoMapper.Mapper.CreateMap<UserDto, User>();
            User UserModel = AutoMapper.Mapper.Map<User>(user);
            UserModel.Id = Guid.NewGuid();
            UserModel.CreateDate = DateTime.Now;
            UserModel.IsActive = true;
            returnResult=userRepo.validateUser(UserModel);
            if (returnResult.isDone)
            {
                try
                {
                    UserModel.Password = "";
                    userRepo.Add(UserModel);
                    userRepo.Save();
                    returnResult.isDone = true;
                    returnResult.message = "new user added successfully";
                    returnResult.returnId = UserModel.Id.ToString();
                }
                catch (Exception ex)
                {
                    returnResult.isDone = false;
                    returnResult.message = ex.Message;

                }
            }
            else
            {
                returnResult.message = "input is not valid. check the validation messages";
            }
            return returnResult;
        }
        public ResultDto EditUser([FromBody]UserDto user)
        {
            if (user.Id == null || user.Id == Guid.Empty || user.Id != getCurrentUserId())
                throwException("User id is not valid or the current user has not access to edit given user id");
            ResultDto returnResult = new ResultDto();
             User oldUser= userRepo.GetSingle(user.Id);
            userRepo.Detach(oldUser);
            if(oldUser==null) throwException("user id is not valid! there is no user with given id.");

            User UserModel=Mapper<User, UserDto>.convertToModel(user);
            UserModel.CreateDate = oldUser.CreateDate;
            UserModel.ModifiedDate = DateTime.Now;
            UserModel.UserName = oldUser.UserName;
            UserModel.ModifiedBy = getCurrentUserId();
            returnResult = userRepo.validateUser(UserModel);
            if (returnResult.isDone)
            {
                try
                {
                    UserModel.Password = "";
                    userRepo.Edit(UserModel);
                    userRepo.Save();
                    returnResult.isDone = true;
                    returnResult.message = " user edited successfully";
                    returnResult.returnId = UserModel.Id.ToString();
                }
                catch (Exception ex)
                {
                    returnResult.isDone = false;
                    returnResult.message = ex.Message;

                }
            }
            else
            {
                returnResult.message = "input is not valid. check the validation messages";
            }
            return returnResult;
        }
        /// <summary>
        /// this service return the list of all registered users
        /// </summary>
        /// <returns> list of users </returns>
        ///

        public ResultDto validateUser(UserDto user)
        {
            ResultDto returnResult = new ResultDto();
            AutoMapper.Mapper.CreateMap<UserDto, User>();
            User UserModel = AutoMapper.Mapper.Map<User>(user);
            returnResult = userRepo.validateUser(UserModel);
            return returnResult;
        }
        public List<UserDto> getRegisteredUser()
        {
          List<User> users=  userRepo.GetAll().ToList();
            AutoMapper.Mapper.CreateMap<User, UserDto>();
            List<UserDto> UserdtoList = AutoMapper.Mapper.Map<List<UserDto>>(users);

          return UserdtoList;

            
        }
        /// <summary>
        /// this service check user name and password and return result
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private ResultDto login([FromBody]UserDto user)
        {
            ResultDto returnResult;
            try
            {
                returnResult = userRepo.login(user.UserName, user.Password);
            }catch(Exception ex)
            {
                returnResult = new ResultDto();
                returnResult.isDone = false;
                returnResult.message = ex.Message+ex.StackTrace;
            }
            return returnResult;
        }

     
    }
}