using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    static GameObject keyboardCanvasStatic;
    GameObject keyboardCanvas;
    public static bool allowChangePos = true;

    private void Awake()
    {
        Debug.Log("Debug 1");
        keyboardCanvas = gameObject.transform.Find("KeyboardCanvas").gameObject;
        keyboardCanvasStatic = keyboardCanvas;

        keyboardCanvasStatic.SetActive(false);
    }

    private void Update()
    {
        if (Authentication.loggedIn && allowChangePos)
        {
            keyboardCanvasStatic.transform.position = new Vector3(-0.44f, 1.229f, 2.024f);
            allowChangePos = false;
            // keyboardCanvas.transform.rotation = Quaternion.Euler(new Vector3(51.137f, 0, 0));
            // keyboardCanvas.transform.localScale = new Vector3(0.1426f, )
        }
    }

    public void SetInputField(TMP_InputField inputField)
    {
        Debug.Log("Inputfield: " + keyboardCanvasStatic.GetComponent<KeyboardInputSystem>().inputField);
        keyboardCanvasStatic.GetComponent<KeyboardInputSystem>().inputField = inputField;
    }

    public void TurnOnKeyboard()
    {
        keyboardCanvasStatic.SetActive(true);
    }

    public void TurnOffKeyboard()
    {
        keyboardCanvasStatic.SetActive(false);
    }
}
