using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Class keeps track of variables called by multiple classes

    [HideInInspector]
    public bool isReadingDialogue;

    [HideInInspector]
    public bool canInteractWithObjects;

    void Start()
    {
        isReadingDialogue = false;
        canInteractWithObjects = true;
    }
}
