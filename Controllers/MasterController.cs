using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdminDyanamoEnterprises.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterRepository _imasterrepository;
        private readonly IColorRepository _icolorrepository;
        private readonly IFabricRepository _ifabricrepository;
        private readonly IMaterialRepository _imaterialrepository;
        private readonly INotyfService _notyf;

        public MasterController(
            IMasterRepository imasterrepository,
            IColorRepository icolorrepository,
            IFabricRepository ifabricrepository,
            IMaterialRepository imaterialrepository,
            INotyfService notyf)
        {
            _imasterrepository = imasterrepository;
            _icolorrepository = icolorrepository;
            _ifabricrepository = ifabricrepository;
            _imaterialrepository = imaterialrepository;
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

        #region ===***************============ Color Type ===************************============
        

        // GET: ColorType/ColorType
        public ActionResult ColorType()
        {
            ColorTypePageViewModel model = new ColorTypePageViewModel
            {
                AddColor = new AddColorType(), // Empty form
                ColorList = _icolorrepository.GetAllListColorType() // ✅ Correct usage
            };
            return View(model);
        }

        // POST: ColorType/FabricType
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColorType(ColorTypePageViewModel addColorType)
        {
            try
            {
                string result = _icolorrepository.Sp_InsertOrUpdateOrDeleteColor(addColorType);

                // Example: "Status: 0, Message: Insert successful., ActionType: insert"
                var statusMatch = Regex.Match(result, @"Status:\s*(\d+)");
                var messageMatch = Regex.Match(result, @"Message:\s*(.*?),");
                var actionMatch = Regex.Match(result, @"ActionType:\s*(\w+)");

                if (statusMatch.Success && messageMatch.Success && actionMatch.Success)
                {
                    TempData["Status"] = statusMatch.Groups[1].Value;
                    TempData["Message"] = messageMatch.Groups[1].Value;
                    TempData["ActionType"] = actionMatch.Groups[1].Value.ToLower();
                }
                else
                {
                    TempData["Status"] = "0";
                    TempData["Message"] = "Unexpected response format.";
                    TempData["ActionType"] = "error";
                }

                return RedirectToAction("ColorType");
            }
            catch
            {
                TempData["Status"] = "0";
                TempData["Message"] = "An error occurred while saving.";
                TempData["ActionType"] = "error";
                return RedirectToAction("ColorType");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteColor(int id)
        {
            try
            {
                _icolorrepository.DeleteColor(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion
        #region ================********** Fabric Type **************===============================
        // GET: FabricType/fabricType
        public ActionResult FabricType()
        {
            FabricTypePageViewModel model = new FabricTypePageViewModel
            {
                AddFabric = new AddFabricType(), // Empty form
                FabricList = _ifabricrepository.GetAllListFabricType() // ✅ Correct usage
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

                // Example: "Status: 0, Message: Insert successful., ActionType: insert"
                var statusMatch = Regex.Match(result, @"Status:\s*(\d+)");
                var messageMatch = Regex.Match(result, @"Message:\s*(.*?),");
                var actionMatch = Regex.Match(result, @"ActionType:\s*(\w+)");

                if (statusMatch.Success && messageMatch.Success && actionMatch.Success)
                {
                    TempData["Status"] = statusMatch.Groups[1].Value;
                    TempData["Message"] = messageMatch.Groups[1].Value;
                    TempData["ActionType"] = actionMatch.Groups[1].Value.ToLower();
                }
                else
                {
                    TempData["Status"] = "0";
                    TempData["Message"] = "Unexpected response format.";
                    TempData["ActionType"] = "error";
                }

                return RedirectToAction("FabricType");
            }
            catch
            {
                TempData["Status"] = "0";
                TempData["Message"] = "An error occurred while saving.";
                TempData["ActionType"] = "error";
                return RedirectToAction("FabricType");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFabric(int id)
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

        #endregion
        #region ===================================*****Material Type *************====================
        
        // GET: MaterialType/MaterialType
        public ActionResult MaterialType()
        {
            MaterialTypePageViewModel model = new MaterialTypePageViewModel
            {
                AddMaterial = new AddMaterialType(), // Empty form
                MaterialList = _imaterialrepository.GetAllListMaterialType() // ✅ Correct usage
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

                // Example: "Status: 0, Message: Insert successful., ActionType: insert"
                var statusMatch = Regex.Match(result, @"Status:\s*(\d+)");
                var messageMatch = Regex.Match(result, @"Message:\s*(.*?),");
                var actionMatch = Regex.Match(result, @"ActionType:\s*(\w+)");

                if (statusMatch.Success && messageMatch.Success && actionMatch.Success)
                {
                    TempData["Status"] = statusMatch.Groups[1].Value;
                    TempData["Message"] = messageMatch.Groups[1].Value;
                    TempData["ActionType"] = actionMatch.Groups[1].Value.ToLower();
                }
                else
                {
                    TempData["Status"] = "0";
                    TempData["Message"] = "Unexpected response format.";
                    TempData["ActionType"] = "error";
                }

                return RedirectToAction("MaterialType");
            }
            catch
            {
                TempData["Status"] = "0";
                TempData["Message"] = "An error occurred while saving.";
                TempData["ActionType"] = "error";
                return RedirectToAction("MaterialType");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMaterial(int id)
        {
            _imaterialrepository.DeleteMaterial(id); // ✅ Correct usage
            _notyf.Success("Material deleted successfully!");
            return Json(new { success = true });
        }
        public ActionResult MasterMaterialType()
        {

            return View();

        }
        #endregion
    }
}
