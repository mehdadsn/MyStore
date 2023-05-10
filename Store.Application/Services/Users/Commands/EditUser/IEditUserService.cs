using Store.Application.Interface.Context;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Users.Commands.EditUser
{
    public interface IEditUserService
    {
        public ResultDto Execute(RequestEditUserDto request);
    }

    public class EditUserService : IEditUserService
    {
        private readonly IDataBaseContext _context;
        public EditUserService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEditUserDto request)
        {
            var user = _context.Users.Find(request.UserId);
            if(user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.FullName = request.FullName;
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "کاربر با موفقیت ویرایش شد"
            };
        }
    }

    public class RequestEditUserDto
    {
        public long UserId { get; set; }
        public string FullName { get; set;}
    }
}
