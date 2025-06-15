using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class UserIdAlreadyExistsException : Exception
    {
        public UserIdAlreadyExistsException(int id) : base($"The user with the id: {id} already exist")
        {

        }
        public int? StatusCode { get; } = 440;
    }

}

