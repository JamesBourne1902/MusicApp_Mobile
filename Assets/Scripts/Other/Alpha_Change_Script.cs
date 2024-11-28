using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Alpha_Change_Script : MonoBehaviour
{
    private void OnEnable()
    {
        colorChange();
        gameObject.GetComponent<Button>().onClick.AddListener(colorChange);
    }

    private void colorChange()
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 0.4f;

        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        PlaylistContents list = PlaylistContents.FromJson(File.ReadAllText(path));

        if (list.contents.Contains(gameObject.GetComponentInChildren<Text>().text))
        {
            gameObject.GetComponent<Image>().color = color;
        }
    }
}
