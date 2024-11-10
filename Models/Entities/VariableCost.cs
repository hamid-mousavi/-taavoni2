using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taavoni.Models.Entities;

namespace taavoni2.Models.Entities
{
   public class VariableCost
{
    public int Id { get; set; }
    public string Title { get; set; } // عنوان هزینه
    public decimal Amount { get; set; } // مبلغ هزینه
    public DateTime DueDate { get; set; } // تاریخ سررسید
    public DateTime? PaymentDate { get; set; } // تاریخ پرداخت (اختیاری)
    public decimal RemainingAmount { get; set; } // مبلغ باقیمانده
    public decimal PaidAmount { get; set; }=0; // مبلغ پرداختی
    public decimal Delay { get; set; } // دیرکرد (به روز)

    // ارتباط یک به چند با جدول پرداخت‌ها
    public ICollection<Payment> Payments { get; set; }

    // ارتباط یک به چند با جدول کاربران
    public string ApplicationUserId { get; set; } // کلید خارجی به ApplicationUser
    public ApplicationUser ApplicationUser { get; set; } // ارتباط با کاربر
}

}