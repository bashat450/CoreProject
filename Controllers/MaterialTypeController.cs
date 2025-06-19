using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AdminDyanamoEnterprises.Controllers
{
    public class MaterialTypeController : Controller
    {
        private readonly IMaterialRepository _imaterialrepository;
        private readonly INotyfService _notyf;

        public MaterialTypeController(IMaterialRepository imaterialrepository, INotyfService notyf)
        {
            _imaterialrepository = imaterialrepository;
            _notyf = notyf;
        }

        // GET: MaterialType
        public ActionResult Index()
        {
            return View();
        }

        // GET: MasterMaterial typeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MaterialType/MaterialType
        public ActionResult MaterialType()
        {
            MaterialTypePageViewModel model = new MaterialTypePageViewModel
            {
                AddMaterial = new AddMaterialType(), // Empty form
                MaterialList = _imaterialrepository.GetAllListType() // ✅ Correct usage
            };
            return View(model);
        }

        // POST: MaterialType/MaterialType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MaterialType(MaterialTypePageViewModel addMaterialType)
        {
            try
            {
                string result = _imaterialrepository.Sp_InsertOrUpdateOrDeleteMaterialType(addMaterialType);
                TempData["Result"] = result;
                return RedirectToAction("MaterialType");
            }
            catch
            {
                _notyf.Error("An error occurred while saving.");
                return View(addMaterialType);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _imaterialrepository.DeleteMaterial(id); // ✅ Correct usage
            _notyf.Success("Material deleted successfully!");
            return Json(new { success = true });
        }

        public ActionResult MasterMaterialType()
        {

            return View();

        }
    }
}
