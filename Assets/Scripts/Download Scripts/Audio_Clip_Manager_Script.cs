using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class Audio_Clip_Manager_Script : MonoBehaviour
{
    public static Audio_Clip_Manager_Script instance;

    private string[][] songInfo;
    private AudioClip audioClip;
    public AudioClip currentSong;
    public AudioClip nextSong;

    private void Awake()
    {
        instance = this;
    }

    public void setSongsReference(string[][] temp)
    {
        songInfo = temp;
    }

    public IEnumerator Download(string songName)
    {
        string dropboxPublicLink = "https://raw.githubusercontent.com/Beast-Bourne/Music_Storage/main/";
        string properName = nameCorrector(songName);
        string link = dropboxPublicLink + properName + ".mp3";

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(link, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            audioClip = DownloadHandlerAudioClip.GetContent(www);

            if (currentSong == null)
            {
                currentSong = audioClip;
            }
            else
            {
                nextSong = audioClip;
            }
        }
    }

    private string nameCorrector(string songName)
    {
        string fullName = "";
        int index = rowFinder(songName);

        for (int i = 0; i < songInfo[index].Length; i++)
        {
            fullName += songInfo[index][i] + ",";
        }

        return fullName;
    }

    private int rowFinder(string name)
    {
        int row = 0;
        while (true)
        {
            if (Array.Exists(songInfo[row], element => element == name))
            {
                return row;
            }
            else
            {
                row += 1;
                continue;
            }
        }
    }

    public void stopDownloads()
    {
        StopAllCoroutines();
    }
}
