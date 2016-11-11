using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class UserRepository : 
        GenericRepository<DataModelContext, User>, IUserRepository  {
        private readonly string PHONE_NUMBER_ALREADY_EXISTS= "PHONE_NUMBER_ALREADY_EXISTS";
        private readonly string EMAIL_ADDRESS_ALREADY_EXISTS= "EMAIL_ADDRESS_ALREADY_EXISTS"; 
        private readonly string USER_NAME_ALREADY_EXISTS = "USER_NAME_ALREADY_EXISTS";
        private readonly string USER_NAME_IS_EMPTY= "USER_NAME_IS_EMPTY";
        private readonly string EMAIL_ADDRESS_IS_EMPTY= "EMAIL_ADDRESS_IS_EMPTY";
        private readonly string PHONE_NUMBER_IS_EMPTY= "PHONE_NUMBER_IS_EMPTY";

        public User GetSingle(Guid Id) {

            var query = Context.Users.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateUser(User userModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkUserName(userModel, retResult);
            checkEmailAddress(userModel, retResult);
            checkMobileNumber(userModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkMobileNumber(User userModel, ResultDto retResult)
        {
            if (userModel == null || String.IsNullOrEmpty(userModel.PhoneNumber))
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages(PHONE_NUMBER_IS_EMPTY);
            }
            else
            {
                var c = Context.Users.Count(u => u.PhoneNumber == userModel.PhoneNumber && u.Id != userModel.Id);
                if (c > 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages(PHONE_NUMBER_ALREADY_EXISTS);
                }
            }

        }

        public User findByUserName(string username)
        {
           List<User> users= Context.Users.Where(u => u.UserName == username).ToList();
            if (users != null && users.Count > 0)
                return users[0];
            else
                return null;
        }

        private void checkEmailAddress(User userModel, ResultDto retResult)
        {
            if (userModel == null || String.IsNullOrEmpty(userModel.EmailAddress))
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages(EMAIL_ADDRESS_IS_EMPTY);
            }
            else
            {
                var c = Context.Users.Count(u => u.EmailAddress == userModel.EmailAddress && u.Id != userModel.Id);
                if (c > 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages(EMAIL_ADDRESS_ALREADY_EXISTS);
                }
            }
        }

        public ResultDto login(string username, string password)
        {
            ResultDto retResult = new ResultDto();
            var query = from u in Context.Users where u.UserName.ToLower().Equals(username.ToLower()) && u.Password == password select u;
            List<User> user=query.ToList();
            if(user.Count>0)
            {
                retResult.isDone = true;
                retResult.message = "Login is Successful!";
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
                retResult.returnId = user[0].Id.ToString();
                
                user[0].LastLoginDate = DateTime.Now;
                Context.SaveChanges();
            }
            else
            {
                retResult.isDone = false;
                retResult.message = "login failed,username or password is not valid";
                retResult.statusCode = enm_STATUS_CODE.FAILED_WITH_ERROR;

            }
            return retResult;
        }

        private void checkUserName(User userModel, ResultDto retResult)
        {
            if (userModel == null || String.IsNullOrEmpty(userModel.UserName))
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages(USER_NAME_IS_EMPTY);
            }
            else
            {
                var c = Context.Users.Count(u => u.UserName == userModel.UserName && u.Id!=userModel.Id);
                if (c > 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages(USER_NAME_ALREADY_EXISTS);
                }
            }
        }
    }
}