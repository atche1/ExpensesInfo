using System.ComponentModel.DataAnnotations;

namespace ExpensesInfo.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Дата")]
        public DateTime Date { get;set; } = DateTime.Today;
        [Required]
        [Display(Name = "Expense Type")]
        public int ExpenseTypeId {  get; set; }
        public ExpenseType? ExpenseType { get; set; }
    }
}
