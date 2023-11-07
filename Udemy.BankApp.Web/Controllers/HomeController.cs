using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mapping;
using Udemy.BankApp.Web.Models;

namespace Udemy.BankApp.Web.Controllers
{
    public class HomeController : Controller
    {

        //private readonly IApplicationUserRepository _applicationUserRepository;
        //private readonly IUserMapper _userMapper;

        //public HomeController( IApplicationUserRepository applicationUserRepository, IUserMapper userMapper)
        //{

        //    _applicationUserRepository = applicationUserRepository;
        //   _userMapper = userMapper;
        //}
        //private readonly IRepository<ApplicationUser> _userRepository;
        //private readonly IRepository<Account> _accountRepository;

        //public HomeController(IRepository<ApplicationUser> userRepository, IRepository<Account> accountRepository)
        //{
        //    _userRepository = userRepository;
        //    _accountRepository = accountRepository;
        //}

        private readonly IUow _uow;

        public HomeController(IUow uow)
        {
            _uow = uow;
        }

        public IActionResult Index()
        {
            
            return View(_uow.GetRepository<ApplicationUser>().GetAll().Select(x=> new UserListModel
            {
                Id= x.Id,
                Name= x.Name, 
                  Surname= x.Surname
            }).ToList());
        }

       


    }
}