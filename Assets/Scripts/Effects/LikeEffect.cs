using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LikeEffect : BaseEffect
{
    [Header("Like Effect Settings")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI likeCountText;
    public Image heartIcon;

    public void Initialize(string username, int likeCount)
    {
        usernameText.text = username;
        likeCountText.text = $"x{likeCount}";
    }

    protected override void Awake()
    {
        base.Awake();
        // ハートアイコンの初期設定
        heartIcon.color = Color.red;
    }
}
