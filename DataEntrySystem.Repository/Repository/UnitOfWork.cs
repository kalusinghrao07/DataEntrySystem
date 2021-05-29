using DataEntrySystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntrySystem.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private DocumentDbContext _context;

        public UnitOfWork(DocumentDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
