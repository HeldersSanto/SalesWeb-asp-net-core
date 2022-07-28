using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWeb.Models;
using SalesWeb.Services;
using SalesWeb.Models.ViewModels;
using SalesWeb.Services.Exceptions;
using System.Diagnostics;

namespace SalesWeb.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _selerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService ss, DepartmentService ds)
        {
            _selerService = ss;
            _departmentService = ds;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _selerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                var department = await _departmentService.FindAllAsync();

                var viewModel = new SellerFormViewModel { Seller = seller, Departments = department };
                return View(viewModel);
            }
            await _selerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not Provided" });
            }

            var obj = await _selerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _selerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details (int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not Provided" });
            }

            var obj = await _selerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not Provided" });
            }

            var obj = await _selerService.FindByIdAsync(id.Value);

            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not Found" });
            }

            List<Department> department = await _departmentService.FindAllAsync();

            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = department };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {

            if (!ModelState.IsValid)
            {
                var department = await _departmentService.FindAllAsync();

                SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = department };
                return View(viewModel);
            }
            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
            await _selerService.UpdateAsync(seller);
            return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}