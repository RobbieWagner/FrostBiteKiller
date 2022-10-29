using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerExitSights : MonoBehaviour
{
    [SerializeField]
    private Killer killer;

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            killer.inSights = false;
        }
    }
}
