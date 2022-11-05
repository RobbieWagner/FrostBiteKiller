using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwap : MonoBehaviour
{
    // Swaps two canvases
    [SerializeField]
    private Canvas thisCanvas;
    [SerializeField]
    private Canvas otherCanvas;
    [SerializeField]
    private bool swanpsCanvasesSlowly = false;

    [SerializeField]
    private Image uiImage;
    [SerializeField]
    private GameObject screenCoverCanvas;

    [SerializeField]
    private bool hasAssociatedMusic;
    [SerializeField]
    private AudioSource associatedMusic;

    public void SwapCanvases()
    {
        if(swanpsCanvasesSlowly) StartCoroutine(SwapCanvasesSlowly());
        else
        {
            thisCanvas.enabled = false;
            otherCanvas.enabled = true;
        }
    }

    public IEnumerator SwapCanvasesSlowly()
    {
        screenCoverCanvas.gameObject.SetActive(true);

        while(uiImage.color.a < 1f)
        {
            uiImage.color = new Color(uiImage.color.r, uiImage.color.g, uiImage.color.b, uiImage.color.a + .01f);
            if(hasAssociatedMusic && associatedMusic.volume != 0) associatedMusic.volume -= 0.03f;
            yield return new WaitForSeconds(.025f);
        }
        thisCanvas.enabled = false;
        otherCanvas.enabled = true;
        screenCoverCanvas.SetActive(false);
        StopCoroutine(SwapCanvasesSlowly());
    }
}
