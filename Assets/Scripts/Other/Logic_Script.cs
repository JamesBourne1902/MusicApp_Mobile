using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic_Script : MonoBehaviour
{
    public static Logic_Script instance;

    public string currentPlaylist;

    private void Awake()
    {
        instance = this;
    }
}
