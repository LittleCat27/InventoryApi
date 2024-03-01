using InventoryApi.Models.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetList")]
        public JsonResult GetList(string? filtro = "")
        {
            List<CategoryCLS> list = new List<CategoryCLS>();
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (filtro == null)
                    {
                        list = (from category in db.Categories
                                select new CategoryCLS
                                {
                                    Id = category.CategoryId,
                                    Name = category.Name
                                }).ToList();
                    }
                    else
                    {
                        list = (from category in db.Categories
                                where category.Name.ToLower().Contains(filtro.ToLower())
                                select new CategoryCLS
                                {
                                    Id = category.CategoryId,
                                    Name = category.Name
                                }).ToList();
                    }
                }

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("Update")]
        public ActionResult Update(CategoryCLS x)
        {
            if (x == null) return StatusCode(404, x);

            try
            {
                using (var db = new InventoryDBContext())
                {
                    var y = (
                        from c in db.Categories
                        where c.CategoryId == x.Id
                        select c
                        ).FirstOrDefault();
                    if (y == null) return StatusCode(404, "Category not found");
                    y.Name = x.Name;
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (id != 0)
                    {
                        var y = db.Categories.Where(c => c.CategoryId == id).Include(c => c.Items).First();
                        if(y != null)
                        {
                            y.Items.Clear();
                            await db.SaveChangesAsync();
                            db.Remove(y);
                            await db.SaveChangesAsync();
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpPost("Create")]
        public ActionResult Create(CategoryCLS category)
        {
            try
            {
                if (string.IsNullOrEmpty(category.Name)) return StatusCode(202, "Category name is empty");

                using (var db = new InventoryDBContext())
                {
                    var x = db.Categories.Where(c => c.Name.ToLower() == category.Name.ToLower()).FirstOrDefault();

                    if (x != null) return StatusCode(201, "Category already exists");

                    Category newCategory = new Category();
                    newCategory.Name = category.Name;
                    db.Categories.Add(newCategory);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

    }
}
