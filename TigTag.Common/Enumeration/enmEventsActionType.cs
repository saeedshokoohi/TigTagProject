using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.Common.Enumeration
{
    public enum enmEventsActionType
    {
        ACTION_NOT_SET=0,
        CREATE_PROFILE=1,
        CREATE_POST=2,
        CREATE_TEAM=3,
        TAKE_PARTICIPATE=4,
        CREATE_PAGE=5,
        ROLE_AS_ADMIN=6,
        FOLLOWING_PAGE=7,
        FOLLOWING_MENU=8,
        ADD_COMMENT=9,
        ADD_COMMENTREPLY=10,
        CREATE_ORDER = 11,
        CREATE_TICKET = 12,
        ADD_ORDER_ITEM = 13,
        ADD_CONTACT_INFO = 14,
        //edit
        EDIT_PROFILE = 101,
        EDIT_POST = 102,
        EDIT_TEAM = 103,
        EDIT_PAGE = 105,
        EDIT_TICKET = 112,
        EDIT_CONTACT_INFO = 114,
        //delete
        REMOVE_ROLE_AS_ADMIN = 206,
        REMOVE_TICKET = 212,
        REMOVE_CONTACT_INFO = 214,
        REMOVE_FOLLOWING_PAGE = 217
    }
}
