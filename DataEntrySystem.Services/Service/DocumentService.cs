using AutoMapper;
using DataEntrySystem;
using DataEntrySystem.DAL.Models;
using DataEntrySystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntrySystem.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IDocumentRepository _documentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DocumentService(IMapper mapper, IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Contracts.Document> AddUpdateDocument(Contracts.Document model)
        {
            Contracts.Document documentModel = null;
            Document document = null;
            if (model.Id != null && model.Id != Guid.Empty)
            {
                document = await _documentRepository.GetByIDAsync(model.Id);
                if (document != null)
                {
                    document.FirstName = model.FirstName;
                    document.LastName = model.LastName;
                    document.Email = model.Email;
                    document.DOB = model.DOB;
                    _documentRepository.Update(document);
                }
            }
            else
            {
                document = _mapper.Map<Document>(model);
                document.Id = Guid.NewGuid();
                await _documentRepository.InsertAsync(document);
            }
            _unitOfWork.Save();
            documentModel = _mapper.Map<Contracts.Document>(document);
            return documentModel;
        }
        public async Task<List<Contracts.Document>> GetAll(string searchText, int pageNo, int pageSize)
        {
            List<Contracts.Document> documentes = new List<Contracts.Document>();
            try
            {
                Func<IQueryable<Document>, IOrderedQueryable<Document>> orderBy = null;

                var DocumentData = _documentRepository.Get(orderBy: orderBy, includeProperties: "", pageIndex: pageNo, pageSize: pageSize);
                if (!string.IsNullOrEmpty(searchText))
                {
                    DocumentData = DocumentData.Where(x => x.FirstName.Contains(searchText) || x.LastName.Contains(searchText) || x.Email.Contains(searchText));
                }

                documentes = _mapper.Map<List<Contracts.Document>>(DocumentData.ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
            return documentes;
        }
        public async Task<Contracts.Document> GetById(Guid id)
        {
            Contracts.Document documentModel = null;
            Document document = await _documentRepository.GetByIDAsync(id);
            documentModel = _mapper.Map<Contracts.Document>(document);
            return documentModel;
        }
        public bool DeleteById(Guid id)
        {
            bool isDelete;
            try
            {
                Document document = _documentRepository.GetByID(id);
                _documentRepository.Delete(document);
                _unitOfWork.Save();
                isDelete = true;
            }
            catch (Exception ex)
            {
                isDelete = false;
            }
            return isDelete;
        }
    }
}
