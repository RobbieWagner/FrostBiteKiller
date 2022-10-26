using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script should be attached to player to force the camera to follow them as they move around
public class CameraGridFollow : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("hello");
        Camera.main.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -10);
    }
}
