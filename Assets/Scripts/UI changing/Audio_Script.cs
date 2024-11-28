using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Audio_Script : MonoBehaviour
{
    private List<string> playlist;

    private int index = -1;

    private float count = 0;
    private float songLength = 0;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text nameText;

    #region pause/unpause variables

    private bool playing = false;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private Image pauseImage;
    [SerializeField]
    private Sprite PauseSprite;
    [SerializeField]
    private Sprite PlaySprite;

    #endregion

    #region shuffle variables

    private bool shuffle = false;
    [SerializeField]
    private Text shuffleText;
    [SerializeField]
    private GameObject previousSongButton;

    private List<int> shuffleList = new List<int>();
    private int currentSongInt;
    private int shuffleLimit = 1;

    #endregion

    #region Volume variables

    [SerializeField]
    private Text volumeText;

    private float volume = 100;
    public AudioSource player;

    #endregion

    private void OnEnable()
    {
        title.text = Logic_Script.instance.currentPlaylist.ToUpper();
        fetchPlaylist();
        incrementIndex();
        StartCoroutine(DownloadAndplaySong());
    }

    private void OnDisable()
    {
        resetVariables();
    }

    private void Update()
    {
        if (playing && count < songLength)
        {
            count += Time.deltaTime;
        }
        else if (playing)
        {
            playing = false;
            nextSong();
        }
    }

    private IEnumerator DownloadAndplaySong()
    {
        string songName = playlist[index];
        currentSongInt = index;
        Audio_Clip_Manager_Script.instance.currentSong = null;
        Audio_Clip_Manager_Script.instance.nextSong = null;
        StartCoroutine(Audio_Clip_Manager_Script.instance.Download(songName));
        nameText.text = "Downloading song. please wait".ToUpper();

        while (true)
        {
            if (Audio_Clip_Manager_Script.instance.currentSong != null)
            {
                player.clip = Audio_Clip_Manager_Script.instance.currentSong;
                player.Play();

                prepVariables();

                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    #region utility functions
    private void incrementIndex()
    {
        if (!shuffle)
        {
            if (index == playlist.Count - 1)
            {
                index = 0;
            }
            else
            {
                index += 1;
            }
        }

        else
        {
            int temp = Random.Range(0, playlist.Count);

            if (!checkShuffleList(temp))
            {
                shuffleList.Add(temp);
                index = temp;
            }
            else
            {
                incrementIndex();
            }
        }
    }

    private void prepVariables()
    {
        songLength = Audio_Clip_Manager_Script.instance.currentSong.length;
        count = 0;
        playing = true;
        nameText.text = ("currently playing: \n\n" + playlist[index]).ToUpper();
        pauseButton.SetActive(true);
        pauseImage.sprite = PauseSprite;

        prepNextSong();
    }

    // sets the 'playlist' variable to the list of song names stored in the currently selected playlist
    private void fetchPlaylist()
    {
        string folderName = "Playlists";
        string path = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

        PlaylistContents list = PlaylistContents.FromJson(File.ReadAllText(path));
        playlist = new List<string>(list.contents);
    }

    private void resetVariables()
    {
        playing = false;
        index = -1;
        songLength = 0;
        shuffleLimit = 1;
        playlist.Clear();
        shuffleList.Clear();
        Audio_Clip_Manager_Script.instance.currentSong = null;
        Audio_Clip_Manager_Script.instance.nextSong = null;
        pauseImage.sprite = PauseSprite;

        if (shuffle)
        {
            enableShuffle();
        }
    }

    private bool checkShuffleList(int index)
    {
        for (int i = 0; i < shuffleLimit; i++)
        {
            if (shuffleList[shuffleList.Count - 1 - i] == index)
            {
                return true;
            }

            if (playlist.Count == shuffleLimit)
            {
                shuffleLimit = 1;
                return true;
            }
        }

        shuffleLimit += 1;

        return false;
    }

    private void prepNextSong()
    {
        incrementIndex();
        string songName = playlist[index];
        StartCoroutine(Audio_Clip_Manager_Script.instance.Download(songName));
    }

    private void stopCurrentSong()
    {
        pauseButton.SetActive(false);
        playing = false;
        player.Pause();
    }

    private void decrementIndex()
    {
        if (index == 0)
        {
            index = playlist.Count - 1;
        }
        else
        {
            index -= 1;
        }
    }

    #endregion

    #region Player Button Functions
    public void nextSong()
    {
        if (Audio_Clip_Manager_Script.instance.nextSong != null)
        {
            Audio_Clip_Manager_Script.instance.currentSong = Audio_Clip_Manager_Script.instance.nextSong;
            Audio_Clip_Manager_Script.instance.nextSong = null;
            player.clip = Audio_Clip_Manager_Script.instance.currentSong;
            player.Play();
            currentSongInt = index;

            prepVariables();
        }
        else if (playing)
        {
            stopCurrentSong();
            Audio_Clip_Manager_Script.instance.stopDownloads();
            StartCoroutine(DownloadAndplaySong());
        }
        else
        {
            stopCurrentSong();
            Audio_Clip_Manager_Script.instance.stopDownloads();
            StopAllCoroutines();
            incrementIndex();
            StartCoroutine(DownloadAndplaySong());

        }
    }

    public void pause()
    {
        if (pauseImage.sprite == PauseSprite)
        {
            pauseImage.sprite = PlaySprite;
            playing = false;
            player.Pause();
        }
        else
        {
            pauseImage.sprite = PauseSprite;
            playing = true;
            player.UnPause();
        }
    }

    public void enableShuffle()
    {
        if (!shuffle)
        {
            shuffle = true;
            shuffleText.text = "shuffle: on".ToUpper();
            previousSongButton.SetActive(false);

            if (Audio_Clip_Manager_Script.instance.nextSong != null)
            {
                Audio_Clip_Manager_Script.instance.nextSong = null;
                shuffleList.Add(currentSongInt);
                prepNextSong();
            }
            else if (playing)
            {
                Audio_Clip_Manager_Script.instance.stopDownloads();
                StopAllCoroutines();
                shuffleList.Add(currentSongInt);
                prepNextSong();
            }
            else
            {
                shuffleList.Add(index);
            }
        }
        else
        {
            shuffle = false;
            shuffleText.text = "shuffle: off".ToUpper();
            shuffleList.Clear();
            previousSongButton.SetActive(true);
            shuffleLimit = 1;

            if (Audio_Clip_Manager_Script.instance.nextSong != null)
            {
                Audio_Clip_Manager_Script.instance.nextSong = null;
                index = currentSongInt;
                prepNextSong();
            }
            else if (playing)
            {
                Audio_Clip_Manager_Script.instance.stopDownloads();
                StopAllCoroutines();
                index = currentSongInt;
                prepNextSong();
            }
        }
    }

    public void previousSong()
    {
        StopAllCoroutines();
        Audio_Clip_Manager_Script.instance.stopDownloads();
        stopCurrentSong();
        index = currentSongInt;
        decrementIndex();
        StartCoroutine(DownloadAndplaySong());
    }

    public void changeVolume(int change)
    {
        volume += change;

        if (volume < 0)
        {
            volume = 0;
        }
        else if (volume > 100)
        {
            volume = 100;
        }

        player.volume = (volume/100);
        volumeText.text = volume.ToString() + "%";
    }

    #endregion
}
