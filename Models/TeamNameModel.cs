using System.ComponentModel.DataAnnotations;

namespace DemoContentSafety.Models;

public class TeamNameModel
{
    [Required(ErrorMessage = "チーム名を入力してください")]
    [StringLength(23, ErrorMessage = "チーム名は23文字以内で入力してください")]
    public string TeamName { get; set; } = string.Empty;
}
