using System.Threading.Tasks;
using Minibank.Core;

namespace Minibank.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly MiniBankContext _context;

        public EfUnitOfWork(MiniBankContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}