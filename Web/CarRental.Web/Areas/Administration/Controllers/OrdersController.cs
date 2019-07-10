using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult Cancel(string id)
        {
           var result = this.ordersService.Cancel(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Finish(string id)
        {
            var result = this.ordersService.Finish(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(string id)
        {
            var result = this.ordersService.Delete(id);
            return RedirectToAction(nameof(All));
        }

        public IActionResult Edit(string id)
        {
            var order = this.ordersService.GetOrderById(id);
            var viewModel = this.mapper.Map<OrderEditViewModel>(order);
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(OrderEditViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("All", "Orders");
            }

            var result = this.ordersService.EditOrder(inputModel.Id, inputModel.Firstname, inputModel.Lastname, 
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