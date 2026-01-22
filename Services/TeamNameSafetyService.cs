using Azure;
using Azure.AI.ContentSafety;

namespace DemoContentSafety.Services;

public class TeamNameSafetyService
{
    private readonly ContentSafetyClient _client;
    private readonly ILogger<TeamNameSafetyService> _logger;
    
    // しきい値: 0-2はOK、3以上はNG
    private const int SeverityThreshold = 3;

    public TeamNameSafetyService(ContentSafetyClient client, ILogger<TeamNameSafetyService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<ValidationResult> ValidateAsync(string teamName)
    {
        // Content Safety未設定の場合は常にOKとして扱う
        if (_client == null)
        {
            _logger.LogWarning("Content Safety client is not configured. Skipping validation.");
            return new ValidationResult
            {
                IsValid = true,
                Message = "登録しました"
            };
        }

        try
        {
            var request = new AnalyzeTextOptions(teamName);
            var response = await _client.AnalyzeTextAsync(request);
            
            var result = response.Value;
            
            // 各カテゴリの severity をチェック
            var categories = new Dictionary<string, int>
            {
                { "Hate", result.CategoriesAnalysis.FirstOrDefault(c => c.Category == TextCategory.Hate)?.Severity ?? 0 },
                { "Sexual", result.CategoriesAnalysis.FirstOrDefault(c => c.Category == TextCategory.Sexual)?.Severity ?? 0 },
                { "Violence", result.CategoriesAnalysis.FirstOrDefault(c => c.Category == TextCategory.Violence)?.Severity ?? 0 },
                { "SelfHarm", result.CategoriesAnalysis.FirstOrDefault(c => c.Category == TextCategory.SelfHarm)?.Severity ?? 0 }
            };

            // いずれかのカテゴリがしきい値以上の場合はNG
            bool isValid = categories.All(c => c.Value < SeverityThreshold);

            return new ValidationResult
            {
                IsValid = isValid,
                Categories = categories,
                Message = isValid 
                    ? "登録しました" 
                    : "不適切な可能性があるため登録できません"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Content Safety API呼び出しエラー");
            return new ValidationResult
            {
                IsValid = false,
                Message = "判定に失敗しました。時間をおいて再試行してください"
            };
        }
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
}
