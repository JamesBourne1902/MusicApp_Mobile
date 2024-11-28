using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Master_Button_Script : MonoBehaviour
{
    public static Master_Button_Script instance;

    [SerializeField]
    private GameObject blankButton;

    private void Awake()
    {
        instance = this;
    }

    // makes a button as a child object of the 'parent' parameter
    // sets the text of that button to the 'text' parameter
    // adds a listener action to that button with the 'text' parameter as that action's paramter
    public GameObject generatButton(GameObject parent, string Text, Action<string> action)
    {
        GameObject temp = Instantiate(blankButton, parent.transform);
        temp.GetComponentInChildren<Text>().text = Text.ToUpper();
        temp.GetComponent<Button>().onClick.AddListener(() => action(Text.ToUpper()));

        return temp;
    }

    // destroys all the child objects of the 'parent' parameter
    // used to wipe out all the buttons in the scrollers to refresh them when playlists change
    public void destroyButtons(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // when the 'Make New Playlist' button is clicked
    // this function is called
    // it opens the input screen to name the playlist
    public void OnMakeNewPlaylistClicked(string temp)
    {
        transition_Script.instance.moveToInputScreen();
    }

    // when one of the playlists is clicked
    // it opens the 'personal screen'
    // the name of the playlist is passed through as a paramater
    public void openPlaylist(string name)
    {
        Logic_Script.instance.currentPlaylist = name;
        transition_Script.instance.moveToPersonalScreen();
    }

    // when the 'add new song' button is clicked
    // opens the 'song selection screen'
    public void addNewSong(string name)
    {
        transition_Script.instance.moveToSongSelectionScreen();
    }

    // when one of the songs buttons is clicked in the 'personal screen'
    // removes the song from the playlist and refreshes the buttons to remove it from the screen
    public void removeSong(string name)
    {
        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        PlaylistContents list = PlaylistContents.FromJson(File.ReadAllText(path));
        list.contents.Remove(name);

        string json = list.toJson();
        File.WriteAllText(path, json);

        Personal_Screen_Script.instance.refreshPage();
    }

    // when one of the song buttons is clicked in the 'song selection screen'
    // adds that song to the current playlist
    public void addSongToPlaylist(string name)
    {
        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        PlaylistContents list = PlaylistContents.FromJson(File.ReadAllText(path));

        if(!list.contents.Contains(name))
        {
            list.contents.Add(name);
            string json = list.toJson();
            File.WriteAllText(path, json);
        }
    }
}
