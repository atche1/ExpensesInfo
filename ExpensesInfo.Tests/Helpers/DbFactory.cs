using ExpensesInfo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
namespace ExpensesInfo.Tests.Helpers
{
    public static class DbFactory
    {
        public static ExpensesInfoDbContext CreateInMemory(string dbName)
        {
            var options = new DbContextOptionsBuilder<ExpensesInfoDbContext>()
.UseInMemoryDatabase(dbName)
.Options;
            return new ExpensesInfoDbContext(options);
        }

    }
}