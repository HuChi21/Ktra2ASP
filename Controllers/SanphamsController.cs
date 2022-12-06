using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ktra2ASP.Models;
using PagedList;
using WebGrease.Css.Ast;

namespace Ktra2ASP.Controllers
{
    public class SanphamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Sanphams
        public ActionResult Index(string sortOrder,string SearchString,string currentFilter, int? page)
        {
            //sap xep
            ViewBag.CurrentSort = sortOrder;
            ViewBag.Xeptheoten = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Xeptheogia = sortOrder == "Giatien" ? "gia_desc" : "Giatien";

            var sanphams = db.Sanphams.Select(p => p);
            if (!String.IsNullOrEmpty(SearchString))
            {
                sanphams = sanphams.Where(p => p.Tenvd.Contains(SearchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    sanphams = sanphams.OrderByDescending(s => s.Tenvd);
                    break;
                case "Giatien":
                    sanphams = sanphams.OrderBy(s => s.Giatien);
                    break;
                case "gia_desc":
                    sanphams = sanphams.OrderByDescending(s => s.Giatien);
                    break;
                default:
                    sanphams = sanphams.OrderBy(s => s.Tenvd);
                    break;

            }

            if(SearchString != null ){
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 3;
            int pageNum = (page ?? 1);

            return View(sanphams.ToPagedList(pageNum, pageSize));
        }

        // GET: Sanphams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // GET: Sanphams/Create
        public ActionResult Create()
        {
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc");
            return View();
        }

        // POST: Sanphams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Mavd,Tenvd,TenAnh,Mota,Giatien,Soluong,MaDanhmuc")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                sanpham.TenAnh = "";
                var f = Request.Files["ImageFile"];
                if(f!=null && f.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(f.FileName);  
                    string UploadPath = Server.MapPath("~/Images" + filename); 
                    f.SaveAs(UploadPath);  
                    sanpham.TenAnh = filename;
                }
                db.Sanphams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }

        // GET: Sanphams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }

        // POST: Sanphams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Mavd,Tenvd,TenAnh,Mota,Giatien,Soluong,MaDanhmuc")] Sanpham sanpham)
        {
            if (ModelState.IsValid)
            {
                var f = Request.Files["ImageFile"];
                if (f != null && f.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(f.FileName);
                    string UploadPath = Server.MapPath("~/Images/" + filename);
                    f.SaveAs(UploadPath);
                    sanpham.TenAnh = filename;
                }
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDanhmuc = new SelectList(db.Danhmucs, "MaDanhmuc", "TenDanhmuc", sanpham.MaDanhmuc);
            return View(sanpham);
        }

        // GET: Sanphams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sanpham sanpham = db.Sanphams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // POST: Sanphams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sanpham sanpham = db.Sanphams.Find(id);
            db.Sanphams.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
