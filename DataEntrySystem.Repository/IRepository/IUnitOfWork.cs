using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntrySystem.Repository
{
    public interface IUnitOfWork
    {
        void Save();
        void Dispose();
    }
}
