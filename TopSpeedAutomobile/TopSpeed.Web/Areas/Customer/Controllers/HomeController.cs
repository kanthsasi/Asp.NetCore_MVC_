using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using TopSpeed.Application.Contracts.Presistance;
using TopSpeed.Application.ExtensionsMethods;
using TopSpeed.Domain.Models;
using TopSpeed.Domain.ViewModel;
using TopSpeed.Infrastructure.Repositories;

namespace TopSpeed.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, bool resetFilter = false)
        {
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

            List<CarDetails> carDetails;

            if (resetFilter)
            {
                TempData.Remove("FilteredPosts");
                TempData.Remove("SelectedBrandId");
                TempData.Remove("SelectedVehicleTypeId");
            }

            if (TempData.ContainsKey("FilteredPosts"))
            {
                carDetails = TempData.Get<List<CarDetails>>("FilteredPosts");
                TempData.Keep("FilteredPosts");
            }
            else
            {
                carDetails = await unitOfWork.CarDetails.GetAllCarDetails();
            }

            int pageSize = 3;

            int pageNumber = page ?? 1;

            int totalItems = carDetails.Count;

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageNumber;

            var pagedPosts = carDetails.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Path);

            HomePostVM homePostVM = new HomePostVM
            {
                carDetails = pagedPosts,
                BrandList = brandList,
                VehicleTypeList = vehicleTypeList,
                BrandId = (Guid?)TempData["SelectedBrandId"],
                VehicleTypeId = (Guid?)TempData["SelectedVehicleTypeId"]
            };

            return View(homePostVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomePostVM homePostVM)
        {
            var posts = await unitOfWork.CarDetails.GetAllCarDetails(homePostVM.searchBox, homePostVM.BrandId, homePostVM.VehicleTypeId);

            TempData.Put("FilteredPosts", posts);
            TempData["SelectedBrandId"] = homePostVM.BrandId;
            TempData["SelectedVehicleTypeId"] = homePostVM.VehicleTypeId;

            return RedirectToAction("Index", new { page = 1, resetFilter = false });
        }

        [Authorize]

        public async Task<IActionResult> Details(Guid id, int? page)
        {
            CarDetails car = await unitOfWork.CarDetails.GetCarDetailsById(id);

            List<CarDetails> cars = new List<CarDetails>();

            if (car != null)
            {
                cars = await unitOfWork.CarDetails.GetAllCarDetails(car.Id, car.BrandId);
            }

            ViewBag.CurrentPage = page;

            CustomerDetailsVM customerDetailsVM = new CustomerDetailsVM
            {
                carDetails = car,
                carDetail = cars
            };

            return View(customerDetailsVM);
        }

        public IActionResult GoBack(int? page)
        {
            string? previousUrl = HttpContext.Session.GetString("PreviousUrl");

            if (!string.IsNullOrEmpty(previousUrl))
            {
                // Append the page number to the previous URL if it exists
                if (page.HasValue)
                {
                    previousUrl = QueryHelpers.AddQueryString(previousUrl, "page", page.Value.ToString());
                }

                HttpContext.Session.Remove("PreviousUrl"); // Remove the session variable

                return Redirect(previousUrl);
            }
            else
            {
                // Handle the case when there is no previous URL stored in the session
                // You can redirect to a default page or take some other action
                return RedirectToAction("Index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
