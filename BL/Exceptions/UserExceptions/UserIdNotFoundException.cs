using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class UserIdNotFoundException : Exception
    {
        public UserIdNotFoundException(int id) : base($"The user with the id: {id} does not exist")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}





