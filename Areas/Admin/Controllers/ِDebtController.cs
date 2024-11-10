using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taavoni.Data;
using Taavoni.DTOs;
using Taavoni.Models.Entities;

namespace Taavoni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class DebtController : Controller
    {
        private readonly IDebtService _debtService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DebtController(IDebtService debtService, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _debtService = debtService;
            _context = context;
            _userManager = userManager;
        }
    [HttpPost]
    public async Task<IActionResult> ApplyDailyPenalty()
    {
        await _debtService.ApplyDailyPenalty();
        
        // پس از اجرای متد، کاربر را به صفحه ایندکس برمی‌گردانیم
        return RedirectToAction("Index");
    }
        public async Task<IActionResult> Index()
        {
            var Debts = await _debtService.GetAllDebtsAsync();
            
            return View(Debts);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var debtDetail = await _debtService.GetDebtByIdAsync(id);
            if (debtDetail == null)
                return NotFound();

            return View(debtDetail);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Users = new MultiSelectList(users, "Id", "UserName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDebtDetailDTO model)
        {
            var user = await _context.Users.FindAsync(model.UserId); // دسترسی مستقیم به کاربران
            if (user == null)
            {
                // اگر کاربر پیدا نشد، یک خطا برمی‌گردانیم یا پیام مناسبی نشان می‌دهیم
                ModelState.AddModelError("", "کاربر مورد نظر یافت نشد.");


                return View(model); // بازگشت به ویو با پیام خطا
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(model); // نمایش دوباره ویو با خطاهای مدل
            }
            await _debtService.CreateDebtDetailAsync(model);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Edit(int id)
        {

            var debtDetail = await _debtService.GetDebtByIdAsync(id);
            if (debtDetail == null)
            {
                return NotFound();
            }

            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                Console.WriteLine(user.UserName); // بررسی نام کاربری
            }


            // ارسال کاربران به ویو
            ViewBag.Users = new SelectList(_context.Users, "Id", "UserName", debtDetail.UserId);

            return View(debtDetail);
        }

        // POST: Debts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditDebtlDTO dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _debtService.UpdateDebtDetailAsync(dto);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "مشکلی در به‌روزرسانی اطلاعات پیش آمده است.");
            }
            else
            {
                ModelState.AddModelError("", "مشکلی در به‌روزرسانی اطلاعات پیش آمده است.");
            }
            return View(dto);


        }

        // GET: Debts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var debtDetail = await _debtService.GetDebtByIdAsync(id);
            if (debtDetail == null)
            {
                return NotFound();
            }

            return View(debtDetail);
        }

        // POST: Debts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _debtService.DeleteDebtDetailAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest("مشکلی در حذف اطلاعات پیش آمده است.");
        }
    }
}