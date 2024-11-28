using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot_Script : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> dots;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private int index = 2;

    private void OnEnable()
    {
        changeDot();
    }

    private void OnDisable()
    {
        StopCoroutine(dotMoveUp());
        StopCoroutine(dotMoveDown());
    }

    private IEnumerator dotMoveUp()
    {
        float timer = 0;
        float duration = 0.5f;

        while (timer < duration)
        {
            dots[index].transform.localPosition = Vector2.Lerp(startPosition, endPosition, timer/duration);
            timer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(dotMoveDown());
    }

    private IEnumerator dotMoveDown()
    {
        float timer = 0;
        float duration = 0.5f;

        while (timer < duration)
        {
            dots[index].transform.localPosition = Vector2.Lerp(endPosition, startPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        changeDot();
    }

    private void changeDot()
    {
        if (index < 2)
        {
            index += 1;
        }
        else
        {
            index = 0;
        }

        startPosition = dots[index].transform.localPosition;
        endPosition = startPosition + new Vector2(0, 100);

        StartCoroutine(dotMoveUp());
    }
}
