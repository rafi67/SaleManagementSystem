using INvoicePracitic.Models;
using INvoicePracitic.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INvoicePracitic.Controllers
{
    public class SalesController : Controller
    {
        private readonly Context db;
        private readonly IWebHostEnvironment env;
        public SalesController(Context db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        [HttpPost]
        public async Task<IActionResult> Add(VmSale model)
        {
            string fileName = null;
            if(model.File != null)
            {
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
            }
            
            var sm = new SaleMaster();
            sm.CreateDate = model.CreateDate;
            sm.SaleId = model.SaleId;
            sm.Name = model.Name;
            sm.Address = model.Address;
            sm.Photo = fileName;
            sm.Gender = model.Gender;
            db.SaleMasters.Add(sm);
            db.SaveChanges();
            var sd = new List<SaleDetails>();
            for(int i=0; i< model.ProductName.Length; i++)
            {
                var osd = new SaleDetails();
                osd.ProductName = model.ProductName[i];
                osd.Price = model.Price[i];
                osd.SaleId = sm.SaleId;
                osd.Qty = model.Qty[i];
                sd.Add(osd);
            }
            db.SaleDetails.AddRange(sd);
            db.SaveChanges();
            return RedirectToAction("Report");
        }
        [HttpGet]
        public IActionResult Report()
        {
            var master = db.SaleMasters.ToList();
            var sd = db.SaleDetails.ToList();
            ViewData["master"] = master;
            ViewData["Details"] = sd;
            return View();
        }
        [HttpGet]
        public IActionResult ShowImage(string fileName)
        {
            if (fileName == null) return null;
            var filePath = Path.Combine(env.WebRootPath, "images", fileName);
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, $"images/filename");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var sale = new VmSale();
            var sm = db.SaleMasters.Find(id);
            if(sm!=null)
            {
                sale.SaleId = sm.SaleId;
                sale.CreateDate = sm.CreateDate;
                sale.Photo = sm.Photo;
                sale.Name = sm.Name;
                sale.Address = sm.Address;
                sale.Gender = sm.Gender;
                var sdList = db.SaleDetails.Where(x => x.SaleId == id).ToList();
                var sdlist = new List<VmSale.VmSaleDetail>();
                foreach(var list in  sdList) 
                { 
                    var sd = new VmSale.VmSaleDetail();
                    sd.SaleId = list.SaleId;
                    sd.SaleDetailId = list.SaleDetailId;
                    sd.ProductName = list.ProductName;
                    sd.Qty = list.Qty;
                    sd.Price = list.Price;
                    sdlist.Add(sd);
                }
                sale.SaleDetails = sdlist;
            }
            return View(sale);
        }
        [HttpPost]
        public IActionResult Update(VmSale model) 
        {

            string fileName = null;
            if (model.File == null)
            {
                fileName = model.Photo;
            }
            else
            {
                string imagePath = Path.Combine(env.WebRootPath, "images", model.Photo);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
                var filePath = Path.Combine(env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
            }
            var sm = new SaleMaster();
            sm.CreateDate = model.CreateDate;
            sm.Name = model.Name;
            sm.Photo = fileName;
            sm.SaleId = model.SaleId;
            sm.Address = model.Address;
            sm.Gender  = model.Gender;
            db.Entry(sm).State = EntityState.Modified;
            var sdr = db.SaleDetails.Where(x=>x.SaleId==model.SaleId).ToList();
            db.SaleDetails.RemoveRange(sdr);
            db.SaveChanges();
            var sdlist = new List<SaleDetails>();
            for (int i = 0; i < model.ProductName.Length; i++)
            {
                var osd = new SaleDetails();
                osd.ProductName = model.ProductName[i];
                osd.SaleId = sm.SaleId;
                osd.Price = model.Price[i];
                osd.Qty = model.Qty[i];
                sdlist.Add(osd);
            }
            db.SaleDetails.AddRange(sdlist);
            db.SaveChanges();
            return RedirectToAction("Report");
        }
        [HttpPost]
        public void DeleteImage(string image)
        {
    
            if (image != null)
            {
                string imagePath = Path.Combine(env.WebRootPath, "images", image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            var sm = db.SaleMasters.Find(id);
            var sd = db.SaleDetails.Where(s=>s.SaleId==id).ToList();
            var d = sm.Photo;
            if (sm != null)
            {
                db.SaleMasters.Remove(sm);
                db.SaleDetails.RemoveRange(sd);
                DeleteImage(d);
                db.SaveChanges();
            }
            return RedirectToAction("Report");
        }
    } 
}
