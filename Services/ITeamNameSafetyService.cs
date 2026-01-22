namespace DemoContentSafety.Services;

public interface ITeamNameSafetyService
{
    Task<ValidationResult> ValidateAsync(string teamName);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, int> Categories { get; set; } = new();
    
    public string GetCategoriesSummary()
    {
        if (Categories.Count == 0)
            return string.Empty;
            
        return string.Join(", ", Categories.Select(c => $"{c.Key}:{c.Value}"));
    }
}
