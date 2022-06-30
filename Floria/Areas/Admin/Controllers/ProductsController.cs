using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Repositories;
using Business.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Floria.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public ProductsController(IProductService productRepository, ICategoryService categoryRepositories, IWebHostEnvironment env)
        {
            _productService = productRepository;
            _categoryService = categoryRepositories;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products;
            try
            {
                products = await _productService.GetAll();
            }
            catch(NullReferenceException ex)
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
                product =await _productService.Get(id);
            }
            catch (ArgumentNullException ex)
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
            ViewData["categories"] = categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Product product)
        {
            string path = _env.WebRootPath;


            return Json(path);


            var categories = await _categoryService.GetAll();
            ViewData["categories"] = categories;
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _productService.Create(product);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {

            if (id is null)
            {
                throw new ArgumentNullException("Id");
            }
            //Burda niyə productRep işlətdikki?
            var data = await _productService.Get(id);

            //var data = await _categoryService.Get(id);
            if (data is null)
            {
                throw new NullReferenceException();
            }
            var categories = await _categoryService.GetAll();
            ViewData["categories"] = categories;
            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,Product product)
        {
            var categories = await _categoryService.GetAll();
            ViewData["categories"] = categories;


            if (!ModelState.IsValid)
            {
                return View(product);
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

