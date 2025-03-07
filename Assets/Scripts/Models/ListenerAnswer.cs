using UnityEngine;

[System.Serializable]
public class ListenerAnswer
{
    public string uniqueId;
    public string username;
    public string nickname;
    public string profilePictureUrl;
    public string answer;

    public ListenerAnswer(string uniqueId, string username, string nickname, string profilePictureUrl, string answer)
    {
        this.uniqueId = uniqueId;
        this.username = username;
        this.nickname = nickname;
        this.profilePictureUrl = profilePictureUrl;
        this.answer = answer;
    }
}
