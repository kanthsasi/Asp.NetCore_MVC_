using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Application.Services.Interface;
using TopSpeed.Domain.ApplicationEnums;
using TopSpeed.Domain.Models;
using TopSpeed.Domain.ViewModel;
using TopSpeed.Infrastructure.Repositories;

namespace TopSpeed.Web.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = CustomRole.MasterAdmin + "," + CustomRole.Admin)]

    public class CarDetailsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUserNameService userNameService;

        public CarDetailsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment,IUserNameService userNameService)
        {
            this.unitOfWork = unitOfWork;

            this.webHostEnvironment = webHostEnvironment;

            this.userNameService = userNameService;
        }

        public async Task<IActionResult> Index()
        {
            List<CarDetails> carDetails = await unitOfWork.CarDetails.GetAllCarDetails();

            return View(carDetails);
        }

        [HttpGet]

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> brandList = unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
              Text = x.Name.ToUpper(),
              Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicalTypeList = unitOfWork.VehicalType.Query().Select(x => new SelectListItem 
            { 
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndfuelType = Enum.GetValues(typeof(EngineAndFuelType)).Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem 
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission)).Cast<Transmission>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            CarDetailsVM carDetailsVM = new CarDetailsVM
            {
                CarDetails = new CarDetails(),
                BrandList = brandList,
                VehicleTypeList = vehicalTypeList,
                EngineAndFuelTypeList = engineAndfuelType,
                TransmissionList = transmission
            };

            return View(carDetailsVM);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CarDetailsVM carDetailsVM)
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

                carDetailsVM.CarDetails.VehicalImage = @"\Images\brand\" + newFileName + extension;
            }
            if (ModelState.IsValid)
            {
                await unitOfWork.CarDetails.Create(carDetailsVM.CarDetails);

                await unitOfWork.Save();

                TempData["success"] = CommonMeassage.RecordCreated;

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {
            var carDetails = await unitOfWork.CarDetails.GetCarDetailsById(id);

            carDetails.CreatedBy = await userNameService.GetUserName(carDetails.CreatedBy);

            carDetails.ModifiedBy = await userNameService.GetUserName(carDetails.ModifiedBy);

            return View(carDetails);

        }

        [HttpGet]

        public async Task<IActionResult> Edit(Guid id)
        {
            var carDetails = await unitOfWork.CarDetails.GetByIdAsync(id);

            IEnumerable<SelectListItem> brandList = unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicleTypeList = unitOfWork.VehicalType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission))
             .Cast<Transmission>()
             .Select(x => new SelectListItem
             {
                 Text = x.ToString().ToUpper(),
                 Value = ((int)x).ToString()
             });

            CarDetailsVM carDetailsVM = new CarDetailsVM
            {
                CarDetails = carDetails,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(carDetailsVM);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(Guid id,CarDetailsVM carDetailsVM)
        {
            var webRootPath = webHostEnvironment.WebRootPath;

            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();

                var upload = Path.Combine(webRootPath, @"Images\brand");

                var extension = Path.GetExtension(file[0].FileName);

                //delete old image
                var objFromDb = await unitOfWork.CarDetails.GetByIdAsync(id);

                if (objFromDb.VehicalImage != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.VehicalImage.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }

                carDetailsVM.CarDetails.VehicalImage = @"\Images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                var objFromDb = await unitOfWork.CarDetails.Update(id,carDetailsVM.CarDetails);

                await unitOfWork.Save();

                TempData["warning"] = CommonMeassage.RecordUpdated;

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Delete(Guid id)
        {
            var carDetails = await unitOfWork.CarDetails.GetByIdAsync(id);

            IEnumerable<SelectListItem> brandList = unitOfWork.Brand.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> vehicleTypeList = unitOfWork.VehicalType.Query().Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = x.Id.ToString()
            });

            IEnumerable<SelectListItem> engineAndFuelType = Enum.GetValues(typeof(EngineAndFuelType))
                .Cast<EngineAndFuelType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString().ToUpper(),
                    Value = ((int)x).ToString()
                });

            IEnumerable<SelectListItem> transmission = Enum.GetValues(typeof(Transmission))
             .Cast<Transmission>()
             .Select(x => new SelectListItem
             {
                 Text = x.ToString().ToUpper(),
                 Value = ((int)x).ToString()
             });

            CarDetailsVM carDetailsVM = new CarDetailsVM
            {
                CarDetails = carDetails,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                EngineAndFuelTypeList = engineAndFuelType,
                TransmissionList = transmission
            };

            return View(carDetailsVM);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(CarDetailsVM carDetailsVM, Guid id)
        {
            string webRootPath = webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(carDetailsVM.CarDetails.VehicalImage))
            {
                //delete old image
                var objFromDb = await unitOfWork.CarDetails.GetByIdAsync(id);

                if (objFromDb.VehicalImage != null)
                {
                    var oldImagePath = Path.Combine(webRootPath, objFromDb.VehicalImage.Trim('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
            }
            await unitOfWork.CarDetails.Delete(carDetailsVM.CarDetails);

            await unitOfWork.Save();

            TempData["error"] = CommonMeassage.RecordDeleted;

            return RedirectToAction(nameof(Index));
        }
    }
}
