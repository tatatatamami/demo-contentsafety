namespace DemoContentSafety.Services;

public class NoOpTeamNameSafetyService : ITeamNameSafetyService
{
    private readonly ILogger<NoOpTeamNameSafetyService> _logger;

    public NoOpTeamNameSafetyService(ILogger<NoOpTeamNameSafetyService> logger)
    {
        _logger = logger;
        _logger.LogWarning("Azure Content Safety is not configured. Validation will be bypassed.");
    }

    public Task<ValidationResult> ValidateAsync(string teamName)
    {
        return Task.FromResult(new ValidationResult
        {
            IsValid = true,
            Message = "登録しました"
        });
    }
}
