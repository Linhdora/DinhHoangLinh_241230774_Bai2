using DinhHoangLinh_241230774_Bai2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DinhHoangLinh_241230774_Bai2.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly QLTaiSanContext _db;
        public HomeController(QLTaiSanContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.loaiTaiSans = await _db.LoaiTaiSans.ToListAsync();
            ViewBag.TieuDe = "Dinh Hoang Linh";
            var ts = await _db.TaiSans.Where(p => p.NguyenGia != null && p.NguyenGia >= 20).ToListAsync();
            return View(ts);
        }
        public async Task<IActionResult> TraVe(int id)
        {
            var ts = await _db.TaiSans.Include(p => p.MaLoaiNavigation).Where(p => p.MaLoai == id).ToListAsync();
            ViewBag.TenLoai = await _db.LoaiTaiSans.Where(l => l.MaLoai == id).Select(l => l.TenLoai).FirstOrDefaultAsync();
            return PartialView("DanhSach",ts);
        }
        private async Task NapDuLieuForm(int? maLoai = null)
        {
            var ds = await _db.LoaiTaiSans.ToListAsync();
            ViewBag.loaiTaiSans = ds;
            ViewBag.DsLoai = new SelectList(ds, "MaLoai", "TenLoai", maLoai);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ts = await _db.TaiSans.FindAsync(id);
            if (ts == null) return NotFound();
            await NapDuLieuForm(ts.MaLoai);
            return View(ts);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaiSan model)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (!ModelState.IsValid)
            {
                await NapDuLieuForm(model.MaLoai);
                return View(model);
            }
            var ts = await _db.TaiSans.FindAsync(model.MaTs);
            if (ts == null) return NotFound();
            ts.MaLoai = model.MaLoai;
            ts.TenTs = model.TenTs;
            ts.NguyenGia = model.NguyenGia;
            ts.Anh = model.Anh;
            try
            {
                await _db.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                return Content(ex.InnerException?.Message ?? ex.Message);
            }
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
