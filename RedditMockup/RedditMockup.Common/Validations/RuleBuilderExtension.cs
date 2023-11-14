using FluentValidation;
using System.Text.RegularExpressions;

namespace RedditMockup.Common.Validations;

public static class RuleBuilderExtension
{
    public static IRuleBuilderOptions<T, Guid> MustBeGuid<T>(this IRuleBuilderInitial<T, Guid> ruleBuilderInitial)
    {

        return ruleBuilderInitial.NotEmpty().Must(BeValidGuid);

        static bool BeValidGuid(Guid validatingGuid)
        {
            var guidString = validatingGuid.ToString();

            const string guidPattern = "^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{4}-[0-9A-Fa-f]{12}$";

            return Regex.IsMatch(guidString, guidPattern);
        }
    }
}