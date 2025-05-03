using GalatisRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GalatisRestaurant.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        // UserManager service for user management
        private UserManager<ApplicationUser> userManager;

        // RoleManager service for role management
        private RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userMngr, RoleManager<IdentityRole> roleMngr)
        {
            // Initializes userManager for user management
            userManager = userMngr;

            // Initializes roleManager for role management
            roleManager = roleMngr;
        }

        public async Task<IActionResult> Index()
        {
            // Retrieve all users and their respective roles
            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (ApplicationUser user in userManager.Users)
            {
                // Retrieve the roles assigned to each user
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user); // Add the user with roles to the list
            }

            // Create a view model with the users and available roles
            UserViewModel model = new UserViewModel
            {
                Users = users,
                Roles = roleManager.Roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Find the user by their ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Attempt to delete the user
                IdentityResult result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Collect error messages and store them in TempData
                    string errorMessage = "";
                    foreach (IdentityError error in result.Errors)
                    {
                        errorMessage += error.Description + " | ";
                    }
                    TempData["message"] = errorMessage;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            // Retrieve the role by its name
            IdentityRole role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                // If the role is not found, display an error message
                TempData["message"] = $"Role '{roleName}' not found.";
            }
            else
            {
                // Retrieve the user by their ID
                ApplicationUser user = await userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    // If the user is not null/the user is found, add them to the role
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
            // Retrieve the user by their ID
            ApplicationUser user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                // If the user is not null/the user is found, remove them from the role
                await userManager.RemoveFromRoleAsync(user, roleName);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            // Creates a new role based on the name provided from the view (the create role form)
            await roleManager.CreateAsync(new IdentityRole(rolename));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminRole()
        {
            // Creates a new role named "Admin"
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            // Retrieves the role by its ID to be deleted
            IdentityRole role = await roleManager.FindByIdAsync(id);

            // Deletes the retrieved role
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}
