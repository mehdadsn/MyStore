﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Interface.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.EditProduct;
using Store.Common.Role;
using Store.Persistence.Migrations;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{ConstRoles.Admin},{ConstRoles.Operator}")]

    public class ProductController : Controller
    {
        private readonly IProductFacad _prodoctFacad;
        public ProductController(IProductFacad productFacad)
        {
            _prodoctFacad = productFacad;
        }
        public IActionResult Index(int page =1, int pageSize = 20)
        {
            return View(_prodoctFacad.GetProductForAdminService.Execute(page, pageSize).Data);
        }
        
        public IActionResult Detail(long Id)
        {
            return View(_prodoctFacad.GetProductDetailForAdminService.Execute(Id).Data);
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Categories = new SelectList(_prodoctFacad.GetCategoreisForNewProductService.Execute().Data, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(RequestAddNewProductDto request, List<AddNewProductFeatures> features)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }

            request.Images = images;
            request.Features = features;
            return Json(_prodoctFacad.AddNewProductService.Execute(request));
        }

        [HttpPost]
        public IActionResult DeleteProduct(long productId)
        {
            return Json(_prodoctFacad.DeleteProductService.Execute(productId));
        }

        [HttpGet]
        public IActionResult EditProduct(long productId)
        {
            ViewBag.Categories = new SelectList(_prodoctFacad.GetCategoreisForNewProductService.Execute().Data, "Id", "Name");
            return View(_prodoctFacad.GetProductDetailForAdminService.Execute(productId));
        }
        [HttpPost]
        public IActionResult EditProduct(RequestEditProductDto editRequset, List<EditProductFeatures> features)
        {
            List<IFormFile> images = new List<IFormFile>();
            for (int i = 0; i < Request.Form.Files.Count; i++)
            {
                var file = Request.Form.Files[i];
                images.Add(file);
            }
            editRequset.Images = images;
            editRequset.Features = features;
            return Json(_prodoctFacad.EditProductService.Execute(editRequset));
        }
    }
}
