using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerEnterSights : MonoBehaviour
{
    [SerializeField]
    private Killer killer;

    [SerializeField]
    private AudioSource killerChaseMusic;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            killer.inSights = true;

            StartCoroutine(PlayChaseMusic());
        }
    }

    IEnumerator PlayChaseMusic()
    {
        while(!killer.chasing) yield return null;

        killerChaseMusic.Play();
        StopCoroutine(PlayChaseMusic());
    }
}
