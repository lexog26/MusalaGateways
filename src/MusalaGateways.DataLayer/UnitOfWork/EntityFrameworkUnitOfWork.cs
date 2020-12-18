using Microsoft.EntityFrameworkCore;
using MusalaGateways.DataLayer.UnitOfWork.Interface;
using System.Threading.Tasks;

namespace MusalaGateways.DataLayer.UnitOfWork
{
    /// <summary>
    /// UnitOfWork's implementation based on Entity Framework
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public sealed class EntityFrameworkUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private TContext _context;

        public EntityFrameworkUnitOfWork(TContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Save changes for current context
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Save changes async way
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}
