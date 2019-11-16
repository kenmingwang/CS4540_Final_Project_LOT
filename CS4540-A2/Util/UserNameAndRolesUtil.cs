using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS4540_A2.Util
{
    public class UserNameAndRolesUtil
    {
        // danny_kopta => [Danny,Kopta]
        public static string[] UserNameToActualName(string UserName)
        {
            var name = UserName.Split("_");
            var first = name[0].First().ToString().ToUpper() + String.Join("", name[0].Skip(1));
            var last = name[1].First().ToString().ToUpper() + String.Join("", name[1].Skip(1)); ;
            string[] rtr = { first, last };
            return rtr;
        }

        // [Danny,Kopta] => danny_kopta
        public static string ActualNameToUserName(string[] ActualName)
        {
            var first = ActualName[0].ToLower();
            var last = ActualName[1].ToLower();
            
            string rtr = first + "_" + last;
            return rtr;
        }
    }
}
