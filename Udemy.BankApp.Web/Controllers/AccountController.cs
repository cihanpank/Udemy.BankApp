using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mapping;
using Udemy.BankApp.Web.Models;

namespace Udemy.BankApp.Web.Controllers
{
    public class AccountController : Controller
    {

        //private readonly IApplicationUserRepository _applicationUserRepository;
        //private readonly IAccountRepository _accountRepository;
        //private readonly IUserMapper _userMapper;
        //private readonly IAccountMapper _accountMapper;

        //public AccountController(, IApplicationUserRepository applicationUserRepository, IUserMapper userMapper, IAccountRepository accountRepository, IAccountMapper accountMapper)
        //{

        //    _applicationUserRepository = applicationUserRepository;
        //    _userMapper = userMapper;
        //    _accountRepository = accountRepository;
        //    _accountMapper = accountMapper;
        //}

        //private readonly IRepository<Account> _accountRepository;
        //private readonly IRepository<ApplicationUser> _userRepository;

        //public AccountController(IRepository<Account> accountRepository, IRepository<ApplicationUser> userRepository)
        //{
        //    _accountRepository = accountRepository;
        //    _userRepository = userRepository;
        //}

        private readonly IUow _uow;

        public AccountController(IUow uow)
        {
            _uow = uow;
        }

        public IActionResult Create(int id)
        {
            var user = _uow.GetRepository<ApplicationUser>().GetById(id);

            return View(new UserListModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname
            });
        }

        [HttpPost]
        public IActionResult Create(AccountCreateModel model)
        {
            _uow.GetRepository<Account>().Create(new Account
            {
                Balance = model.Balance,
                AccountNumber = model.AccountNumber,
                ApplicationUserId = model.ApplicationUserId
            });
            _uow.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        public IActionResult GetByUserId(int userId)
        {
            var query = _uow.GetRepository<Account>().GetQueryable();
            var accounts = query.Where(x => x.ApplicationUserId == userId).ToList();

            var user = _uow.GetRepository<ApplicationUser>().GetById(userId);
            ViewBag.FullName = user.Name + " " + user.Surname;
            var list = new List<AccountListModel>();
            foreach (var account in accounts)
            {
                list.Add(new AccountListModel
                {
                    ApplicationUserId = account.ApplicationUserId,
                    Balance = account.Balance,
                    AccountNumber = account.AccountNumber,
                    Id = account.Id,


                });
            }

            return View(list);

        }

        [HttpGet]
        public IActionResult SendMoney(int accountId)
        {
            var query = _uow.GetRepository<Account>().GetQueryable();
            var accounts = query.Where(x => x.Id != accountId);

            var list = new List<AccountListModel>();

            ViewBag.SenderId= accountId;

            foreach (var account in accounts)
            {
                list.Add(new()
                {
                    Id= account.Id,
                    Balance = account.Balance,
                    AccountNumber = account.AccountNumber,
                    ApplicationUserId= account.ApplicationUserId

                });
            }
            return View(new SelectList(list,"Id","AccountNumber"));
        
        }

        [HttpPost]
        public IActionResult SendMoney(SendMoneyModel model)
        {
            var senderAccount=_uow.GetRepository<Account>().GetById(model.SenderId);
            senderAccount.Balance -= model.Amount;

            _uow.GetRepository<Account>().Update(senderAccount);

           var account= _uow.GetRepository<Account>().GetById(model.AccountId);
            account.Balance+= model.Amount;
            _uow.GetRepository<Account>().Update(account);

            _uow.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}
