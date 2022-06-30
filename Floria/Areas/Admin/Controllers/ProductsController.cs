using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Repositories;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P226_ASP_Intro.Helpers.Extensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace P226_ASP_Intro.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductService productRepository,
                                  ICategoryService categoryRepository,
                                  IImageService imageService,
                                  IWebHostEnvironment env)
        {
            _productService = productRepository;
            _categoryService = categoryRepository;
            _imageService = imageService;
            _env = env;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            List<Product> products;

            try
            {
                products = await _productService.GetAll();
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            Product product;
            try
            {
                product = await _productService.Get(id);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAll();
            ViewData["categoies"] = categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var categories = await _categoryService.GetAll();
            ViewData["categoies"] = categories;
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (product.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image can not be empty");
                return View(product);
            }

            string fileName = await product.ImageFile.CreateFile(_env);

            Image image = new Image();
            image.Name = fileName;

            await _imageService.Create(image);

            product.ImageId = image.Id;

            await _productService.Create(product);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
            {
                throw new ArgumentNullException("Id");
            }
            var data = await _productService.Get(id);
            if (data is null)
            {
                throw new NullReferenceException("Product is null");
            }

            var categories = await _categoryService.GetAll();
            ViewData["categoies"] = categories;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product)
        {
            var categories = await _categoryService.GetAll();
            ViewData["categoies"] = categories;

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (product.ImageFile != null)
            {
                string fileName = await product.ImageFile.CreateFile(_env);

                Image image = new Image();
                image.Name = fileName;

                await _imageService.Create(image);

                int? oldImageId = product.ImageId;

                product.ImageId = image.Id;

                await _productService.Update(id, product);

                await _imageService.Delete(oldImageId);
            }



            await _productService.Update(id, product);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                await _productService.Delete(id);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (NullReferenceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
