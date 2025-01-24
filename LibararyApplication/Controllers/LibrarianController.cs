using LibararyApplication.DatabaseContext;
using LibararyApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace LibararyApplication.Controllers
{
    [Authorize(Policy = "librarian")]
    public class LibrarianController : Controller
    {
        private readonly MyDbContext _context;

        public LibrarianController(MyDbContext context)
        {
            _context = context;
        }
        
        // Get All Book From DB
        public async Task<IActionResult> AllBooks()
        {
            var allBooks = _context.Books.ToList();
            ViewBag.Header = "booksOperator";
            return View(allBooks);
        }

        // All Category http post method
        public async Task<IActionResult> AllCategory()
        {
            var categories = await _context.Categories.ToArrayAsync();
            return View(categories);
        }

        // Add Category http post method
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }

        // Add Category http post method
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            if(model == null)
            {
                TempData["error"] = "اطلاعات دسته بندی ناقص می باشد";
                return RedirectToAction("AllBooks");
            }
            var newCategory = new Category
            {
                Name = model.Name,
                CreatedAt = DateTime.UtcNow,
            };
            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            TempData["success"] = "دسته بندی با موفقیت اضافه شد";
            return RedirectToAction("AllCategory");

        }

        // Add Book By Book Id  --> action method : get
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            var categories = await _context.Categories.ToArrayAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            {
                
            };

            ViewBag.Header = "booksOperator";
            return View();
        }

        // Add Book By Book Id  --> action method : post
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _context.Categories.ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                TempData["error"] = "اطلاعات ناقص می باشد";
                return View(model);
            }

            var newBook = new Book
            {
                Name = model.Name,
                Description = model.Description,
                Quantity = model.Quantity,
                CategoryId = model.CategoryId,

            };
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            if (model.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    newBook.Id + Path.GetExtension(model.Picture.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(stream);
                }
            }
            TempData["success"] = "کتاب با موفقیت افزوده گردد";
            return RedirectToAction("AllBooks");
        }

        // Delete Book By Id
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var bookTarget = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (bookTarget == null)
            {
                TempData["error"] = "کتاب مورد نظر یافت نشد";
                return RedirectToAction("AllBooks");
            }

            _context.Books.Remove(bookTarget);
            await _context.SaveChangesAsync();

            TempData["success"] = "عملیات حدف کتاب با موفقیت انجام شد";
            return RedirectToAction("AllBooks");
        }

        // Edit book by Id:
        [HttpGet]
        public async Task<IActionResult> EditBook(int bookId)
        {
            var bookTarget = await _context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
            if (bookTarget == null)
            {
                TempData["error"] = "کتاب مورد نظر یافت نشد";
                return RedirectToAction("AllBooks");
            }

            var targetToShowFront = new EditBookViewModel
            {
                Id = bookId,
                Name = bookTarget.Name,
                Description = bookTarget.Description,
                Quantity = bookTarget.Quantity,
            };

            ViewBag.Header = "booksOperator";
            return View(targetToShowFront);
        }

        // Edit book by Id:
        [HttpPost]
        public async Task<IActionResult> EditBook(EditBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "اطلاعات ناقص وارد شده است";
                return View(model);
            }
            var targetBook = await _context.Books.FindAsync(model.Id);
            targetBook.Name = model.Name;
            targetBook.Description = model.Description;
            targetBook.Quantity = model.Quantity;
            
            
            await _context.SaveChangesAsync();

            if (model.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    targetBook.Id + Path.GetExtension(model.Picture.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(stream);
                }
            }

            TempData["success"] = "ویرایش مشخصات کتاب با موفقیت انجام شد";
            return RedirectToAction("AllBooks");

        }

        // Show All Reservation
        public async Task<IActionResult> Allreserved()
        {
            var allReservation = await _context.Reservations.Include(i => i.User).ToListAsync();

            
            ViewBag.Header = "allReserved";
            return View(allReservation);
        }

        // Submit Reserved
        public async Task<IActionResult> SubmitReserved(int reservationId)
        {
            var target = await _context.Reservations.Include(i => i.reservationItem)
                .ThenInclude(rb => rb.Book).FirstOrDefaultAsync(i => i.Id == reservationId);

            if (target == null)
            {
                TempData["erroe"] = "رزو کتاب یافت نشد";
                return RedirectToAction("Allreserved");
            }

            foreach (var item in target.reservationItem)
            {
                if (item.Quantity > item.Book.Quantity)
                {
                    TempData["erroe"] = $"موجودی کتاب {item.Book.Name} از مقدار سفارش داده شده کمتر است";
                    return RedirectToAction("Allreserved");

                }

                item.Book.Quantity -= item.Quantity;
            }

            target.ReadStatus = true;
            target.LibrarianStatus = true;
            target.SubmitTime = DateTime.Now;
            target.Description = "رزو کتاب تایید شد";
            await _context.SaveChangesAsync();
            TempData["success"] = "وضعبت سفارش با موفیت به تایید تفییر یافت";
            return RedirectToAction("Allreserved");
        }

        // Unsubmit Reserved
        [HttpGet]
        public async Task<IActionResult> UnsubmitReserved(int reservationId)
        {
            var target = await _context.Reservations.FirstOrDefaultAsync(i => i.Id == reservationId);

            if (target == null) {
                return RedirectToAction();

            }

            target.ReadStatus = true;
            target.LibrarianStatus = false;
            target.Description = "سبد رزو شما توسط کتابدار تایید نشد";

            await _context.SaveChangesAsync();

            return RedirectToAction("Allreserved");
        }

        // Rrturn Reserve 
        public async Task<IActionResult> ReturningReservation(int reservationId)
        {
            var target = await _context.Reservations.Include(i => i.reservationItem)
                .ThenInclude(rb => rb.Book).FirstOrDefaultAsync(i => i.Id == reservationId);

            if (target == null)
            {
                TempData["erroe"] = "رزو کتاب یافت نشد";
                return RedirectToAction("Allreserved");
            }

            foreach (var item in target.reservationItem)
            {
                item.Book.Quantity += item.Quantity;
            }

            target.ReturnStatus = true;
            await _context.SaveChangesAsync();
            TempData["success"] = "برگشت امانت با موفقیت انجام شد";
            return RedirectToAction("Allreserved");
        }

        // Show Card
        public async Task<IActionResult> ShowCard(int reservationId)
        {
            var targetRservation = await _context.ReservationItems.Include(i => i.Book).Where(i => i.ReservationId == reservationId).ToArrayAsync();

            var book = targetRservation.Select(i => new BookInfo
            {
                Id = i.Book.Id,
                Name = i.Book.Name,
                Quantity = i.Quantity
            }).Distinct().ToArray();

            var newShowCardViewModel = new ShowCardViewModel
            {
                books = book,
                reservationItems = targetRservation
            };

            ViewBag.Header = "allReserved";
            return View(newShowCardViewModel);
        }
    }
}

