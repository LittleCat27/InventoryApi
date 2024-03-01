using InventoryApi.Models.Clases;
using InventoryApi.Models;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting.Internal;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetItemList")]
        public JsonResult GetItemList(string? filtro = "")
        {
            List<ItemCLS> list = new List<ItemCLS>();
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (filtro != null && filtro != "") list = (
                            from item in db.Items
                            where item.Name.ToLower().Contains(filtro.ToLower())
                            select new ItemCLS
                            {
                                Barcode = item.Barcode,
                                Code = item.Code,
                                Description = item.Description,
                                ImageUrl = item.ImageUrl,
                                ItemId = item.ItemId,
                                Name = item.Name
                            }).ToList();
                    else list = (
                            from item in db.Items
                            select new ItemCLS
                            {
                                Barcode = item.Barcode,
                                Code = item.Code,
                                Description = item.Description,
                                ImageUrl = item.ImageUrl,
                                ItemId = item.ItemId,
                                Name = item.Name
                            }).ToList();
                    foreach(ItemCLS i in list)
                    {
                        var z = (
                        from item in db.ActualPricings
                        where item.ItemId == i.ItemId
                        orderby item.PricingId descending
                        select item
                        ).FirstOrDefault();
                        if (z != null) i.price = z.Price;

                        var c = db.Items.Where(x => x.ItemId == i.ItemId).Include(x => x.Categories).FirstOrDefault();

                        List<CategoryCLS> categorias = new List<CategoryCLS>();

                        if (c != null)
                            foreach (var cat in c.Categories)
                                categorias.Add(new CategoryCLS { Id = cat.CategoryId, Name = cat.Name });

                        if (categorias.Count > 0) i.categories = categorias;
                    }
                }

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost("GetList")]
        public JsonResult GetList(ItemCLS filter)
        {
            List<ItemCLS> list = new List<ItemCLS>();
            try
            {
                using (var db = new InventoryDBContext())
                {
                    List<int> catIds = new List<int>();
                    List<Item> x;


                    if (filter.categories.Count > 0)
                    {
                        foreach (CategoryCLS i in filter.categories) catIds.Add(i.Id);
                        var c = db.Categories.Where(i => catIds.Contains(i.CategoryId));
                        x = db.Items.Include(i => i.Categories).Where(i => i.Categories.Intersect(c).Any()).Include(i => i.Inventories).ToList();
                    }
                    else x = db.Items.Include(i => i.Categories).Include(i => i.Inventories).ToList();
                    
                    if (filter != null)
                    {
                        if (filter.Name != "") x = x.Where(i => i.Name.ToLower().Contains(filter.Name.ToLower())).ToList();
                        if (filter.Barcode != "") x = x.Where(i => i.Barcode == filter.Barcode).ToList();
                    }

                        



                    if (x !=  null && x.Count > 0)
                    {
                        list = (from item in x
                                select new ItemCLS
                                {
                                    Barcode = item.Barcode,
                                    Code = item.Code,
                                    Description = item.Description,
                                    ImageUrl = item.ImageUrl,
                                    ItemId = item.ItemId,
                                    Name = item.Name,
                                    quantity = TotalQuantity(item.Inventories.ToList()),
                                    inventory =GetInventories(item.Inventories.ToList(),db.Stores.ToList())
                                }).ToList();
                    }

                    foreach (ItemCLS i in list)
                    {
                        var z = (
                        from item in db.ActualPricings
                        where item.ItemId == i.ItemId
                        orderby item.PricingId descending
                        select item
                        ).FirstOrDefault();
                        if (z != null) i.price = z.Price;

                        var c = db.Items.Where(x => x.ItemId == i.ItemId).Include(x => x.Categories).FirstOrDefault();

                        List<CategoryCLS> categorias = new List<CategoryCLS>();

                        if (c != null)
                            foreach (var cat in c.Categories)
                                categorias.Add(new CategoryCLS { Id = cat.CategoryId, Name = cat.Name });

                        if (categorias.Count > 0) i.categories = categorias;
                    }
                }

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        private static List<InventoryCLS> GetInventories(List<Inventory> x, in List<Store> stores)
        {
            var list = new List<InventoryCLS>();

            foreach (Inventory item in x)
            {
                string name = stores.Where(s => s.StoreId == item.StoreId).FirstOrDefault().Name;
                list.Add(new InventoryCLS
                {
                    quantity = item.Quantity,
                    storeId = item.StoreId,
                    storeName = name
                });
            }

            
            return list;
        }
        private static int TotalQuantity(List<Inventory> x)
        {
            int total = 0;
            foreach (Inventory item in x) total += item.Quantity;
            return total;
        }

        [HttpGet("GetItemQuantity")]
        public JsonResult GetItemQ(int? item_id, int? store_id)
        {
            int x = 0;
            try
            {
                using (var db = new InventoryDBContext())
                {
                    var y = (from inv in db.Inventories
                         where inv.ItemId == item_id && inv.StoreId == store_id
                         select inv).FirstOrDefault();
                    if (y != null) x = y.Quantity; else x = 0;
                }
                return new JsonResult(x);
            }
            catch
            {
                return new JsonResult(0);
            }

            

        }

        [HttpGet("GetItem")]
        public JsonResult GetItem(int? id)
        {
            ItemCLS x = new ItemCLS();

            using (var db = new InventoryDBContext())
            {
                var y = (from item in db.Items.Include(i => i.Inventories)
                         where item.ItemId == id
                         select new ItemCLS
                        {
                            Barcode = item.Barcode,
                            Code = item.Code,
                            Description = item.Description,
                            ImageUrl = item.ImageUrl,
                            ItemId = item.ItemId,
                            Name = item.Name,
                            quantity = TotalQuantity(item.Inventories.ToList()),
                            inventory = GetInventories(item.Inventories.ToList(),db.Stores.ToList())
                        }).FirstOrDefault();
                if (y != null) x = y;

                var z = (
                        from item in db.ActualPricings
                        where item.ItemId == id
                        orderby item.PricingId descending
                        select item
                        ).FirstOrDefault();
                if (z != null) x.price = z.Price;

                var c = db.Items.Where(i => i.ItemId == id).Include(i => i.Categories).FirstOrDefault();

                List<CategoryCLS> categorias = new List<CategoryCLS>();

                if (c != null) 
                    foreach(var cat in c.Categories)
                        categorias.Add(new CategoryCLS {Id = cat.CategoryId, Name = cat.Name});
                if (categorias.Count > 0) x.categories = categorias;
            }

            return new JsonResult(x);
        }

        [HttpPut("UpdateItem")]
        public ActionResult UpdateItem(ItemCLS x)
        {
            if (x == null) return StatusCode(404, x);

            try
            {
                using (var db = new InventoryDBContext())
                {
                    var y = (
                        from item in db.Items
                        where item.ItemId == x.ItemId
                        select item
                        ).FirstOrDefault();

                    if (y == null) return StatusCode(404, "Item no encontrado");

                    y.Barcode = x.Barcode;
                    y.Code = x.Code;
                    y.Description = x.Description;
                    y.ImageUrl = x.ImageUrl;
                    y.Name = x.Name;

                    var z = (
                        from item in db.ActualPricings
                        where  item.ItemId == x.ItemId
                        orderby item.PricingId descending
                        select item
                        ).FirstOrDefault();

                    ActualPricing p = new ActualPricing
                    {
                        ItemId = x.ItemId,
                        Price = x.price,
                        LastUpdate = DateTime.Now
                    };

                    if (z != null)
                    {
                        p.PricingId = z.PricingId + 1;
                        if (z.Price != x.price)
                        {
                            db.ActualPricings.Add(p);
                        }
                    }
                    else
                    {
                        p.PricingId = 1;
                        db.ActualPricings.Add(p);
                    }


                    db.SaveChanges();

                }

                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
            
        }


        [HttpDelete("DeleteItem")]
        public ActionResult Delete(int? id)
        {
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (id != 0)
                    {
                        try
                        {
                            var x = db.Inventories.Where(i => i.ItemId == id).ToList();
                            db.RemoveRange(x);
                        }
                        catch { }
                        try
                        {
                            var y = db.ActualPricings.Where(i => i.ItemId == id).ToList();
                            db.RemoveRange(y);
                        } catch { }
                        db.SaveChanges();
                        var item = db.Items.Where(i => i.ItemId == id).Include(i => i.Categories).First();
                        item.Categories.Clear();
                        db.SaveChanges();
                        db.Remove(item);
                        db.SaveChanges();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("CreateItem")]
        public ActionResult Create(ItemCLS item)
        {
            try
            {
                using (var db = new InventoryDBContext())
                {
                    Item newItem = new Item
                    {
                        Barcode = item.Barcode,
                        Code = item.Code,
                        Description = item.Description,
                        ImageUrl = item.ImageUrl,
                        Name = item.Name
                    };
                    db.Items.Add(newItem);
                    
                    db.SaveChanges();

                    ActualPricing p = new ActualPricing
                    {
                        ItemId = newItem.ItemId,
                        Price = item.price,
                        LastUpdate = DateTime.Now
                    };
                    db.ActualPricings.Add(p);
                    db.SaveChanges();
                }
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        public ActionResult UpdateCategory(ItemCategories item)
        {
            try
            {
                using (var db = new InventoryDBContext())
                {
                    var x = db.Items.Where(i => i.ItemId == item.id).Include(i => i.Categories).First();

                    if (x != null)
                    {
                        var y = db.Categories.Where(c => item.Categories.Contains(c.CategoryId)).ToList();
                        x.Categories = y;
                        //foreach (Category c in y) x.Categories.Add(c); //Agrego todas las categorias que tenga en la lista
                        db.SaveChanges();
                    }
                    else return StatusCode(404, "Categories not found in the database");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUpload model)
        {

            if (model == null)
            {
                return Content("Invalid Submission!");
            }


            if (model.Image == null)
            {
                return Content("File not selected");
            }

            if (model.ItemId == 0)
            {
                return Content("Item not selected");
            }


            var path = Path.Combine(AppContext.BaseDirectory, @"Images\", (Custom.RandomString(32) + Path.GetExtension(model.Image.FileName)));

            model.ImageUrl = path;

            try
            {
                using (var db = new InventoryDBContext())
                {
                    var x = db.Items.Where(i => i.ItemId == model.ItemId).First();

                    if (x != null) x.ImageUrl = model.ImageUrl;
                    else return Content("Item with id " + model.ItemId + " not found");

                    db.SaveChanges();
                }

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                    stream.Close();
                }
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


    }
}