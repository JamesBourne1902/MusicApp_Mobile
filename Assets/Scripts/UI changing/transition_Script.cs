using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transition_Script : MonoBehaviour
{
    public static transition_Script instance;

    #region main screens

    [SerializeField]
    private GameObject loadingScreen; // the opening screen. used for when downloading the song names
    [SerializeField]
    private GameObject homeScreen; // the home screen. to access the playlists or quit the app
    [SerializeField]
    private GameObject playlistsScreen; // the screen containing a button to access each of your saved playlists
    [SerializeField]
    private GameObject personalScreen; // the screen containing all the information about the playlist you selected
    [SerializeField]
    private GameObject songSelectionScreen; // the screen containing all the songs available to listen to
    [SerializeField]
    private GameObject playingScreen; // the screen shown when playing songs
    [SerializeField]
    private GameObject inputScreen; // the screen containing the text input for naming a new playlists
    [SerializeField]
    private GameObject renameScreen; // the screen containing the text input for renaming a playlist

    #endregion

    private GameObject currentScreen;
    private GameObject previousScreen;

    private void Awake()
    {
        instance = this;
    }

    public void quit()
    {
        Application.Quit();
    }

    #region Moving Forwards

    public void moveToHomeScreen()
    {
        loadingScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void moveToPlaylistsScreen()
    {

        personalScreen.SetActive(false);
        homeScreen.SetActive(false);
        playlistsScreen.SetActive(true);
    }

    public void moveToInputScreen()
    {
        playlistsScreen.SetActive(false);
        inputScreen.SetActive(true);
    }

    public void moveToRenameScreen()
    {
        personalScreen.SetActive(false);
        renameScreen.SetActive(true);
    }

    public void moveToPersonalScreen()
    {
        inputScreen.SetActive(false);
        renameScreen.SetActive(false);
        playlistsScreen.SetActive(false);
        personalScreen.SetActive(true);
    }

    public void moveToSongSelectionScreen()
    {
        personalScreen.SetActive(false);
        songSelectionScreen.SetActive(true);
    }

    public void moveToPlayingScreen()
    {
        personalScreen.SetActive(false);
        playingScreen.SetActive(true);
    }

    #endregion

    #region Moving Backwards

    public void returnToHomeScreen()
    {
        playlistsScreen.SetActive(false);
        homeScreen.SetActive(true);
    }

    public void stopNaming()
    {
        inputScreen.SetActive(false);
        playlistsScreen.SetActive(true);
    }

    public void returnToPlaylistsScreen()
    {
        personalScreen.SetActive(false);
        playlistsScreen.SetActive(true);
    }

    public void stopRenaming()
    {
        renameScreen.SetActive(false);
        personalScreen.SetActive(true);
    }

    public void returnToPersonalScreen()
    {
        songSelectionScreen.SetActive(false);
        playingScreen.SetActive(false);
        personalScreen.SetActive(true);
    }

    #endregion
}
