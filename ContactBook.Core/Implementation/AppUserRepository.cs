using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Core.DTO;
using ContactBook.Core.Services;
using ContactBook.Data;
using ContactBook.Infrastructure.Helper;
using ContactBook.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Core.Implementation
{

    public class AppUserRepository: IAppUserRepository
    {
        //AddUser
        //getalluser
        //getuserbyId
        private readonly AppDbContext _context;
        private readonly IPhotoServices _photoServices;
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepository(AppDbContext context, IPhotoServices photoServices, UserManager<AppUser> userManager)
        {
            _context = context;
            _photoServices = photoServices;
            _userManager = userManager;
        }

        public async Task<string> AddUser(AppUserDTO appUser)
        {
            var existingUSer = await _context.AppUsers.FirstOrDefaultAsync(e => e.PhoneNumber == appUser.PhoneNumber);
            if (existingUSer != null)
            {
                return "User already Exist";
            }

            var newAppUser = new AppUser
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                City = appUser.City,
                Country = appUser.Country,

            };
            _context.AppUsers.Add(newAppUser);
            var saveChanges = await _context.SaveChangesAsync();
            if (saveChanges > 0)
            {
                return "User added successfully";
            }

            return "User could not be added";
        }

        public async Task<List<AppUserDTO>> SearchForTerm(string term)
        {

            if (string.IsNullOrWhiteSpace(term))
            {
                return new List<AppUserDTO>();
            }
            var users = await _userManager.Users
                .Where(p => p.Email.Contains(term)
                            || p.FirstName.Contains(term)
                            || p.LastName.Contains(term)
                            || p.City.Contains(term)
                            || p.State.Contains(term)
                            || p.Country.Contains(term)
                ).ToListAsync();
            var AppUserDTO = users.Select(item => new AppUserDTO
            {
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                ImageUrl = item.ImageUrl,
                City = item.City,
                State = item.State,
                Country = item.Country,
                FacebookUrl = item.FacebookUrl,
                TwitterUrl = item.TwitterUrl
            }).ToList();
            return AppUserDTO;

        }

       
        public async Task<AppUserDTO> GetAppUserById(string id)
        {
            var userData = await _context.AppUsers.FirstOrDefaultAsync(p => p.Id == id);
            var newuser = new AppUserDTO
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email,
                PhoneNumber = userData.PhoneNumber,
                City = userData.City,
                Country = userData.Country,
                ImageUrl = userData.ImageUrl,
                State = userData.State,
                FacebookUrl = userData.FacebookUrl,
                TwitterUrl = userData.TwitterUrl
            };
            return newuser;
        }


        public async Task<string> UpdateAppUser(AppUser appUser)
        {
            var existingUser = await _context.AppUsers.FirstOrDefaultAsync(e => e.Id == appUser.Id);
            if (existingUser != null)
            {
                _context.AppUsers.Update(existingUser);
                return "User updated successfully";
            }

            return "No User found";
        }

        public async Task<bool> AddPhoto(string id, PhotoDTO photoDTO)
        {
            var result = await _photoServices.AddPhotoAsync(photoDTO.ImageUrl);
            var existingUser = await _userManager.FindByIdAsync(id);
            existingUser.ImageUrl = result.Url.AbsolutePath;
            await _userManager.UpdateAsync(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<AppUserDTO>> GetAllUser(PaginParameter userParameter)
        {
            var contacts = _context.AppUsers
                .OrderBy(p => p.FirstName)
                .Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize)
                .ToList();


            var data = new List<AppUserDTO>();
            foreach (var userData in _context.AppUsers.ToList())
            {
                data.Add(new AppUserDTO
                {
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Email,
                    PhoneNumber = userData.PhoneNumber,
                    City = userData.City,
                    Country = userData.Country,
                    ImageUrl = userData.ImageUrl,
                    State = userData.State,
                    FacebookUrl = userData.FacebookUrl,
                    TwitterUrl = userData.TwitterUrl
                });
            }
            return data; /*.Skip((userParameter.PageNumber - 1) * userParameter.PageSize)
                .Take(userParameter.PageSize).ToList();*/
        }
    }
}
 