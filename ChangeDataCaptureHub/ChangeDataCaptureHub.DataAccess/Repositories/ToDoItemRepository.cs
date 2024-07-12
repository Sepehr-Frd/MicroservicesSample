using ChangeDataCaptureHub.Model.Models;
using Microsoft.Extensions.Options;

namespace ChangeDataCaptureHub.DataAccess.Repositories;

public class ToDoItemRepository : BaseRepository<ToDoItemDocument>
{
    public ToDoItemRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}