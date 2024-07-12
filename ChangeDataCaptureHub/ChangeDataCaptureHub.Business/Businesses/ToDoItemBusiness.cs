using ChangeDataCaptureHub.DataAccess;
using ChangeDataCaptureHub.Model.Models;

namespace ChangeDataCaptureHub.Business.Businesses;

public class ToDoItemBusiness : BaseBusiness<ToDoItemDocument>
{
    public ToDoItemBusiness(IBaseRepository<ToDoItemDocument> repository) : base(repository)
    {
    }
}