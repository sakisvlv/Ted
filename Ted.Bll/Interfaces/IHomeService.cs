using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;
using Ted.Model.DTO;

namespace Ted.Bll.Interfaces
{
    public interface IHomeService
    {
        Task<Result<ExperienceDTO>> GetLastExperience(string userId);
    }
}
