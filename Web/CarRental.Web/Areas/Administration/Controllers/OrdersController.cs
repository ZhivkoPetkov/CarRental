using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CarRental.Services.Contracts;
using CarRental.Web.ViewModels.Orders;
using Microsoft.AspNetCore.Mvc;

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
           await this.ordersService.Cancel(id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Finish(string id)
        {
           await this.ordersService.Finish(id);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.ordersService.Delete(id);
            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(string id)
        {
            var order = this.ordersService.GetOrderById(id);
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

        public IActionResult All()
        {
            var orders = this.ordersService.All();
            var viewModels = this.mapper.Map<List<MyOrdersViewModel>>(orders);
            return this.View(viewModels);
        }

    }
}