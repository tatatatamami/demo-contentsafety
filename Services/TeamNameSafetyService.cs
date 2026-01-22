using Azure;
using Azure.AI.ContentSafety;

namespace DemoContentSafety.Services;

public class TeamNameSafetyService : ITeamNameSafetyService
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
        try
        {
            var request = new AnalyzeTextOptions(teamName);
            var response = await _client.AnalyzeTextAsync(request);
            
            var result = response.Value;
            
            // 各カテゴリの severity をチェックし、しきい値判定も同時に行う
            var categories = new Dictionary<string, int>();
            bool isValid = true;
            
            foreach (var category in result.CategoriesAnalysis)
            {
                string categoryName;
                if (category.Category == TextCategory.Hate)
                {
                    categoryName = "Hate";
                }
                else if (category.Category == TextCategory.Sexual)
                {
                    categoryName = "Sexual";
                }
                else if (category.Category == TextCategory.Violence)
                {
                    categoryName = "Violence";
                }
                else if (category.Category == TextCategory.SelfHarm)
                {
                    categoryName = "SelfHarm";
                }
                else
                {
                    categoryName = category.Category.ToString();
                }
                
                var severity = category.Severity ?? 0;
                categories[categoryName] = severity;
                
                // しきい値チェック
                if (severity >= SeverityThreshold)
                {
                    isValid = false;
                }
            }

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
}
