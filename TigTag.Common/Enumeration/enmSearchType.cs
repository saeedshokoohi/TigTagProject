using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.Common.Enumeration
{
    public enum enmSearchType
    {
//        1) رویداد هایی که این کاربر ساخته 
//2) پست های این صفحه
//3) تیم های این صفحه
//4) دنبال کننده های این صفحه
//5) کسایی که من دنبال کردم
//6) شرکت کنندگان

           
        SEARCH_ON_EVENTS=1,
        SEARCH_ON_POSTS = 2,
        SEARCH_ON_TEAMS = 3,
        SEARCH_ON_FOLLOWERS = 4,
        SEARCH_ON_FOLLOWINGS = 5,
        SEARCH_ON_PARTICIPANTS = 6
        
    }
}
