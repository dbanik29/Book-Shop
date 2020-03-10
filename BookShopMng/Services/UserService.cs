using BookShopMng.Helpers;
using BookShopMng.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookShopMng.Services
{
    public interface IUserService
    {
        UsersInformation Authentication(string username, string password);
        Task<List<UsersInformation>> GetUsersInfo();
        Task<int> AddUser(UsersInformation user);
        Task<int> DeleteUser(int? UserId);
        Task UpdateUser(UsersInformation user);
    }
    public class UserService : IUserService
    {
        private readonly BookShopDbContext _context;
        private readonly IConfiguration _Configuration;
        public UserService(IConfiguration Configuration, BookShopDbContext _bookShopDbContext)
        {

            _context = _bookShopDbContext;
            _Configuration = Configuration;
        }
        public UsersInformation Authentication(string username, string password)
        {
            var useradmin = _context.UserInfos.SingleOrDefault(x => x.UserName == username && x.Password == password);
            if (useradmin == null)
                return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettingsSection = _Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role,useradmin.Role),
                    new Claim(ClaimTypes.Name,useradmin.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            useradmin.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            useradmin.Password = null;
            return useradmin;
        }
        public async Task<List<UsersInformation>> GetUsersInfo()
        {
            if (_context != null)
            {
                return await _context.UserInfos.ToListAsync();
            }
            return null;
        }
        public async Task<int> AddUser(UsersInformation user)
        {
            if (_context != null)
            {
                await _context.UserInfos.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteUser(int? UserId)
        {
            int result = 0;
            if (_context != null)
            {
                var user = await _context.UserInfos.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (user != null)
                {
                    _context.UserInfos.Remove(user);
                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
        public async Task UpdateUser(UsersInformation user)
        {
            if (_context != null)
            {
                _context.UserInfos.Update(user);
                await _context.SaveChangesAsync();
            }
        }

    }
}
