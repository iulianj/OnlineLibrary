using OnlineLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibrary.Services
{
  public interface IPublishingService
  {
    List<Publishings> GetAllRecords();
    void CreatePublishing(Publishings publishing);
    void EditPublishing(Publishings publishing);
    void DeletePublishing(Publishings publishing);
  }
}
