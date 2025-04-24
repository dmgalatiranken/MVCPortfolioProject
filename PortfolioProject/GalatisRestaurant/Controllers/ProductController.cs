using GalatisRestaurant.Data;
using GalatisRestaurant.Models;
using Microsoft.AspNetCore.Mvc;

namespace GalatisRestaurant.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment; // For uploading images

        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await products.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            ViewBag.Ingredients = await ingredients.GetAllAsync();
            ViewBag.Categories = await categories.GetAllAsync();
            if (id == 0)
            {
                // Add new product since id is 0
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                // id is not 0, so edit the product
                Product product = await products.GetByIdAsync(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient, Category"
                });
                ViewBag.Operation = "Edit";
                return View(product);
            }
        }

        [HttpPost]
        // ingredientIds is from the name attribute in the checkbox, that's also why it's an array (checkbox, not radio)
        // catId is from the name attribute in the radio select, that's why it's not an array (radio, not checkbox)
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId) 
        {
            if (ModelState.IsValid)
            {
                if (product.ImageFile != null)
                {
                    // _webHostEnvironment is used to get the path to the wwwroot folder, then navigate to the 'images' subfolder
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Generate a unique file name using a GUID to prevent file name collisions (in case two uploaded files have the same name)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;

                    // Combine the uploads folder path with the unique file name to get the full file path
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        // Save the uploaded image to the wwwroot/images folder
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    // Store the unique file name in the product's ImageUrl property (can be used later to display the image)
                    product.ImageUrl = uniqueFileName;
                }

                if (product.ProductId == 0)
                {
                    // Add operation since ProductId is 0
                    ViewBag.Ingredients = await ingredients.GetAllAsync();
                    ViewBag.Categories = await categories.GetAllAsync();
                    product.CategoryId = catId; // Setting the product's category id

                    // Loops through ingredients and adds each ingredient to the product's join table
                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    await products.AddAsync(product);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    // Retrieve the existing product from the database
                    var existingProduct = await products.GetByIdAsync(product.ProductId, new QueryOptions<Product> { Includes = "ProductIngredients" });

                    if (existingProduct == null)
                    {
                        ModelState.AddModelError("", "Product not found.");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }

                    // Update the existing product's properties
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.Stock = product.Stock;
                    existingProduct.CategoryId = catId;

                    // Update the product's ingredients
                    existingProduct.ProductIngredients?.Clear(); // Clear existing ingredients
                    foreach (int id in ingredientIds)
                    {
                        existingProduct.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }

                    try
                    {
                        await products.UpdateAsync(existingProduct);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", $"Error: {ex.GetBaseException().Message}");
                        ViewBag.Ingredients = await ingredients.GetAllAsync();
                        ViewBag.Categories = await categories.GetAllAsync();
                        return View(product);
                    }
                }
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index", "Product");
            }
            catch
            {
                ModelState.AddModelError("", "Product not found.");
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
