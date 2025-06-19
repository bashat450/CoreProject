using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AdminDyanamoEnterprises.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterRepository _imasterrepository;
        private readonly INotyfService _notyf;

        public MasterController(IMasterRepository imasterrepository, INotyfService notyf)
        {
            _imasterrepository = imasterrepository;
            _notyf = notyf;
        }
        // GET: MasterController
        public ActionResult Index()
        {

            return View();
        }

        // GET: MasterController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MasterController/Category
        public ActionResult CategoryType()
        {
            CategoryTypePageViewModel model = new CategoryTypePageViewModel
            {
                AddCategory = new AddCategoryType(), // Empty form
                CategoryList = _imasterrepository.GetAllListType() // From database or service
            };
            return View(model);
            /* return View(model);*/
        }

        // POST: MasterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryType(CategoryTypePageViewModel addCategoryType)
        {
            try
            {
                _imasterrepository.InsertorUpdateCategoryType(addCategoryType);
                _notyf.Success("Success ");
                return RedirectToAction("CategoryType");
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            _imasterrepository.DeleteCategory(id);
            return Json(new { success = true });
        }
        
        public ActionResult MasterCategoryType()
        {
           
            return View();
           
        }
    }
}
