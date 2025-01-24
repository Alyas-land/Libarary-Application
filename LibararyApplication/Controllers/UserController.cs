using LibararyApplication.DatabaseContext;
using LibararyApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using System.Security.Claims;

namespace LibararyApplication.Controllers
{
    [Authorize(policy: "user")]
    public class UserController : Controller
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> AllBooks()
        {
            var allBooks = _context.Books.ToList();
            ViewBag.Header = "AllBook"; 
            return View(allBooks);
        }

        [HttpGet]
        public async Task<IActionResult> ShowBook(int bookId)
        {
            var target = await _context.Books.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == bookId);
            if (target == null)
            {
                TempData["error"] = "کتاب موردنظر پیدا نشد.";
                return RedirectToAction("AllBooks");
            }

            ViewBag.Header = "AllBook";
            return View(target);
        }

        // Recommend Book By Popular Books then those have most reservation
        public async Task<IActionResult> RecommendedSystem()
        {
            var popularBooks = await _context.Books.OrderByDescending(i => i.reservationItem.Count).Take(10).ToArrayAsync();

            ViewBag.Header = "RecommendedSystem";
            return View(popularBooks);

        }

        // Result Searchbox in navbar
        public async Task<IActionResult> ResultSearch(string bookName)
        {
            var targets = await _context.Books.Where(i => i.Name.Contains(bookName)).ToArrayAsync();

            
            int targetCount = targets.Count();
            TempData["success"] = $"تعداد {targetCount} کتاب مطابق با عبارت جست و جو شده یافت شد";
            return View(targets);
        }

        [HttpGet]
        public async Task<IActionResult> ShowBooksForReserve()
        {
            var allBooks = _context.Books.ToList();
            ViewBag.Header = "add-reserve";
            return View(allBooks);
        }

        
        public async Task<IActionResult> Reserving(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            
            if (book == null)
            {
                TempData["Error"] = "کتاب پیدا نشد.";
                return RedirectToAction("AllBooks");
            }
            if (book != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Console.WriteLine($"NameIdentifier Claim: {userId}");

                var openReservation = _context.Reservations.SingleOrDefault(i => i.UserId == userId && i.ReadStatus == false);

                if (openReservation != null) {
                    var checkProductInReservation = await _context.ReservationItems.FirstOrDefaultAsync(i => i.ReservationId == openReservation.Id && i.BookId == bookId);
                    if (checkProductInReservation != null)
                    {
                        checkProductInReservation.Quantity += 1;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newItem = new ReservationItem
                        {
                            BookId = bookId,
                            ReservationId = openReservation.Id,
                            Quantity = 1

                        };
                        await _context.AddAsync(newItem);
                    }
                    
                }
                else
                {
                    var newReservation = new Reservation
                    {
                        UserId = userId,
                        CreatedAt = DateTime.Now,
                        ReadStatus = false,
                        LibrarianStatus = false,
                        ReturnStatus = false,
                        Description = ""
                    };

                    _context.Reservations.Add(newReservation);
                    await _context.SaveChangesAsync();

                    var newReservationItems = new ReservationItem
                    {
                        BookId = book.Id,
                        ReservationId = newReservation.Id,
                        Quantity = 1
                    };
                    await _context.AddAsync(newReservationItems);

                }
                await _context.SaveChangesAsync();
                TempData["success"] = "کتاب با موفقیت رزو شد";
                return RedirectToAction("AllBooks");
            }
            TempData["error"] = ":کتاب پیدا نشد";
            return RedirectToAction("AllBooks");
        }

        public async Task<IActionResult> DeleteItemFromReservation(int bookId)
        {
            var targetBook = await _context.Books.FindAsync(bookId);
            if (targetBook == null)
            {
                TempData["error"] = "خطا! کتاب یافت نشد";
                return RedirectToAction("ShowCard");
            }
            return View();
        }

        public async Task<IActionResult> AllReservationCurrentUser()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reservationsUser = _context.Reservations.Include(i => i.User).Where(i => i.UserId == userId).ToList();
            ViewBag.Header = "show-reserve";
            return View(reservationsUser);
        }

        public async Task<IActionResult> ShowCard(int reservationId)
        {

            var targetReserveItems = await _context.ReservationItems.Include(i => i.Book).Where(i => i.ReservationId == reservationId).ToArrayAsync();


            var books = targetReserveItems.Select(i => new BookInfo
            {
                Id = i.Book.Id,
                Name = i.Book.Name,
                Quantity = i.Quantity
            }).Distinct().ToArray();

            var newShowCardViewModel = new ShowCardViewModel
            {
                reservationItems = targetReserveItems,
                books = books
            };
            ViewBag.Header = "show-reserve";
            return View(newShowCardViewModel);
        }

    }
}
