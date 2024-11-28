using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Songlist_Manager_Script : MonoBehaviour
{
    public static Songlist_Manager_Script instance;

    #region manipulator variables

    public GitHubFileList git;
    AvailableSongs theList;

    #endregion

    #region checker variables

    public string[][] songInfo;

    private string fileName = "/FullSongList";
    public string path;

    #endregion

    #region maker Variables

    [SerializeField]
    private GameObject scroller;

    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        git = GameObject.FindGameObjectWithTag("Downloader").GetComponent<GitHubFileList>();
        path = Application.persistentDataPath + fileName;
    }

    #region song list manipulators

    // takes the list of songs downloaded from my git repository and splits them into a jagged array
    // format: [0]- song name, [1]- genre, [2+]- artist
    public void listSplitter()
    {
        theList = new AvailableSongs();
        theList.availableSongs = new string[git.fileList.Count][];

        for (int i = 0; i < git.fileList.Count; i++)
        {
            List<string> temp = nameSeparator(git.fileList[i]);
            theList.availableSongs[i] = new string[temp.Count];

            for (int j = 0; j < temp.Count; j++)
            {
                theList.availableSongs[i][j] = temp[j];
            }
        }

        checkSongChanges();
    }

    // separates the names of my github files into their components
    // name format: SONG NAME,GENRE,ARTIST,ARTIST.mp3
    // can have multiple artists but only one genre
    public List<string> nameSeparator(string fileName)
    {
        int index = -1;
        int startPoint = 0;

        List<int> list = new List<int>();
        List<string> names = new List<string>();

        do // the 'do' causes the code to execute at least once even if the while statement is false
        {
            index = fileName.IndexOf(',', index + 1); // starts the search from the letter after each ","

            if (index != -1)
            {
                list.Add(index);
            }

        } while (index != -1); // .IndexOf returns -1 if nothing is found which breaks out of the loop

        foreach (int i in list)
        {
            int change = i - startPoint;
            string temp = fileName.Substring(startPoint, change);
            startPoint = i + 1;

            names.Add(temp);
        }

        return names;
    }

    #endregion

    #region song list Checkers

    // Writes all the downloaded songs information to a JSON file.
    // if one already exists, it is overwritten.
    private void WriteToSongFile()
    {
        string json = theList.ToJson();
        File.WriteAllText(path, json);
        makeSongListButtons();
        transition_Script.instance.moveToHomeScreen();
    }

    // checks if the stored song file matches the songs available to stream
    private void checkSongChanges()
    {

        if (File.Exists(path))
        {
            // songInfo is a read of the currently stored file
            // theList is the newly downloaded file
            songInfo = AvailableSongs.FromJson(File.ReadAllText(path));

            if (songInfo.Length == theList.availableSongs.Length)
            {
                // downloaded file and stored file are the same
                // do nothing and move to home screen
                makeSongListButtons();
                transition_Script.instance.moveToHomeScreen();
            }
            else
            {
                // downloaded file is bigger that stored file
                // overwrite stored file
                WriteToSongFile();
            }
        }
        else
        {
            // no stored file found so a new one was made
            // new one will always be most up to date so no need to run additional checks
            WriteToSongFile();
        }
    }

    #endregion

    #region song list button makers

    private void makeSongListButtons()
    {
        Audio_Clip_Manager_Script.instance.setSongsReference(theList.availableSongs);

        for (int i = 0; i < theList.availableSongs.Length; i++)
        {
            GameObject temp =  Master_Button_Script.instance.generatButton(scroller, theList.availableSongs[i][0], Master_Button_Script.instance.addSongToPlaylist);
            temp.AddComponent<Alpha_Change_Script>();
        }
    }

    #endregion
}
