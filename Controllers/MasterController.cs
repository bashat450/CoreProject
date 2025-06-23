using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Common;
using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository;
using AdminDyanamoEnterprises.Repository;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using static AdminDyanamoEnterprises.Repository.IMasterRepository;

namespace AdminDyanamoEnterprises.Controllers
{
    public class MasterController : Controller
    {
        private readonly IColorRepository _icolorrepository;
        private readonly IFabricRepository _ifabricrepository;
        private readonly IMaterialRepository _imaterialrepository;
        private readonly IMasterRepository _imasterrepository;
        private readonly INotyfService _notyf;

        public MasterController(
            IMasterRepository imasterrepository,
            IColorRepository icolorrepository,
            IFabricRepository ifabricrepository,
            IMaterialRepository imaterialrepository,
            INotyfService notyf
            )
        {
            _imasterrepository = imasterrepository;
            _icolorrepository = icolorrepository;
            _ifabricrepository = ifabricrepository;
            _imaterialrepository = imaterialrepository;
            _notyf = notyf;
        }
      
        public ActionResult CategoryType()
        {
            CategoryTypePageViewModel model = new CategoryTypePageViewModel
            {
                AddCategory = new AddCategoryType(), // Empty form
                CategoryList = _imasterrepository.GetAllCategoryType() // From database or service
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
                var result = _imasterrepository.InsertorUpdateCategoryType(addCategoryType);

                if (result.ErrorCode == 0)
                {
                    _notyf.Success(result.ReturnMessage); // e.g., "Insert successful."
                }
                else
                {
                    _notyf.Error(result.ReturnMessage); // e.g., "Category already exists."
                }

                return RedirectToAction("CategoryType");
            }
            catch
            {
                _notyf.Error("Something went wrong.");
                return View();
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _imasterrepository.DeleteCategory(id);
            return Json(new { success = true });
        }

        public ActionResult SubCategoryType()
        {
            SubCategoryTypeJoinModel model = new SubCategoryTypeJoinModel();
            model.CategoryList = _imasterrepository.GetAllCategoryType();
            /*model.SubCategoryList = _imasterrepository.GetAllSubCategoriesWithCategoryName(); */// Join query
            model.SubCategoryList = _imasterrepository.GetAllSubCategoriesWithCategoryName();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubCategoryType(SubCategoryTypeJoinModel model)
        {
            if (model.SubAddCategory != null)
            {
                MasterResponse result = _imasterrepository.InsertOrUpdateSubCategory(model.SubAddCategory);

                if (result.ErrorCode == 0)
                {
                    _notyf.Success(result.ReturnMessage);
                }
                else
                {
                    _notyf.Error(result.ReturnMessage);
                }
            }

            return RedirectToAction("SubCategoryType");
        }


        public ActionResult PatternType()
        {
            PatternTypePageViewModel model = new PatternTypePageViewModel
            {
                AddPattern = new AddPatternType(), // Empty form
                PatternList = _imasterrepository.GetAllPatternType() // From database or service
            };
            return View(model);
            /* return View(model);*/
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatternType(PatternTypePageViewModel addPatternType)
        {
            try
            {
                var result = _imasterrepository.InsertOrUpdateOrDeletePattern(addPatternType);
                if (result.ErrorCode == 0)
                {
                    _notyf.Success(result.ReturnMessage); // e.g., "Insert successful."
                }
                else
                {
                    _notyf.Error(result.ReturnMessage); // e.g., "Category already exists."
                }

                return RedirectToAction("PatternType");
            }
            catch
            {
                _notyf.Error("Something went wrong.");
                return View();
            }
        }

        [HttpPost]
        public IActionResult DeletePattern(int id)
        {
            _imasterrepository.DeletePattern(id);
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult DeleteSubCategory(int id)
        {
            try
            {
                _imasterrepository.DeleteSubCategory(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
