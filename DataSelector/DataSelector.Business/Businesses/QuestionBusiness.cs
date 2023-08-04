using DataSelector.DataAccess;
using DataSelector.Model.Models;

namespace DataSelector.Business.Businesses;

public class QuestionBusiness : BaseBusiness<QuestionDocument>
{
    public QuestionBusiness(IBaseRepository<QuestionDocument> repository) : base(repository)
    {
    }
}

