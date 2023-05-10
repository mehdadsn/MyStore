using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Services.Users.Commands.EditUser;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Commands.UserStatusChange;
using Store.Application.Services.Users.Queries.GetRows;
using Store.Application.Services.Users.Queries.GetUsers;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUsersService;
        private readonly IGetRolesService _getRolesService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IRemoveUserService _removeUserService;
        private readonly IUserStatusChangeService _userStatusChangeService;
        private readonly IEditUserService _editUserService;
        public UsersController(IGetUsersService getUsersService,
            IGetRolesService getRolesService,
            IRegisterUserService registerUserService,
            IRemoveUserService removeUserService,
            IUserStatusChangeService userStatusChangeService,
            IEditUserService editUserService)
        {
            _getUsersService = getUsersService;
            _getRolesService = getRolesService;
            _registerUserService = registerUserService;
            _removeUserService = removeUserService;
            _userStatusChangeService = userStatusChangeService;
            _editUserService = editUserService;
        }

        public IActionResult Index(string searchKey, int page=1)
        {
            return View(_getUsersService.Execute(new RequestGetUserDto { Page = page, SearchKey = searchKey}));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles =new SelectList(_getRolesService.Execute().Data, "Id", "Name" );
            return View();
        }
        [HttpPost]
        public IActionResult Craete(string email, string fullName, long roleId, string password, string rePassword) {
            var result = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = email,
                FullName = fullName,
                roles = new List<RolesRegisterUserDto>()
                {
                    new RolesRegisterUserDto
                    {
                        Id = roleId
                    }
                },
                Password = password,
                RePassword = rePassword,
            });
            return Json(result);
        }

        [HttpPost]
        public IActionResult Delete(long UserId)
        {
            return Json(_removeUserService.Execute(UserId));
        }
        [HttpPost]
        public IActionResult UserStatusChange(long UserId)
        {
            return Json(_userStatusChangeService.Execute(UserId));
        }

        [HttpPost]
        public IActionResult Edit(long Userid, string Fullname)
        {
            return Json(_editUserService.Execute(new RequestEditUserDto
            {
                FullName = Fullname,
                UserId = Userid,
            }));
        }
    }
}
