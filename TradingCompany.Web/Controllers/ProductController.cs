using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DTO;
using TradingCompany.MVC.Models;
using TradingCompany.DALEF.Concrete; 

namespace TradingCompany.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _manager;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductManager manager, IMapper mapper, ILogger<ProductController> logger)
        {
            _manager = manager;
            _mapper = mapper;
            _logger = logger;
        }

        [AllowAnonymous]
        public ActionResult Index() => View(_manager.GetAllProducts());

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var product = _manager.GetProductById(id);
            return product == null ? NotFound() : View(product);
        }

        private void AddDictionaries(EditProductModel model)
        {
            model.Categories = _mapper.Map<List<SelectListItem>>(_manager.GetAllCategories());
            model.Suppliers = _mapper.Map<List<SelectListItem>>(_manager.GetAllSuppliers());
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
        public ActionResult Create()
        {
            var model = new EditProductModel();
            AddDictionaries(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
        public ActionResult Create(EditProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _manager.CreateProduct(_mapper.Map<Product>(model));
                    if (result != null) return RedirectToAction("Index");

                    ModelState.AddModelError(string.Empty, "Database failed to save product.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, "Server error.");
            }

            AddDictionaries(model);
            return View(model);
        }

        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
        public ActionResult Edit(int id)
        {
            var product = _manager.GetProductById(id);
            if (product == null) return NotFound();

            var model = _mapper.Map<EditProductModel>(product);
            AddDictionaries(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Manager)}")]
        public ActionResult Edit(int id, EditProductModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ProductId = id;
                    var result = _manager.UpdateProduct(_mapper.Map<Product>(model));
                    if (result != null) return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", id);
            }

            AddDictionaries(model);
            return View(model);
        }

        [Authorize(Roles = nameof(RoleType.Admin))]
        public ActionResult Delete(int id)
        {
            var product = _manager.GetProductById(id);
            return product == null ? NotFound() : View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = nameof(RoleType.Admin))]
        public ActionResult DeleteConfirmed(int id)
        {
            var success = _manager.DeleteProduct(id);
            if (success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Cannot delete: Product is linked to orders.");
            return View(_manager.GetProductById(id));
        }
    }
}