using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataEntrySystem.Services
{
    public interface IDocumentService
    {
        Task<Contracts.Document> AddUpdateDocument(Contracts.Document model);
        Task<List<Contracts.Document>> GetAll(string searchText, int pageNo, int pageSize);
        Task<Contracts.Document> GetById(Guid id);
        bool DeleteById(Guid id);
    }
}
