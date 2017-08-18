using System;
using System.Threading.Tasks;
using vega.Interfaces;

namespace vega.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;
        public UnitOfWork(VegaDbContext context)
        {
            _context = context;
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}