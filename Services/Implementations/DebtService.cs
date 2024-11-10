using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Taavoni.Data;
using Taavoni.DTOs;
using Taavoni.Models;

public class DebtService : IDebtService
{
    private readonly ApplicationDbContext _context;

    public DebtService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DebtDetailDTO>> GetAllDebtsAsync()
    {
        var Debts = await _context.Debts.Include(d => d.User).ToListAsync();
        return Debts.Select(d => new DebtDetailDTO
        {
            Id = d.Id,
            StartDate = d.StartDate,
            EndDate = d.EndDate,
            Guard = d.Guard,
            IsPaid = d.IsPaid,
            Name = d.User?.Name,
            UserName = d.User?.UserName,
            UserId = d.UserId,
            DueDate = d.DueDate,
            PenaltyRate =d.PenaltyRate,
            RemainingAmount = d.RemainingAmount

        }).ToList();
    }

    public async Task<DebtDetailDTO> GetDebtByIdAsync(int id)
    {
        //  var debtDetail = await _context.Debts.Include(d=>d.User).FindAsync(id);
        //   if (debtDetail == null) return null;
        var debtDetail = await _context.Debts
        .Include(d => d.User)
        .FirstOrDefaultAsync(d => d.Id == id);

        if (debtDetail == null) return null;
        return new DebtDetailDTO
        {
            Id = debtDetail.Id,
            StartDate = debtDetail.StartDate,
            EndDate = debtDetail.EndDate,
            Guard = debtDetail.Guard,
            IsPaid = debtDetail.IsPaid,
            UserId = debtDetail.UserId,
            UserName = debtDetail.User?.Name,
            RemainingAmount = debtDetail.RemainingAmount,
            DueDate = debtDetail.DueDate,
            PenaltyRate = debtDetail.PenaltyRate
        };
    }

    public async Task AddDebtDetailAsync(DebtDetailDTO debtDetailDTO)
    {
        var debtDetail = new Debt
        {
            StartDate = debtDetailDTO.StartDate,
            EndDate = debtDetailDTO.EndDate,
            Guard = debtDetailDTO.Guard,
            IsPaid = debtDetailDTO.IsPaid,
            UserId = debtDetailDTO.UserId,
            DueDate = debtDetailDTO.DueDate,
            PenaltyRate = debtDetailDTO.PenaltyRate,
            RemainingAmount = debtDetailDTO.RemainingAmount


        };
        await _context.Debts.AddAsync(debtDetail);
        await _context.SaveChangesAsync();
    }

    public async Task CreateDebtDetailAsync(CreateDebtDetailDTO createDebtDetailDTO)
    {

        var debtDetail = new Debt
        {
            StartDate = createDebtDetailDTO.StartDate,
            EndDate = createDebtDetailDTO.EndDate,
            Guard = createDebtDetailDTO.Guard,
            IsPaid = createDebtDetailDTO.IsPaid,
            UserId = createDebtDetailDTO.UserId,
            DueDate = createDebtDetailDTO.DueDate,
            PenaltyRate = createDebtDetailDTO.PenaltyRate,
            RemainingAmount = createDebtDetailDTO.Guard
        };

        _context.Debts.Add(debtDetail);
        await _context.SaveChangesAsync();
    }


    public async Task<bool> UpdateDebtDetailAsync(EditDebtlDTO dto)
    {
        var debtDetail = await _context.Debts.FindAsync(dto.Id);
        if (debtDetail == null)
        {
            return false;
        }

        debtDetail.StartDate = dto.StartDate;
        debtDetail.EndDate = dto.EndDate;
        debtDetail.Guard = dto.Guard;
        debtDetail.IsPaid = dto.IsPaid;
        debtDetail.DueDate = dto.DueDate;
        debtDetail.PenaltyRate = dto.PenaltyRate;

        // debtDetail.UserId = dto.UserId;

        _context.Debts.Update(debtDetail);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteDebtDetailAsync(int id)
    {
        var debtDetail = await _context.Debts.FindAsync(id);
        if (debtDetail == null)
        {
            return false;
        }

        _context.Debts.Remove(debtDetail);
        await _context.SaveChangesAsync();

        return true;
    }
        public async Task ApplyDailyPenalty()
    {
        var debts = await _context.Debts
            .Where(d => d.DueDate < DateTime.Now && d.RemainingAmount > 0)
            .ToListAsync();

        foreach (var debt in debts)
        {

            // اگر جریمه امروز برای این بدهی اعمال نشده باشد
        if (debt.LastPenaltyAppliedDate != DateTime.Today)
        {
         
       
            var daysDelayed = (DateTime.Now - debt.DueDate).Days;
            var penalty = debt.Guard * debt.PenaltyRate * daysDelayed;
            debt.RemainingAmount += penalty;

            // بروزرسانی تاریخ آخرین اعمال جریمه
            debt.LastPenaltyAppliedDate = DateTime.Today;
        }
        }

        await _context.SaveChangesAsync();
    }



}
