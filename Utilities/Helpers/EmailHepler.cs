using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities.Helpers
{
    public class EmailHepler
    {
         public bool EmailIsValid(string email)
    {
        string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        if (Regex.IsMatch(email, expression))
        {
            if (Regex.Replace(email, expression, string.Empty).Length == 0)
            {
                return true;
            }
        }
        return false;
    }
    }
}
