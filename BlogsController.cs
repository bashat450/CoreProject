using AdminDyanamoEnterprises.DTOs.Master;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BlogsModel model)
        {
            if (ModelState.IsValid)
            {
                _repository.InsertBlog(model);
                return RedirectToAction("List");
            }
            return View(model);
        }

        // GET: /Blogs/Edit/{id}
        public IActionResult Edit(int id)
        {
            var blog = _repository.GetBlogById(id);
            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(BlogsModel model)
        {
            if (ModelState.IsValid)
            {
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
