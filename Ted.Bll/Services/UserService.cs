using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;

namespace Ted.Bll.Services
{
    public class UserService : IUserService
    {
        private readonly Context _context;

        public UserService(Context context)
        {
            _context = context;
        }
        public async Task<Result<string>> InsertPhoto(string userId, byte[] PhotoByteArray)
        {
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.NotFound, "Αδυναμία ανάκτησης υπολοιίου");
            }

            Photo photo = new Photo();
            photo.File = PhotoByteArray;
            user.Photo = photo;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Result<string>.CreateFailed(
                    HttpStatusCode.InternalServerError, "Αδυναμία Αποθήκευσης Φωτογραφίας");
            }
            return Result<string>.CreateSuccessful("Η φωτογραφία σας αποθηκεύτηκε επιτυχώς");
        }
        public async Task<Result<string>> GetPhoto(string userId)
        {
            var photo = await _context.Photos.FindAsync(Guid.Parse(userId));
            if (photo == null)
            {
                return Result<string>.CreateFailed(
                   HttpStatusCode.NotFound, "Αδυναμία φόρτωσης φωτογραφίας");
            }
            return Result<string>.CreateSuccessful(Convert.ToBase64String(photo.File));
        }
    }
}
