using OnlineLibrary.Data.Infrastructure;
using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public class PublishingService:IPublishingService
  {
    private IRepository<Publishings> publishingRepo;
    private IUnitOfWork unitOfWork;

    public PublishingService(IRepository<Publishings> publishingRepo, IUnitOfWork unitOfWork)
    {
      this.publishingRepo = publishingRepo;
      this.unitOfWork = unitOfWork;
    }

    public List<Publishings> GetAllRecords()
    {
      return publishingRepo.GetAll().ToList();
    }

    public void CreatePublishing(Publishings publishing)
    {
      publishingRepo.Add(publishing);
      unitOfWork.Commit();
    }
  }
}
