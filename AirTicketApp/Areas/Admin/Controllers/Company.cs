using AirTicketApp.Data.Common.MessageConstants;
using AirTicketApp.Models.CompanyModels;
using AirTicketApp.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirTicketApp.Areas.Admin.Controllers
{
    [Authorize]
    public class Company : BaseController
    {
        private readonly ICompanyService companyService;
        public Company(ICompanyService _companyService)
        {
            this.companyService = _companyService;
        }
        public async Task<IActionResult> All()
        {
            var model = new List<CompanyViewModel>();
            try
            {
                model = await companyService.GetAllCompanies();

                return View(model);
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "System error!";
                return ((IActionResult)model);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var model = await companyService.GetCompanyById(Id);

                return View(model);
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return View(new CompanyViewModel());
            }
            catch
            {
                TempData[MessageConstant.ErrorMessage] = "System error";
                return View(new CompanyViewModel());
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return View(model);
            }

            try
            {
                await companyService.Edit(model);
                TempData[MessageConstant.SuccessMessage] = $"Successfully edited {model.Name}!";
                return RedirectToAction("All", "Company", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = "System error!";
                return RedirectToAction("All", "Company", new { area = "Admin" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {

            return View(new CompanyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.WarningMessage] = "Not all validations are passed!";
                return View(model);
            }

            try
            {
                await companyService.Add(model);
                TempData[MessageConstant.SuccessMessage] = $"Successfully added {model.Name}!";
                return RedirectToAction("All", "Company", new { area = "Admin" });
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = "System error!";
                return RedirectToAction("All", "Company", new { area = "Admin" });
            }

        }

    }
}
