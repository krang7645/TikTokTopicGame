using UnityEngine;
using System;

public class TikTokManager : MonoBehaviour
{
    public static TikTokManager Instance { get; private set; }

    private SVManager svManager;
    private bool isConnected;

    public event Action<bool> OnConnectionStatusChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSVManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSVManager()
    {
        svManager = FindObjectOfType<SVManager>();
        if (svManager == null)
        {
            Debug.LogError("SVManager not found in the scene!");
            return;
        }

        // SVManagerのイベントハンドラを設定
        svManager.OnConnected += HandleConnected;
        svManager.OnDisconnected += HandleDisconnected;
        svManager.OnComment += HandleComment;
        svManager.OnLike += HandleLike;
        svManager.OnGift += HandleGift;
        svManager.OnFollow += HandleFollow;
    }

    private void HandleConnected()
    {
        isConnected = true;
        OnConnectionStatusChanged?.Invoke(true);
        Debug.Log("TikTok Live connected!");
    }

    private void HandleDisconnected()
    {
        isConnected = false;
        OnConnectionStatusChanged?.Invoke(false);
        Debug.Log("TikTok Live disconnected!");
    }

    private void HandleComment(CommentData data)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ProcessListenerAnswer(
                data.uniqueId,
                data.userId,
                data.nickname,
                data.profilePictureUrl,
                data.comment
            );
        }
    }

    private void HandleLike(LikeData data)
    {
        UIManager.Instance.ShowLikeEffect(data);
    }

    private void HandleGift(GiftData data)
    {
        UIManager.Instance.ShowGiftEffect(data);
    }

    private void HandleFollow(FollowData data)
    {
        UIManager.Instance.ShowFollowEffect(data);
    }

    public void StartServer()
    {
        if (svManager != null)
        {
            svManager.StartServer();
        }
    }

    public void StopServer()
    {
        if (svManager != null)
        {
            svManager.StopServer();
        }
    }

    public bool IsConnected()
    {
        return isConnected;
    }
}
