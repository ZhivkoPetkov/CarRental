using AutoMapper;
using CarRental.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CarRental.Web.ViewModels.Vouchers;

namespace CarRental.Web.Controllers
{
    public class VouchersController : BaseController
    {
        private readonly IVouchersService vouchersService;
        private readonly IMapper mapper;

        public VouchersController(IVouchersService vouchersService, IMapper mapper)
        {
            this.vouchersService = vouchersService;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult MyVouchers()
        {
            var vouchers = this.vouchersService.GetAllForUser(this.User.Identity.Name);
            var viewModels = this.mapper.Map<List<VoucherViewModel>>(vouchers);
            return this.View(viewModels);
        }

    }
}
