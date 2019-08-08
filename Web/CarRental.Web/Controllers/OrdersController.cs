using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.InputModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRental.Web.ViewModels.Orders;
using CarRental.Web.ViewModels.Vouchers;
using CarRental.Web.Hubs;
using Microsoft.AspNetCore.SignalR;
using CarRental.Common;

namespace CarRental.Web.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrdersService ordersService;
        private readonly IMapper mapper;
        private readonly IVouchersService vouchersService;
        private readonly IHubContext<NotifyHub> notifyHub;
        private readonly ICarsService carsService;

        public OrdersController(IOrdersService ordersService, IMapper mapper, 
                                    IVouchersService vouchersService, IHubContext<NotifyHub> notifyHub, ICarsService carsService)
        {
            this.ordersService = ordersService;
            this.mapper = mapper;
            this.vouchersService = vouchersService;
            this.notifyHub = notifyHub;
            this.carsService = carsService;
        }
       
        [Authorize]
        public IActionResult MyOrders()
        {
            var userEmail = this.User.Identity.Name;
            var orders = this.ordersService.GetAllOrdersForUser(userEmail);
            var viewModels = this.mapper.Map<List<MyOrdersViewModel>>(orders);

            return this.View(viewModels);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Preview(OrderPreviewInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("/");
            }

            inputModel.DiscountPercent = await this.vouchersService.GetDiscountForCode(inputModel.DiscountCode);
            var vouchers = this.mapper.Map<List<VoucherViewModel>>(this.vouchersService.GetAllActiveForUser(this.User.Identity.Name));
            inputModel.Vouchers = vouchers;

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Order(OrderInputViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await this.ordersService.MakeOrder(this.User.Identity.Name, inputModel.Id, inputModel.PickUpPlace, inputModel.ReturnPlace,
                    inputModel.Price, inputModel.PickUp, inputModel.Return, inputModel.DiscountCode);

            if (!result)
            {
                return RedirectToAction(nameof(Invalid));
            }

            var carModel = this.carsService.GetCarModelById(inputModel.Id);
            var days = (inputModel.Return - inputModel.PickUp).Days;

            var message = string.Format(GlobalConstants.SignalRMessageForNewOrder, carModel, days);
            await this.notifyHub.Clients.All.SendAsync(GlobalConstants.SignalRMethodNewOrder, message);

            return RedirectToAction(nameof(MyOrders));
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var order = await this.ordersService.GetOrderById(id);

            if (order is null)
            {
                return RedirectToAction(nameof(MyOrders));
            }

            var viewModel = this.mapper.Map<OrderDetailsViewModel>(order);
            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Invalid()
        {
            return this.View();
        }
    }
}
