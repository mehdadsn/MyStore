using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.UserLogin
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserLoginDto> Execute(string username, string password);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly IDataBaseContext _context;
        public UserLoginService(IDataBaseContext context)
        {
            _context = context;
        }
        

        public ResultDto<ResultUserLoginDto> Execute(string email, string password)
        {
            if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data = new ResultUserLoginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "نام کاربری و رمز عبور را وارد کنید"
                };
            }
            var passwordHasher = new PasswordHasher();

            var user = _context.Users.Include(p => p.UserRoles)
                .ThenInclude(p => p.Role)
                .Where(p => p.Email.Equals(email) 
                && p.IsActive == true)
                .FirstOrDefault();
            bool correctPassword = false;
            if (user != null)
            {
                correctPassword = (passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Success);
            }
             
            if (user == null || !correctPassword)
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data = new ResultUserLoginDto()
                    {

                    }, 
                    IsSuccess=false,
                    Message = "کاربری با این ایمیل و رمز عبور یافت نشد!"
                };
            }

            var roles = "";
            foreach(var item in user.UserRoles)
            {
                roles += $"{item.Role.Name}";
            }

            return new ResultDto<ResultUserLoginDto>()
            {
                Data = new ResultUserLoginDto()
                {
                    Roles = roles,
                    UserId = user.Id,
                    Name = user.FullName
                },
                IsSuccess = true,
                Message = "با موفقیت وارد شدید!"
            };
        }
    }

    public class ResultUserLoginDto 
    { 
        public string Roles { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
    }
}
