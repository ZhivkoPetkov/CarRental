using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Vouchers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class VouchersController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IMapper mapper;
        private readonly IVouchersService vouchersService;

        public VouchersController(IUsersService usersService, IMapper mapper, IVouchersService vouchersService)
        {
            this.usersService = usersService;
            this.mapper = mapper;
            this.vouchersService = vouchersService;
        }

        public IActionResult Generate()
        {
            var users = this.usersService.GetAllUsers();
            var viewModels = this.mapper.Map<List<UserVoucherViewModel>>(users);
            return this.View(viewModels);
        }

        public IActionResult All()
        {
            var vouchers = this.vouchersService.GetAllVouchers();
            var viewModels = this.mapper.Map<List<VoucherDetailsViewModel>>(vouchers);
            return this.View(viewModels);
        }

        public IActionResult Delete(int id)
        {
            var result = this.vouchersService.DeleteVoucher(id);

            if (!result)
            {
                return BadRequest("Invalid ID for deletion");
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult Generate(UserVoucherViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Generate));
            }

            var result = this.vouchersService.CreateForUserCustom(inputModel.Email, inputModel.Discount);

            if (!result)
            {
                return RedirectToAction(nameof(Generate));
            }

            return RedirectToAction(nameof(Generate));
        }
    }
}
