using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registration.Controllers
{
    
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly student_dbContext _context;

        public HomeController(student_dbContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var studentList = await _context.StudentInformations
         .OrderByDescending(s => s.Id) 
         .Select(s => new
         {
                s.Id,
                s.Name,
                s.Email,
                s.MobileNo,
                s.Lang,
                s.Gender,
                s.Languages,
                s.Date,
                PhotoPath = s.PhotoPath ?? "No Photo Provided", // Default text if PhotoPath is NULL
                DocumentPath = s.DocumentPath ?? "No Document Provided" // Default text if DocumentPath is NULL
            })
            .ToListAsync();


            List<StudentInformation> ss = new List<StudentInformation>();
            foreach (var student in studentList)
            {
                StudentInformation stdInfo = new StudentInformation();
                stdInfo.PhotoPath = student.PhotoPath ?? "No Photo Provided";  // Set default if null
                stdInfo.DocumentPath = student.DocumentPath ?? "No Document Provided";  // Set default if null
                stdInfo.Id = student.Id;
                stdInfo.Name = student.Name;
                stdInfo.Email = student.Email;
                stdInfo.MobileNo = student.MobileNo;
                stdInfo.Lang = student.Lang;
                stdInfo.Gender = student.Gender;
                stdInfo.Languages = student.Languages;
                stdInfo.Date = student.Date;
                ss.Add(stdInfo);
            }

            return View(ss);


            return View(studentList);
        }


        // GET: Home/Create
        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            ViewBag.Language = GetLanguageOption();

            ViewBag.Languages = GetLanguageOptions();
            return View();
        }

        [Route("[action]")]
        [HttpPost]

        public async Task<IActionResult> Create([Bind("Name,Email,MobileNo,Lang,Gender,Languages,Date,PhotoFile,DocumentFile,lstLanguages")] StudentInformation studentInformation)
        {
            try
            {
                ViewBag.Language = GetLanguageOption();
                ViewBag.Languages = GetLanguageOptions();
                if (!ModelState.IsValid)
                {
                    return View(studentInformation); // Return the view with validation errors
                }

                // Set upload directory
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Ensure the upload directory exists
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Handle photo file upload
                if (studentInformation.PhotoFile != null && studentInformation.PhotoFile.Length > 0)
                {
                    try
                    {
                        var photoFileName = Guid.NewGuid() + Path.GetExtension(studentInformation.PhotoFile.FileName);
                        var photoFilePath = Path.Combine(uploadDirectory, photoFileName);

                        using (var fileStream = new FileStream(photoFilePath, FileMode.Create))
                        {
                            await studentInformation.PhotoFile.CopyToAsync(fileStream);
                        }

                        // Save the file path to the model
                        studentInformation.PhotoPath = photoFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Photo upload failed: {ex.Message}");
                        return View(studentInformation); // Return view with error
                    }
                }



                if (studentInformation.DocumentFile != null && studentInformation.DocumentFile.Length > 0)
                {
                    try
                    {
                        // Generate a unique name for the PDF file
                        var pdfFileName = Guid.NewGuid() + Path.GetExtension(studentInformation.DocumentFile.FileName);
                        var pdfFilePath = Path.Combine(uploadDirectory, pdfFileName);

                        // Save the PDF file to the specified path
                        using (var fileStream = new FileStream(pdfFilePath, FileMode.Create))
                        {
                            await studentInformation.DocumentFile.CopyToAsync(fileStream);
                        }

                        // Save the file path to the model
                        studentInformation.DocumentPath = pdfFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, $"PDF upload failed: {ex.Message}");
                        return View(studentInformation); // Return view with error
                    }
                }

                // Concatenate selected languages into a comma-separated string
                if (Request.Form["lstLanguages"].Count > 0)
                {
                    studentInformation.Languages = string.Join(",", Request.Form["lstLanguages"]);
                }

                // Save the student information to the database
                try
                {
                    _context.Add(studentInformation);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Database operation failed: {ex.Message}");
                    return View(studentInformation); // Return view with error
                }

                // Redirect to the index after successful creation
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the unexpected error and rethrow or handle gracefully
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return View(studentInformation);
            }



            if (ModelState.IsValid)
            {
                string selectedLang = studentInformation.Lang;

                // Concatenate selected languages into a comma-separated string
                if (Request.Form["lstLanguages"].Count > 0)
                {
                    studentInformation.Languages = string.Join(",", Request.Form["lstLanguages"]);
                }

                _context.Add(studentInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //ViewBag.Languages = GetLanguageOptions();
            //ViewBag.Language = GetLanguageOption();

            return View(studentInformation);
        }

        // GET: Home/Edit/5
        [HttpGet]

        [Route("[action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInformation = await _context.StudentInformations.FirstOrDefaultAsync(x => x.Id == id);


            if (studentInformation == null)
            {
                return NotFound();
            }
            ViewBag.ImageFileName = studentInformation.PhotoPath;
            ViewBag.DocumentFileName = studentInformation.DocumentPath;

            ViewBag.Language = GetLanguageOption();

            ViewBag.Languages = GetLanguageOptions();

            return View(studentInformation);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Email,MobileNo,Lang,Gender,Languages,Date,Id,PhotoFile,DocumentFile,lstLanguages")] StudentInformation studentInformation)
        {
            if (id != studentInformation.Id)
            {
                return NotFound();
            }

            // Fetch the existing entity from the database
            var existingStudent = await _context.StudentInformations.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            // Update basic fields
            existingStudent.Name = studentInformation.Name;
            existingStudent.Email = studentInformation.Email;
            existingStudent.MobileNo = studentInformation.MobileNo;
            existingStudent.Lang = studentInformation.Lang;
            existingStudent.Gender = studentInformation.Gender;
            existingStudent.Date = studentInformation.Date;

            // Handle Photo Upload
            if (studentInformation.PhotoFile != null && studentInformation.PhotoFile.Length > 0)
            {
                try
                {
                    // Generate new file name and save new photo
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var photoFileName = Guid.NewGuid() + Path.GetExtension(studentInformation.PhotoFile.FileName);
                    var photoFilePath = Path.Combine(uploadDirectory, photoFileName);

                    using (var fileStream = new FileStream(photoFilePath, FileMode.Create))
                    {
                        await studentInformation.PhotoFile.CopyToAsync(fileStream);
                    }

                    // Delete the old file only after successfully saving the new one
                    if (!string.IsNullOrEmpty(existingStudent.PhotoPath))
                    {
                        var oldPhotoPath = Path.Combine("wwwroot/uploads", existingStudent.PhotoPath);
                        if (System.IO.File.Exists(oldPhotoPath))
                        {
                            System.IO.File.Delete(oldPhotoPath);
                        }
                    }

                    existingStudent.PhotoPath = photoFileName; // Update with new file name
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Photo upload failed: {ex.Message}");
                }
            }
            else
            {
                // Retain the existing photo path
                studentInformation.PhotoPath = existingStudent.PhotoPath;
            }

            // Handle Document Upload
            if (studentInformation.DocumentFile != null && studentInformation.DocumentFile.Length > 0)
            {
                try
                {
                    // Generate new file name and save new document
                    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var documentFileName = Guid.NewGuid() + Path.GetExtension(studentInformation.DocumentFile.FileName);
                    var documentFilePath = Path.Combine(uploadDirectory, documentFileName);

                    using (var fileStream = new FileStream(documentFilePath, FileMode.Create))
                    {
                        await studentInformation.DocumentFile.CopyToAsync(fileStream);
                    }

                    // Delete the old file only after successfully saving the new one
                    if (!string.IsNullOrEmpty(existingStudent.DocumentPath))
                    {
                        var oldDocumentPath = Path.Combine("wwwroot/uploads", existingStudent.DocumentPath);
                        if (System.IO.File.Exists(oldDocumentPath))
                        {
                            System.IO.File.Delete(oldDocumentPath);
                        }
                    }

                    existingStudent.DocumentPath = documentFileName; // Update with new file name
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Document upload failed: {ex.Message}");
                }
            }
            else
            {
                // Retain the existing document path
                studentInformation.DocumentPath = existingStudent.DocumentPath;
            }

            // Handle Languages
            if (Request.Form["lstLanguages"].Count > 0)
            {
                existingStudent.Languages = string.Join(",", Request.Form["lstLanguages"]);
            }
            else
            {
                ModelState.AddModelError("lstLanguages", "Please select at least one language.");
            }

            
               try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentInformationExists(existingStudent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
         
            if (studentInformation.PhotoFile == null)
            {
                studentInformation.PhotoPath = existingStudent.PhotoPath; // Keep existing file
            }
            if (studentInformation.DocumentFile == null)
            {
                studentInformation.DocumentPath = existingStudent.DocumentPath; // Keep existing file
            }

            // Reload language options in case of errors
            ViewBag.Language = GetLanguageOption();
            ViewBag.Languages = GetLanguageOptions();
            return View(studentInformation);
        }

        // GET: Home/Details/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInformation = await _context.StudentInformations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentInformation == null)
            {
                return NotFound();
            }

            return View(studentInformation);
        }


        // GET: Home/Delete/5
        [HttpGet]
        [Route("Home/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentInformation = await _context.StudentInformations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (studentInformation == null)
            {
                return NotFound();
            }

            return View(studentInformation);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Home/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentInformation = await _context.StudentInformations.FindAsync(id);

            if (studentInformation != null)
            {
                _context.StudentInformations.Remove(studentInformation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }







        private bool StudentInformationExists(int id)
        {
            return (_context.StudentInformations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private List<SelectListItem> GetLanguageOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "C#", Text = "C#" },
                new SelectListItem { Value = "Java", Text = "Java" },
                new SelectListItem { Value = ".NET", Text = ".NET" },
                new SelectListItem { Value = "PHP", Text = "PHP" }
            };
        }
        private List<SelectListItem> GetLanguageOption()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Value = "English", Text = "English" },
        new SelectListItem { Value = "Hindi", Text = "Hindi" },
        new SelectListItem { Value = "Marathi", Text = "Marathi" },
        new SelectListItem { Value = "Japanese", Text = "Japanese" }
    };
        }

        

       

    }
}
