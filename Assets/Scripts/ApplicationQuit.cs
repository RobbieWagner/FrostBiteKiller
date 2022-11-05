using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuit : MonoBehaviour
{
    // Allows the user to quit the game
    public void QuitApplication()
    {
        Application.Quit();
    }

    private void Update() 
    {
        if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Q)) QuitApplication();
    }
}
