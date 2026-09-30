using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly librarycontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentsController(librarycontext context, IWebHostEnvironment webHostEnvironment)
        {

            this._context = context;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var model = _context.Students.OrderBy(s => s.Name).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (student.Photo != null && student.Photo.Length > 0)
            {
                string ext = Path.GetExtension(student.Photo.FileName).ToLower();
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photo");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                
                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    var uniqueFileName = student.Name + ext;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        student.Photo.CopyTo(fileStream);
                    }
                    student.PhotoPath = "/photo/" + uniqueFileName;
                    _context.Students.Add(student);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving the student.");
                        return View(student);
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Only .jpg, .jpeg, and .png files are allowed.");
                    
                }

                
            }
            return View(student);
        }
        public IActionResult Edit(int? Id)
        {
            if(Id == null)
            {
                return BadRequest();
            }
            var model= _context.Students.Find(Id);
            if(model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {

            //if(student.PhotoPath != null )
            //{
            //    student.PhotoPath = student.PhotoPath;
            //}
            //else
            //{
                if (student.Photo != null && student.Photo.Length > 0)
                {
                    string ext = Path.GetExtension(student.Photo.FileName).ToLower();
                    string oldPath = student.PhotoPath;
                    string oldfullpath = "wwwroot"+oldPath;

                    System.IO.File.Delete(oldfullpath);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Photo");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        var uniqueFileName = student.Name + ext;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            student.Photo.CopyTo(fileStream);
                        }

                        student.PhotoPath = "/photo/" + uniqueFileName;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Only .jpg, .jpeg, and .png files are allowed.");
                        return View(student);
                    }
                }
                else
            {
                student.PhotoPath = student.PhotoPath;
            }
            //}


    _context.Students.Update(student);
            if(_context.SaveChanges() > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while updating the student.");
            }
            return View(student);
        }
        public IActionResult Delete(int id)
        {
            var model = _context.Students.Find(id);
            if (model != null)
            {
                string oldPath = model.PhotoPath;
                string oldfullpath = "wwwroot" + oldPath;

                System.IO.File.Delete(oldfullpath);
                _context.Students.Remove(model);

                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while deleting the student.");
                    return View("Index", _context.Students.ToList());
                }

            }
            else
            {
                return NotFound();
            }
        }
    }
}
