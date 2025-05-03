using GalatisRestaurant.Data;
using GalatisRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GalatisRestaurant.Controllers
{
    public class ReviewController : Controller
    {
        // Repository for Review model data
        private readonly Repository<Review> reviews;

        // Retrieves the UserManager service for user management
        private readonly UserManager<ApplicationUser> userManager;

        // Database context for accessing the database
        private readonly ApplicationDbContext context;

        public ReviewController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Initialize the repository with the database context
            reviews = new Repository<Review>(context);

            // Assign the user manager for user management
            this.userManager = userManager;

            // Assign the database context
            this.context = context;
        }

        [Route("/reviews")]
        public async Task<IActionResult> Index()
        {
            // Retrieve all reviews from the database
            var reviewList = await context.Reviews
                .Include(r => r.User) // Includes the associated User for each review
                .ToListAsync();

            return View(reviewList);
        }


        [Authorize]
        [HttpGet]
        public IActionResult AddReview()
        {
            // Returns a view for a user to add a review
            return View(new Review()); 
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(Review review)
        {
            // Retrieves the current user
            var user = await userManager.GetUserAsync(User);

            // Setting the user id for the review
            review.UserId = user.Id;

            // Setting the review date to the current date and time
            review.ReviewDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                TempData["successmessage"] = "Review added successfully!";

                // Add the review to the database
                await reviews.AddAsync(review);
                return RedirectToAction("Index");
            }

            // If the model state is not valid, return the view with the review object
            return View(review); 
        }
    }
}
