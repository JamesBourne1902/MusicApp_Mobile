using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note_Movement_Script : MonoBehaviour
{
    private float coordChange;

    [SerializeField]
    private Vector2 origin;
    [SerializeField]
    private bool changeX;

    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1);
        resetNote();
    }

    // resests the notes position when it goes off screen
    void Update()
    {
        if (transform.position.y >= 5.5 || transform.position.x >= 3.5)
        {
            resetNote();
        }
    }

    // returns the notes to the origin (off screen to the bottom left)
    // alternates between starting further along the horizontal or vertical
    private void resetNote()
    {
        if (changeX)
        {
            coordChange = Random.Range(0, 4.5f);
            transform.position = origin + new Vector2(coordChange, 0);
            changeX = false;
        }
        else
        {
            coordChange = Random.Range(0, 8.5f);
            transform.position = origin + new Vector2(0, coordChange);
            changeX = true;
        }
    }
}
