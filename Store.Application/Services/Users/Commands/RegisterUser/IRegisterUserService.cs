using Microsoft.AspNet.Identity;
using Store.Application.Interface.Context;
using Store.Common.Dto;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.RegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        public RegisterUserService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "پست الکترونیکی را وارد کنید"
                    };
                }
                if(string.IsNullOrWhiteSpace(request.FullName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد کنید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور را وارد کنید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.RePassword) || request.Password != request.RePassword)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور را وارد کنید"
                    };
                }

                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
                var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
                if(!match.Success)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمبل را بدرستی وارد کنید",
                    };
                }

                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password);

                User user = new User()
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    Password = hashedPassword,
                    IsActive = true,
                };

                List<UserRole> userRoles = new List<UserRole>();
                foreach (var item in request.roles)
                {
                    var roles = _context.Roles.Find(item.Id);
                    userRoles.Add(new UserRole
                    {
                        Role = roles,
                        RoleId = roles.Id,
                        User = user,
                        UserId = user.Id,
                    });
                }
                user.UserRoles = userRoles;

                _context.Users.Add(user);
                _context.SaveChanges();


                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = user.Id
                    },
                    IsSuccess = true,
                    Message = "ثبت نام با موفقیت انجام شد!"
                };
            }
            catch(Exception)
            {
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد!"
                };
            }      
        }
    }

    public class RequestRegisterUserDto{
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<RolesRegisterUserDto> roles { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    public class RolesRegisterUserDto
    {
        public long Id { get; set; }
    }

    public class ResultRegisterUserDto
    {
        public long UserId { get; set; }
       
    }
}
