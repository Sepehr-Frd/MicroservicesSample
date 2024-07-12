namespace ToDoListManager.Common.Constants;

public static class RegexPatternConstants
{
    public const string UsernamePattern = @"^(?![\d_])[\w\d_]{8,16}$";
    public const string PasswordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[.,!@#$%^&*()_+])[A-Za-z\d.,!@#$%^&*()_+]{8,20}$";
}