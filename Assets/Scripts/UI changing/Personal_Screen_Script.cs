using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Personal_Screen_Script : MonoBehaviour
{
    public static Personal_Screen_Script instance;

    [SerializeField]
    private GameObject scroller;
    [SerializeField]
    private Text title;
    

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        changeTitle();
        refreshPage();
    }

    private void makeSongButtons()
    {
        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        PlaylistContents list = PlaylistContents.FromJson(File.ReadAllText(path));
        
        foreach (string name in list.contents)
        {
            Master_Button_Script.instance.generatButton(scroller, name, Master_Button_Script.instance.removeSong);
        }

        Master_Button_Script.instance.generatButton(scroller, "ADD NEW SONG", Master_Button_Script.instance.addNewSong);

    }

    private void destroySongButtons()
    {
        Master_Button_Script.instance.destroyButtons(scroller);
    }

    private void changeTitle()
    {
        title.text = Logic_Script.instance.currentPlaylist;
    }

    public void refreshPage()
    {
        destroySongButtons();
        makeSongButtons();
    }

    public void deletePlaylist()
    {
        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        File.Delete(path);
        transition_Script.instance.moveToPlaylistsScreen();
    }
}
