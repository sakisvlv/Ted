using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ted.Model;

namespace Ted.Bll.Interfaces
{
    public interface IUserService
    {
        Task<Result<byte[]>> GetPhoto(string userId);
        Task<Result<string>> InsertPhoto(string userId, byte[] PhotoByteArray);
    }
}
