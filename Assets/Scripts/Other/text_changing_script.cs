using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text_changing_script : MonoBehaviour
{
    [SerializeField]
    private InputField InputField;
    [SerializeField]
    private Text errorMessage;

    private void OnEnable()
    {
        InputField.text = "Tap here to name".ToUpper();
        errorMessage.text = "";
    }
}
