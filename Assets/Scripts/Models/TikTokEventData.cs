using UnityEngine;

[System.Serializable]
public class CommentData
{
    public string uniqueId;
    public string userId;
    public string nickname;
    public string profilePictureUrl;
    public string comment;
    public long timestamp;
}

[System.Serializable]
public class LikeData
{
    public string userId;
    public string nickname;
    public string profilePictureUrl;
    public int likeCount;
    public long timestamp;
}

[System.Serializable]
public class GiftData
{
    public string userId;
    public string nickname;
    public string profilePictureUrl;
    public string giftId;
    public string giftName;
    public int giftCount;
    public int diamondCount;
    public long timestamp;
}

[System.Serializable]
public class FollowData
{
    public string userId;
    public string nickname;
    public string profilePictureUrl;
    public long timestamp;
}
