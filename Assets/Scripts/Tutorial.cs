using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private string tutorialText;
    [HideInInspector]
    public bool wasDisplayed;

    void Start()
    {
        wasDisplayed = false;
    }
    
    public string GetText()
    {
        wasDisplayed = true;
        return tutorialText;
    }
}
