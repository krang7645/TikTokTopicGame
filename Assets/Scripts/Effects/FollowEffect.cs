using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FollowEffect : BaseEffect
{
    [Header("Follow Effect Settings")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI messageText;
    public Image userIcon;

    public void Initialize(string username, string profilePictureUrl)
    {
        usernameText.text = username;
        messageText.text = "フォローしました！";

        // プロフィール画像の読み込み
        if (!string.IsNullOrEmpty(profilePictureUrl))
        {
            StartCoroutine(LoadProfileImage(profilePictureUrl));
        }
    }

    private System.Collections.IEnumerator LoadProfileImage(string url)
    {
        using (var www = UnityEngine.Networking.UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                var texture = ((UnityEngine.Networking.DownloadHandlerTexture)www.downloadHandler).texture;
                userIcon.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
        }
    }
}
