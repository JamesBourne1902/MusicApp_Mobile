using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class Song_Search_Script : MonoBehaviour
{
    #region Search Variables

    [SerializeField]
    private GameObject button;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text filterText;

    private bool hidden = false;
    private string searchFilter = "name";

    private List<GameObject> filteredSongButtons = new List<GameObject>();

    #endregion

    #region available songs variables

    private string fileName = "FullSongList";
    private string path;
    private string[][] songInfo;

    #endregion

    #region main song buttons variables

    private List<GameObject> mainSongButtons = new List<GameObject>();
    [SerializeField]
    private GameObject scrollerContents;

    #endregion

    private void Start()
    {
        getAllChildObjects();
        fetchSongInfoArray();
    }

    private void OnDisable()
    {
        removeSearchedButtons();
        inputField.text = "tap to search".ToUpper();

        if (hidden)
        {
            changeMainVisibility();
        }
    }

    private void getAllChildObjects()
    {
        foreach (Transform child in scrollerContents.transform)
        {
            mainSongButtons.Add(child.gameObject);
        }
    }

    private void fetchSongInfoArray()
    {
        path = Path.Combine(Application.persistentDataPath, fileName);
        songInfo = AvailableSongs.FromJson(File.ReadAllText(path));
    }

    private void changeMainVisibility()
    {
        if (!hidden)
        {
            foreach (GameObject song in mainSongButtons)
            {
                song.SetActive(false);
            }
            hidden = true;
        }
        else
        {
            foreach (GameObject song in mainSongButtons)
            {
                song.SetActive(true);
            }
            hidden = false;
        }
    }

    private void removeSearchedButtons()
    {
        foreach (GameObject song in filteredSongButtons)
        {
            Destroy(song);
        }
        filteredSongButtons.Clear();
    }

    public void changeSearchFilter()
    {
        if (searchFilter == "name")
        {
            searchFilter = "genre";
            filterText.text = searchFilter.ToUpper();
        }
        else if (searchFilter == "genre")
        {
            searchFilter = "artist";
            filterText.text = searchFilter.ToUpper();
        }
        else
        {
            searchFilter = "name";
            filterText.text = searchFilter.ToUpper();
        }

        removeSearchedButtons();
        if (inputField.text.Length > 0 && inputField.text != "TAP TO SEARCH")
        {
            searchForSong();
        }
    }

    #region search functions

    private void searchForSong()
    {
        if (searchFilter == "name")
        {
            searchByName();
        }
        else if (searchFilter == "genre")
        {
            searchByGenre();
        }
        else
        {
            searchByArtist();
        }
    }

    private void searchByName()
    {
        for (int i = 0; i < songInfo.Length; i++)
        {
            if (inputField.text.Length <= songInfo[i][0].Length)
            {
                string temp = songInfo[i][0].Substring(0, inputField.text.Length);
                if (temp == inputField.text)
                {
                    GameObject temp2 = Master_Button_Script.instance.generatButton(scrollerContents, songInfo[i][0], Master_Button_Script.instance.addSongToPlaylist);
                    temp2.AddComponent<Alpha_Change_Script>();
                    filteredSongButtons.Add(temp2);
                }
            }
        }
    }

    private void searchByGenre()
    {
        for (int i = 0; i < songInfo.Length; i++)
        {
            if (inputField.text.Length <= songInfo[i][1].Length)
            {
                string temp = songInfo[i][1].Substring(0, inputField.text.Length);
                if (temp == inputField.text)
                {
                    GameObject temp2 = Master_Button_Script.instance.generatButton(scrollerContents, songInfo[i][0], Master_Button_Script.instance.addSongToPlaylist);
                    temp2.AddComponent<Alpha_Change_Script>();
                    filteredSongButtons.Add(temp2);
                }
            }
        }
    }

    private void searchByArtist()
    {
        for (int i = 0; i < songInfo.Length; i++)
        {
            for(int j = 2; j < songInfo[i].Length; j++)
            {
                if (inputField.text.Length <= songInfo[i][j].Length)
                {
                    string temp = songInfo[i][j].Substring(0, inputField.text.Length);
                    if (temp == inputField.text)
                    {
                        GameObject temp2 = Master_Button_Script.instance.generatButton(scrollerContents, songInfo[i][0], Master_Button_Script.instance.addSongToPlaylist);
                        temp2.AddComponent<Alpha_Change_Script>();
                        filteredSongButtons.Add(temp2);
                        break;
                    }
                }
            }
        }
    }

    #endregion

    public void capitalise()
    {
        inputField.text = inputField.text.ToUpper();
    }

    public void Search()
    {
        if (!hidden)
        {
            changeMainVisibility();
        }
        removeSearchedButtons();

        if (inputField.text.Length > 0)
        {
            searchForSong();
        }
        else
        {
            inputField.text = "tap to search";
            changeMainVisibility();
        }
    }

}
