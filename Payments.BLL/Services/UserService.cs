using AutoMapper;
using Microsoft.AspNet.Identity;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Entities;
using Payments.DAL.Entities.Cards;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        IMapper UserMapper { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;

            UserMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreditCard, CreditCardDTO>();
                cfg.CreateMap<Account, AccountDTO>();
                cfg.CreateMap<ClientProfile, UserDTO>();
                cfg.CreateMap<Picture, PictureDTO>();
            }).CreateMapper();
        }

        public async Task<OperationDetails> CreateUser(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email, Role = "user" };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                var mapper = new MapperConfiguration(c => c.CreateMap<PictureDTO, Picture>()).CreateMapper();
                var picture = mapper.Map<PictureDTO, Picture>(userDto.Picture);
                ClientProfile clientProfile = new ClientProfile
                {
                    Id = user.Id,
                    Name = userDto.Name,
                    MiddleName = userDto.MiddleName,
                    Surname = userDto.Surname,
                    BirthDate = userDto.BirthDate,
                    Picture = picture
                    //Residence = residence
                };

                Database.ClientRepository.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Registration completed successfully", "");
            }
            else
            {
                return new OperationDetails(false, "A user with this login already exists", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public IEnumerable<UserDTO> GetAllClientProfiles()
        {
            return UserMapper.Map<IEnumerable<ClientProfile>, IEnumerable<UserDTO>>(Database.ClientRepository
                .FindAll().Where(x => x.ApplicationUser.Role == "user" || x.ApplicationUser.Role == "blockedUser"));
        }

        public IEnumerable<UserDTO> GetAllClientProfiles(int paymentCount, int sectorNumber, string sortFieldName)
        {
            var users = Database.ClientRepository
                .FindAll().Where(x => x.ApplicationUser.Role == "user" || x.ApplicationUser.Role == "blockedUser");
            switch (sortFieldName)
            {
                case "name_asc":
                    users = users.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    users = users.OrderByDescending(x => x.Name);
                    break;
                case "surname_asc":
                    users = users.OrderBy(x => x.Surname);
                    break;
                case "surname_desc":
                    users = users.OrderByDescending(x => x.Surname);
                    break;
                case "middleName_asc":
                    users = users.OrderBy(x => x.MiddleName);
                    break;
                case "middleName_desc":
                    users = users.OrderByDescending(x => x.MiddleName);
                    break;
                case "birthDate_asc":
                    users = users.OrderBy(x => x.BirthDate);
                    break;
                case "birthDate_desc":
                    users = users.OrderByDescending(x => x.BirthDate);
                    break;
                case "status_asc":
                    users = users.OrderBy(x => x.IsBlocked);
                    break;
                case "status_desc":
                    users = users.OrderByDescending(x => x.IsBlocked);
                    break;
                default:
                    users = users.OrderByDescending(x => x.Id);
                    break;
            }
            return UserMapper.Map<IEnumerable<ClientProfile>, IEnumerable<UserDTO>>(users)
                .Skip((sectorNumber - 1) * paymentCount).Take(paymentCount);
        }

        public int GetNumberOfProfiles()
        {
            return Database.ClientRepository.FindAll()
                .Where(x => x.ApplicationUser.Role == "user" || x.ApplicationUser.Role == "blockedUser").Count();
        }

        public UserDTO FindUserByEmail(string email)
        {
            var user = Database.UserManager.FindByEmail(email);

            if (user != null)
            {
                return new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email
                };
            }
            return null;
        }

        public UserDTO FindUserById(string id)
        {
            var user = Database.UserManager.FindById(id);

            if (user != null)
            {
                var cards = UserMapper.Map<IEnumerable<CreditCard>, List<CreditCardDTO>>(user.ClientProfile.Cards);
                var accounts = UserMapper.Map<IEnumerable<Account>, List<AccountDTO>>(user.ClientProfile.Accounts);
                PictureDTO picture = null;

                if (user.ClientProfile.Picture!=null)
                {  picture = new PictureDTO();
                    picture.Image = user.ClientProfile.Picture.Image;
                    picture.Name = user.ClientProfile.Picture.Name;
                }
                return new UserDTO()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.ClientProfile.Name,
                    Surname = user.ClientProfile.Surname,
                    MiddleName = user.ClientProfile.MiddleName,
                    BirthDate = user.ClientProfile.BirthDate,
                    Cards = cards,
                    Accounts = accounts,
                    Picture = picture
                };

            }
            return null;
        }

        public OperationDetails UploadPicture(PictureDTO pictureDto, string userId)
        {
            var user = Database.UserManager.FindById(userId);

            if (user!=null)
            {
                if (pictureDto!= null)
                {
                    Picture picture = new Picture()
                    {
                        ClientProfile = user.ClientProfile,
                        Image = pictureDto.Image,
                        Name = pictureDto.Name,
                        IsBlocked = pictureDto.IsBlocked,
                        IsDeleted = pictureDto.IsDeleted
                    };
                    Database.PictureRepository.Create(picture);
                    Database.PictureRepository.SaveChanges();
                    return new OperationDetails(true, "Picture added successfully", "");
                }
                else
                {
                    return new OperationDetails(false, "File problem", "Image");    
                }
            }
            else
            {
                return new OperationDetails(false, "User doesn't  exist", "Id");
            }
        }

        public void EditPassword(UserDTO userDto, string userId)
        {
            var user = Database.UserManager.FindById(userId);
            if (user != null)
            {
                user.PasswordHash = Database.UserManager.PasswordHasher.HashPassword(userDto.Password);
                Database.UserManager.Update(user);
                Database.Save();
            }
        }

        public void ResetPassword(UserDTO userDto, string email, string token, string userId)
        {
            var user = Database.UserManager.FindByEmail(email);
            if (user != null)
            {
                Database.UserManager.ResetPassword(userId, token, userDto.Password);
                Database.Save();
            }
        }

        public async Task SendEmailAsync(string userId, string body)
        {
            if (userId != null && body != null)
            {
                await Database.UserManager.SendEmailAsync(userId, "Confirm Password", body);
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public string GeneratePasswordResetToken(string userId)
        {
            return Database.UserManager.GeneratePasswordResetToken(userId);
        }
    }
}
