using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace CarRental.Web.Areas.Administration.Controllers
{
    public class OrdersController : AdministrationController
    {
        private readonly IOrdersService ordersService;
        private readonly IMapper mapper;

        public OrdersController(IOrdersService ordersService, IMapper mapper)
        {
            this.ordersService = ordersService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var isCanceled = await this.ordersService.Cancel(id);

            if (!isCanceled)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Finish(string id)
        {
            var isFinished = await this.ordersService.Finish(id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var isDeleted = await this.ordersService.Delete(id);

            if (!isDeleted)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var order = await this.ordersService.GetOrderById(id);

            if (order is null)
            {
                return RedirectToAction("All", "Orders");
            }

            var viewModel = this.mapper.Map<OrderEditViewModel>(order);
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderEditViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("All", "Orders");
            }

            await this.ordersService.EditOrder(inputModel.Id, inputModel.Firstname, inputModel.Lastname,
                                                                         inputModel.Email, inputModel.Price);
            return RedirectToAction("All", "Orders");
        }

        public async Task<IActionResult> All()
        {
            var orders = await this.ordersService.All().ToListAsync();
            var viewModels = this.mapper.Map<List<MyOrdersViewModel>>(orders);
            return this.View(viewModels);
        }
    }
}