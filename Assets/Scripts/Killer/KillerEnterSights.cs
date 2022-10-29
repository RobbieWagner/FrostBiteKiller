using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerEnterSights : MonoBehaviour
{
    [SerializeField]
    private Killer killer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            killer.inSights = true;
        }
    }
}
