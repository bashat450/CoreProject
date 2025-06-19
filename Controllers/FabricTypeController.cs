
using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AdminDyanamoEnterprises.Controllers
{
    public class FabricTypeController : Controller
    {
        private readonly IFabricRepository _ifabricrepository;
        private readonly INotyfService _notyf;

        public FabricTypeController(IFabricRepository ifabricrepository, INotyfService notyf)
        {
            _ifabricrepository = ifabricrepository;
            _notyf = notyf;
        }

        // GET: FabricType
        public ActionResult Index()
        {
            return View();
        }

        // GET: Masterfabric typeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FabricType/fabricType
        public ActionResult FabricType()
        {
            FabricTypePageViewModel model = new FabricTypePageViewModel
            {
                AddFabric = new AddFabricType(), // Empty form
                FabricList = _ifabricrepository.GetAllListType() // ✅ Correct usage
            };
            return View(model);
        }

        // POST: FabricType/FabricType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FabricType(FabricTypePageViewModel addFabricType)
        {
            try
            {
                string result = _ifabricrepository.Sp_InsertOrUpdateOrDeleteFabric(addFabricType);
                TempData["Result"] = result;
                return RedirectToAction("FabricType");
            }
            catch
            {
                _notyf.Error("An error occurred while saving.");
                return View(addFabricType);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _ifabricrepository.DeleteFabric(id);
                _notyf.Success("Fabric deleted successfully!");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Optionally log error
                return Json(new { success = false, error = ex.Message });
            }
        }


        public ActionResult MasterFabricType()
        {

            return View();

        }
    }
}
