using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Domain.Models;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]

    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWebHostEnvironment webHostEnvironment;

        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;

            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await unitOfWork.Brand.GetAllAsync();

            return View(brands);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Brand brand)
        {
            //To access the wwwroot folder and stored a path as a string.

            string webRootPath = webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"Images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\Images\brand\" + newFileName + extension;
            }
            if (ModelState.IsValid)
            {
                await unitOfWork.Brand.Create(brand);

                await unitOfWork.Save();

                TempData["success"] = CommonMeassage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            var brand = await unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var brand = await unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Brand brand, Guid id)
        {
            var webRootPath = webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"Images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old image
                var objFromDb = await unitOfWork.Brand.GetByIdAsync(id);

                if (objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                brand.BrandLogo = @"\Images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                var objFromDb = await unitOfWork.Brand.Update(brand, id);

                await unitOfWork.Save();

                TempData["warning"] = CommonMeassage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            var brand = await unitOfWork.Brand.GetByIdAsync(id);

            return View(brand);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(Brand brand, Guid id)
        {
            string webRootPath = webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {
                //delete old image
                var objFromDb = await unitOfWork.Brand.GetByIdAsync(id);

                if (objFromDb.BrandLogo != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.BrandLogo.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }
            await unitOfWork.Brand.Delete(brand);

            await unitOfWork.Save();

            TempData["error"] = CommonMeassage.RecordDeleted;

            return RedirectToAction(nameof(Index));
        }
    }
}
