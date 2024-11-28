using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class input_Script : MonoBehaviour
{
    [SerializeField]
    private InputField newNameField;
    [SerializeField]
    private InputField renameField;
    [SerializeField]
    private Text errorMessage;
    [SerializeField]
    private Text renameErrorMessage;

    private readonly string invalidCharacters = @"[<>:""/\\|?*]";
    private readonly string reservedNames = @"^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])$";


    // this function runs when a new playlist is made and named
    // it creates a new file with the name of the playlist
    // then moves on to the 'Personal Screen'
    public void onNewPlaylistNameSubmitted()
    {
        if (!isInvalidName(newNameField.text))
        {
            string folderName = "Playlists";
            string playlistName = newNameField.text;
            string path = Path.Combine(Application.persistentDataPath, folderName, playlistName);

            PlaylistContents list = new PlaylistContents();

            try // if this successfully reads a file with the name input, it gives an error saying file with that name already exists
            {
                list = PlaylistContents.FromJson(File.ReadAllText(path));
                errorMessage.text = ("Invalid name \n\n Playlist with this name already exists").ToUpper();
            }
            catch // if no file is read, a new one with the given name is created.
            {
                string json = list.toJson();
                File.WriteAllText(path, json);
                Logic_Script.instance.currentPlaylist = playlistName;
                transition_Script.instance.moveToPersonalScreen();
            }
        }
    }

    public void onPlaylistRenamed()
    {
        if (!isInvalidName(renameField.text))
        {
            string folderName = "Playlists";
            string playlistName = renameField.text;
            string newPath = Path.Combine(Application.persistentDataPath, folderName, playlistName);
            string oldPath = Path.Combine(Application.persistentDataPath, folderName, Logic_Script.instance.currentPlaylist);

            try
            {
                File.Move(oldPath, newPath);
                Logic_Script.instance.currentPlaylist = playlistName;
                transition_Script.instance.moveToPersonalScreen();
            }
            catch
            {
                renameErrorMessage.text = ("Invalid name \n\n Playlist with this name already exists").ToUpper();
            }
        }
    }

    private bool isInvalidName(string text)
    {
        // checks for the invalid characters
        if (Regex.IsMatch(text, invalidCharacters))
        {
            errorMessage.text = ("Invalid Name. \n\n Name can not contain @[\"<>:\"\"/\\\\|?*] ").ToUpper();
            renameErrorMessage.text = errorMessage.text;
            return true;
        }
        // checks for the system reserved file names
        if (Regex.IsMatch(text, reservedNames, RegexOptions.IgnoreCase))
        {
            errorMessage.text = ("Invalid Name. \n\n Name is reserved by the system").ToUpper();
            renameErrorMessage.text = errorMessage.text;
            return true;
        }
        // checks for whitespace
        if (text.StartsWith(" ") || text.EndsWith(" ") || text.StartsWith(".") || text.EndsWith("."))
        {
            errorMessage.text = ("invalid name. \n\n Name starts/ends with whitespace").ToUpper();
            renameErrorMessage.text = errorMessage.text;
            return true;
        }
        // checks for the length of the file name
        if (text.Length > 20)
        {
            errorMessage.text = ("invalid name. \n\n name is limited to 20 characters").ToUpper();
            renameErrorMessage.text = errorMessage.text;
            return true;
        }
        // checks for whitespace only file names
        if (string.IsNullOrEmpty(text))
        {
            errorMessage.text = ("invalid name. \n\n name only contains whitespace").ToUpper();
            renameErrorMessage.text = errorMessage.text;
            return true;
        }

        return false;

    }

    public void onCharacterEntered(InputField temp)
    {
        temp.text = temp.text.ToUpper();
    }
}
