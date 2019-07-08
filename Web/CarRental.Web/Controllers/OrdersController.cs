using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.InputModels.Orders;
using CarRental.Web.ViewModels.Orders;
using CarRental.Web.ViewModels.Vouchers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarRental.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IMapper mapper;
        private readonly IVouchersService vouchersService;

        public OrdersController(IOrdersService ordersService, IMapper mapper, IVouchersService vouchersService)
        {
            this.ordersService = ordersService;
            this.mapper = mapper;
            this.vouchersService = vouchersService;
        }
       
        [Authorize]
        public IActionResult MyOrders()
        {
            var userEmail = this.User.Identity.Name;
            var orders = this.ordersService.GetAllOrdersForUser(userEmail);
            if (orders.Count == 0)
            {
                this.View();
            }

            var viewModels = this.mapper.Map<List<MyOrdersViewModel>>(orders);
            return this.View(viewModels);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Preview(OrderPreviewInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }

            inputModel.DiscountPercent = this.vouchersService.GetDiscountForCode(inputModel.DiscountCode);
            var vouchers = this.mapper.Map<List<VoucherViewModel>>(this.vouchersService.GetAllActiveForUser(this.User.Identity.Name));
            inputModel.Vouchers = vouchers;
            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order(OrderInputViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }

            var result = this.ordersService.MakeOrder(this.User.Identity.Name, inputModel.Id, inputModel.PickUpPlace, inputModel.ReturnPlace,
                    inputModel.Price, inputModel.PickUp, inputModel.Return, inputModel.DiscountCode);

            if (!result)
            {
                return Redirect("/");
            }
            return RedirectToAction(nameof(MyOrders));
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var order = this.ordersService.GetOrderById(id);
            var viewModel = this.mapper.Map<OrderDetailsViewModel>(order);
            return this.View(viewModel);
        }
    }
}
