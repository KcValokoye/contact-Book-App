using ContactBook.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactBook.Infrastructure.Helper;
using ContactBook.Core.DTO;
using static ContactBook.Infrastructure.Helper.PaginParameter;

namespace ContactBook.Core.Services
{
    public interface IAppUserRepository 
    {
        Task<string> AddUser(AppUserDTO appUser);
        Task<List<AppUserDTO>> SearchForTerm(string term);

        Task<AppUserDTO> GetAppUserById(string id);
        Task<List<AppUserDTO>> GetAllUser(PaginParameter userParameter);

        /*Task<string> UpdateAppUser(AppUser appUser);*/
        Task<bool> AddPhoto(string id, PhotoDTO photo);
    }
}
