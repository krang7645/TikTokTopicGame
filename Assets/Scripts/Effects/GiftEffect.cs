using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiftEffect : BaseEffect
{
    [Header("Gift Effect Settings")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI giftNameText;
    public Image giftIcon;

    public void Initialize(string username, string giftName, Sprite giftSprite = null)
    {
        usernameText.text = username;
        giftNameText.text = giftName;

        if (giftSprite != null)
            giftIcon.sprite = giftSprite;
    }
}
