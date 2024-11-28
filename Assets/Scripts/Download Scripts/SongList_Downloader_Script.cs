using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


// DONT FUCK WITH THIS CODE.
// NO CLUE HOW IT WORKS IT JUST DOES.
// Outputs a list of file names from my github repository

public class GitHubFileList : MonoBehaviour
{
    public List<string> fileList = new List<string>();
    private string token1 = "ghp_XUVpNkWHuJYqf";
    private string token2 = "jLMMFMP0HEqWMMaDS2pU3jp";

    private void Start()
    {
        StartCoroutine(GetFileList());
    }

    IEnumerator GetFileList()
    {
        string apiUrl = $"https://api.github.com/repos/Beast-Bourne/Music_Storage/contents/";

        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        request.SetRequestHeader("Authorization", "token " + token1 + token2);
        request.SetRequestHeader("User-Agent", "UnityApp");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error: {request.error}");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;

            // Parse the JSON response
            GitHubContent[] contents = JsonHelper.FromJson<GitHubContent>(jsonResponse);

            // Extract file names
            foreach (var content in contents)
            {
                fileList.Add(content.name.Replace(".mp3", "").ToUpper());
            }
        }

        Songlist_Manager_Script.instance.listSplitter();
    }
}

[System.Serializable]
public class GitHubContent
{
    public string name;
    // Add other properties if needed
}

// Helper class for JSON array deserialization
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
