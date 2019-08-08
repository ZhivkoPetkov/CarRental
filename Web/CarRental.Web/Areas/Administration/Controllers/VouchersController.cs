using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.Areas.Administration.ViewModels.Vouchers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

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

        public async Task<IActionResult> Generate()
        {
            var users = await this.usersService.GetAllUsers().ToListAsync();
            var viewModels = this.mapper.Map<List<UserVoucherViewModel>>(users);
            return this.View(viewModels);
        }

        public async Task<IActionResult> All()
        {
            var vouchers = await this.vouchersService.GetAllVouchers().ToListAsync();
            var viewModels = this.mapper.Map<List<VoucherDetailsViewModel>>(vouchers);
            return this.View(viewModels);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.vouchersService.DeleteVoucher(id);

            if (!isDeleted)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Generate(UserVoucherViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Generate));
            }

            var isGenerated = await this.vouchersService.CreateForUserCustom(inputModel.Email, inputModel.Discount);

            if (!isGenerated)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Generate));
        }
    }
}
