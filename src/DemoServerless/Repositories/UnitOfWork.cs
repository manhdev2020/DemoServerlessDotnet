using DemoServerless.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoServerless.Repositories
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreDbContext _context;

        public UnitOfWork(CoreDbContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            // Implement code to rollback changes
        }
    }

}
