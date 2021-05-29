using DataEntrySystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntrySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet("GetAll")]
        public async Task<List<Contracts.Document>> GetAll(string searchText, int pageNo, int pageSize)
        {
            return await _documentService.GetAll(searchText, pageNo, pageSize);
        }

        [HttpGet("GetById/{id}")]
        public async Task<Contracts.Document> GetById(Guid id)
        {
            return await _documentService.GetById(id);
        }

        [HttpPost("AddUpdateDocument")]
        public async Task<Contracts.Document> AddUpdateDocument([FromBody] Contracts.Document model)
        {
            return await _documentService.AddUpdateDocument(model);
        }

        [HttpDelete("DeleteById/{id}")]
        public bool DeleteById(Guid id)
        {
            bool isDelete;
            isDelete = _documentService.DeleteById(id);
            return isDelete;
        }
    }
}
