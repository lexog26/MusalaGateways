using System;
using System.Threading.Tasks;

namespace MusalaGateways.DataLayer.UnitOfWork.Interface
{
    /// <summary>
    /// Unit of Work design pattern definition
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Commit();

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>Task with the number of objects in an Added, Modified, or Deleted state</returns>
        Task<int> CommitAsync();
    }
}
