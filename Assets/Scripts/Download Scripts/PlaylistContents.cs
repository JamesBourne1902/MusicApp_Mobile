using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaylistContents
{
    public List<string> contents = new List<string>();

    public string toJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static PlaylistContents FromJson(string json)
    {
        return JsonUtility.FromJson<PlaylistContents>(json);
    }
}
