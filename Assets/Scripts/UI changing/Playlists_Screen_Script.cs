using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Playlists_Screen_Script : MonoBehaviour
{
    private string folderPath;
    private List<string> playlists = new List<string>();

    [SerializeField]
    private GameObject scroller;

    private void OnEnable()
    {
        DestroyAllButtons();
        makeNewButtons();
    }

    private void DestroyAllButtons()
    {
        Master_Button_Script.instance.destroyButtons(scroller);
    }

    // when the 'playlists screen' is opened this function is called
    // it makes a button for each playlist thats been saved
    // then makes a button to make a new playlist
    private void makeNewButtons()
    {
        CheckFolderExists();
        readAllPlaylistNames();

        foreach (string name in playlists)
        {
            Master_Button_Script.instance.generatButton(scroller, name, Master_Button_Script.instance.openPlaylist);
        }

        Master_Button_Script.instance.generatButton(scroller, "MAKE NEW PLAYLIST", Master_Button_Script.instance.OnMakeNewPlaylistClicked);
    }

    // checks whether or not the playlists folder exists
    // if it doesnt, one is made
    // this folder contains the JSON files of each playlist the user has made
    private void CheckFolderExists()
    {
        string folderName = "Playlists";
        folderPath = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    // reads the names of each JSON file in the playlists folder
    // adds those names to a list
    private void readAllPlaylistNames()
    {
        playlists.Clear();
        string[] fileNames = Directory.GetFiles(folderPath);

        foreach (string fileName in fileNames)
        {
            playlists.Add(Path.GetFileName(fileName));
        }
    }
}
