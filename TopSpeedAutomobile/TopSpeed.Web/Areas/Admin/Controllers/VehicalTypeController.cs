using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Models;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]

    public class VehicalTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public VehicalTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            List<VehicalType> vehicalTypes = await unitOfWork.VehicalType.GetAllAsync();

            return View(vehicalTypes);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(VehicalType vehical)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.VehicalType.Create(vehical);

                await unitOfWork.Save();

                TempData["success"] = CommonMeassage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            var vehicalType = await unitOfWork.VehicalType.GetByIdAsync(id);

            return View(vehicalType);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var vehicalType = await unitOfWork.VehicalType.GetByIdAsync(id);

            return View(vehicalType);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Guid id,VehicalType vehical)
        {                         
            if (ModelState.IsValid)
            {
                var objFromDb = await unitOfWork.VehicalType.Update(id,vehical);

                await unitOfWork.Save();

                TempData["warning"] = CommonMeassage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            var vehicalType = await unitOfWork.VehicalType.GetByIdAsync(id);

            return View(vehicalType);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(VehicalType vehical, Guid id)
        {           
            await unitOfWork.VehicalType.Delete(vehical);

            TempData["error"] = CommonMeassage.RecordDeleted;

            return RedirectToAction(nameof(Index));
        }
    }
}
