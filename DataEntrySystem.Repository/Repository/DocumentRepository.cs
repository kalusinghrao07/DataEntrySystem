using DataEntrySystem.DAL.Models;
using DataEntrySystem.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntrySystem.Repository
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        private readonly DocumentDbContext _dbContext;
        public DocumentRepository(DocumentDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
