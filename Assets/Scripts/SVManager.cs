using UnityEngine;
using System;

public class SVManager : MonoBehaviour
{
    // イベントデリゲート
    public event Action OnConnected;
    public event Action OnDisconnected;
    public event Action<CommentData> OnComment;
    public event Action<LikeData> OnLike;
    public event Action<GiftData> OnGift;
    public event Action<FollowData> OnFollow;

    // サーバーの状態
    private bool isServerRunning = false;

    // サーバーを開始
    public void StartServer()
    {
        if (!isServerRunning)
        {
            // ここにサーバー開始の実装を追加
            isServerRunning = true;
            OnConnected?.Invoke();
        }
    }

    // サーバーを停止
    public void StopServer()
    {
        if (isServerRunning)
        {
            // ここにサーバー停止の実装を追加
            isServerRunning = false;
            OnDisconnected?.Invoke();
        }
    }

    // コメントを受信したときの処理
    private void HandleComment(string json)
    {
        var data = JsonUtility.FromJson<CommentData>(json);
        OnComment?.Invoke(data);
    }

    // いいねを受信したときの処理
    private void HandleLike(string json)
    {
        var data = JsonUtility.FromJson<LikeData>(json);
        OnLike?.Invoke(data);
    }

    // ギフトを受信したときの処理
    private void HandleGift(string json)
    {
        var data = JsonUtility.FromJson<GiftData>(json);
        OnGift?.Invoke(data);
    }

    // フォローを受信したときの処理
    private void HandleFollow(string json)
    {
        var data = JsonUtility.FromJson<FollowData>(json);
        OnFollow?.Invoke(data);
    }

    private void OnDestroy()
    {
        if (isServerRunning)
        {
            StopServer();
        }
    }
}
