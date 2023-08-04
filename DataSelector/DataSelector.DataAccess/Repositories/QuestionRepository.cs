using DataSelector.Model.Models;
using Microsoft.Extensions.Options;

namespace DataSelector.DataAccess.Repositories;

public class QuestionRepository : BaseRepository<QuestionDocument>
{
    public QuestionRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}

