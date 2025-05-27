using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    internal class SubjectBL
    {
        private readonly DAL.Api.ISubjectServiceDAL _subjectServiceDAL; 
        public SubjectBL(DAL.Api.ISubjectServiceDAL subjectServiceDAL)
        {
            _subjectServiceDAL = subjectServiceDAL;
        }
        public async Task<List<DAL.Models.Subject>> GetAllSubjectsAsync()
        {
            return await _subjectServiceDAL.GetAllSubjects();
        }

    }
}
