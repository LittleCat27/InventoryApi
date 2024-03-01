using InventoryApi.Models.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : Controller
    {
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(ILogger<HistoryController> logger)
        {
            _logger = logger;
        }


        [HttpGet("GetList")]
        public JsonResult GetList(string? filtro = "")
        {
            
            List<ItemPrice> list = new List<ItemPrice>();
            
            try
            {
                using (var db = new InventoryDBContext())
                {
                    List<ItemPrice> listAux = new List<ItemPrice>();
                    if (filtro != null && filtro != "")
                    {
                        listAux = (
                        from item in db.Items
                        where item.Name.ToLower().Contains(filtro.ToLower())
                        select new ItemPrice
                        {
                            ItemId = item.ItemId,
                            Name = item.Name,
                        }).ToList();
                    }
                    else
                    {
                        listAux = (
                        from item in db.Items
                        select new ItemPrice
                        {
                            ItemId = item.ItemId,
                            Name = item.Name,
                        }).ToList();
                    }
                        
                    foreach(var item in listAux) { 
                        list.AddRange((from i in db.ActualPricings
                                where i.ItemId == item.ItemId
                                orderby i.PricingId descending
                                select new ItemPrice
                                {
                                    ItemId= item.ItemId,
                                    Name = item.Name,
                                    LastUpdate = i.LastUpdate,
                                    Price = i.Price,
                                    PriceId = i.PricingId
                                }).ToList());
                    }
                }


                return new JsonResult(list);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public ActionResult Delete(DeletePrice item)
        {
            try
            {
                using (var db = new InventoryDBContext())
                {
                    if (item.ItemId != 0)
                    {
                       var ap = db.ActualPricings.Where(i => i.PricingId == item.PriceId && i.PricingId == item.PriceId).FirstOrDefault();

                        if (ap == null) return StatusCode(404, "Item or Price not found");

                        db.ActualPricings.Remove(ap);
                        db.SaveChanges();
                    }
                    else return StatusCode(404, "Item or Price not found");
                }
                return Ok("Delete Successful");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
