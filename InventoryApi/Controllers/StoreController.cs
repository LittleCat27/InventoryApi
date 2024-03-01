using InventoryApi.Models.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly ILogger<StoreController> _logger;

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetList")]
        public JsonResult GetList(string? filtro = "")
        {
            List<StoreCLS> list = new List<StoreCLS>();
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (filtro != null && filtro != "") list = (
                            from store in db.Stores
                            where store.Name.ToLower().Contains(filtro.ToLower())
                            select new StoreCLS
                            {
                                AddressId = store.AddressId,
                                Name = store.Name,
                                StoreId = store.StoreId
                            }).ToList();
                    else list = (
                            from store in db.Stores
                            select new StoreCLS
                            {
                                AddressId = store.AddressId,
                                Name = store.Name,
                                StoreId = store.StoreId
                            }).ToList();
                    foreach (StoreCLS store in list)
                    {
                        store.AddressName = db.Addresses.Where(a => a.AddressId == store.AddressId).FirstOrDefault().Address1;
                    }
                }
                

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet("GetAddressList")]
        public JsonResult GetAddressList()
        {
            List<AddressCLS> list = new List<AddressCLS>();
            try
            {
                using (var db = new InventoryDBContext())
                {
                    list = (from address in db.Addresses
                            select new AddressCLS
                            {
                                id = address.AddressId,
                                name = address.Address1,
                                cityId = address.CityId,
                                postalCode = address.PostalCode
                            }).ToList();
                }

                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        [HttpPut("Update")]
        public ActionResult Update(StoreCLS x)
        {
            if (x == null) return StatusCode(404, x);

            try
            {
                using (var db = new InventoryDBContext())
                {
                    var y = (
                        from s in db.Stores
                        where s.StoreId == x.StoreId
                        select s
                        ).FirstOrDefault();
                    if (y == null) return StatusCode(404, "Store not found");
                    y.Name = x.Name;
                    y.AddressId = x.AddressId;
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
                        var y = db.Stores.Where(s => s.StoreId == id).First();
                        if (y != null)
                        {
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
        public ActionResult Create(StoreCLS store)
        {
            try
            {
                if (string.IsNullOrEmpty(store.Name)) return StatusCode(202, "Store name is empty");
                if (store.AddressId == 0) return StatusCode(203, "Store address is empty");

                using (var db = new InventoryDBContext())
                {
                    var x = db.Stores.Where(s => s.Name.ToLower() == store.Name.ToLower() && s.AddressId == store.AddressId).FirstOrDefault();

                    if (x != null) return StatusCode(201, "Store already exists");

                    Store newStore = new Store();
                    newStore.Name = store.Name;
                    newStore.AddressId = store.AddressId;
                    db.Stores.Add(newStore);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }


        [HttpPut("UpdateStock")]
        public ActionResult Update(int itemId = 0, int storeId = 0, int stock = 0)
        {
            if (itemId == 0 || storeId == 0 || stock == null) return StatusCode(404, "itemId:"+itemId+", stockId:" + storeId ); ;

            try
            {
                using (var db = new InventoryDBContext())
                {
                    var x = db.Inventories.Where(i => i.ItemId == itemId && i.StoreId == storeId).FirstOrDefault();

                    if (x == null || x.ItemId == 0)
                    {
                        Inventory i = new Inventory
                        {
                            ItemId = itemId,
                            StoreId = storeId,
                            Quantity = stock,
                            Expiration = DateTime.Now
                        };
                        db.Inventories.Add(i);
                    }
                    else
                    {
                        x.Quantity = stock;
                    }


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
