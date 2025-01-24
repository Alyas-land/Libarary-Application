using LibararyApplication.DatabaseContext;
using LibararyApplication.Models;
using LibararyApplication.Models.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibararyApplication.Controllers
{
    [Authorize(Policy = "admin")]
    public class AdminController : Controller
    {
        private readonly MyDbContext _dbContext;
        public AdminController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> UsersList()
        {
            var allUsers = _dbContext.Users.ToArray();

            var allUserViewModel = allUsers.Select(i => new AllUserViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Role = i.Role,
                CreatedAt = i.CreatedAccount
            }).ToArray();

            ViewBag.Header = "usersList";
            return View(allUserViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var target = await _dbContext.Users.SingleOrDefaultAsync(i => i.Id == userId);
            if (target == null)
            {
                return RedirectToAction("UsersList");
            }
            _dbContext.Users.Remove(target);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("UsersList");
        }

        public async Task<IActionResult> SubmitUserToLibrarian(int userId)
        {
            var target = await _dbContext.Users.SingleOrDefaultAsync(i => i.Id == userId);
            if (target == null)
            {
                return RedirectToAction("UsersList");
            }
            target.Role = "librarian";

            _dbContext.Update(target);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("UsersList");
        }

        // Show All Reservation
        public async Task<IActionResult> Allreserved()
        {
            var allReservation = await _dbContext.Reservations.Include(i => i.User).ToListAsync();


            ViewBag.Header = "allReserved";
            return View(allReservation);
        }

        // Show Card
        public async Task<IActionResult> ShowCard(int reservationId)
        {
            var targetRservation = await _dbContext.ReservationItems.Include(i => i.Book).Where(i => i.ReservationId == reservationId).ToArrayAsync();

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
