using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AdminDyanamoEnterprises.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogsRepository _repository;

        public BlogsController(IBlogsRepository repository)
        {
            _repository = repository;
        }

        // GET: /Blogs/List
        public IActionResult List()
        {
            var blogs = _repository.GetAllBlogs();
            return View(blogs);
        }

        // GET: /Blogs/Create
        public IActionResult AddBlog()
        {
            return View();
        }

        // POST: /Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBlog(BlogsModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    model.ImageUrl = "uploads/" + fileName; // Save relative path
                }

                _repository.InsertBlog(model);
                return RedirectToAction("List");
            }

            return View(model);
        }


        // GET: /Blogs/Edit/{id}
        public IActionResult EditBlog(int id)
        {
            var blog = _repository.GetBlogById(id);
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBlog(BlogsModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    var filePath = Path.Combine(uploadsPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    // Save relative path for display
                    model.ImageUrl = "uploads/" + fileName;
                }

                _repository.UpdateBlog(model);
                return RedirectToAction("List");
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                TempData["ErrorMessage"] = "Invalid Blog ID.";
                return RedirectToAction("List");
            }

            _repository.DeleteBlog(id);
            TempData["SuccessMessage"] = "Blog post deleted successfully.";
            return RedirectToAction("List");
        }

        // Optional: GET /Blogs/Details/{id}
        public IActionResult Details(int id)
        {
            var blog = _repository.GetBlogById(id);
            if (blog == null)
                return NotFound();

            return View(blog);
        }
        [HttpPost]
        public IActionResult TogglePublish(int id, bool published)
        {
            var blog = _repository.GetBlogById(id);
            if (blog == null)
                return NotFound();

            blog.Published = published;
            _repository.UpdateBlog(blog);

            return Ok();
        }

    }
}
