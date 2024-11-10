using Taavoni.Models.Entities;

namespace Taavoni.Models;

public class Debt
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Guard { get; set; }
    public bool IsPaid { get; set; } = true;// وضعیت پرداخت
    public string UserId { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? LastPenaltyAppliedDate { get; set; } // تاریخ آخرین اعمال جریمه
    public decimal PenaltyRate { get; set; } // درصد جریمه روزانه
    /// <summary>
    /// ارتباطات 
    /// </summary>
    public List<Payment> Payments { get; set; }

    public ApplicationUser User { get; set; }
}

