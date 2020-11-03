using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Payments.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateUser(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        IEnumerable<UserDTO> GetAllClientProfiles();
        IEnumerable<UserDTO> GetAllClientProfiles(int paymentCount, int sectorNumber, string sortFieldName);
        int GetNumberOfProfiles();
        UserDTO FindUserById(string id);
        UserDTO FindUserByEmail(string email);
        OperationDetails UploadPicture(PictureDTO pictureDto, string userId);
        void EditPassword(UserDTO userDto, string userId);
        void ResetPassword(UserDTO userDto, string email, string token, string userId);
        Task SendEmailAsync(string userId, string body);
        string GeneratePasswordResetToken(string userId);
    }
}
