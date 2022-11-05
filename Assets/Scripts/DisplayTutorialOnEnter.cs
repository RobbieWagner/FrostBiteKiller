using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTutorialOnEnter : MonoBehaviour
{

    private TutorialCanvas tutorialC;
    [SerializeField]
    private Tutorial runTutorial;

    private bool hasTutorialDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        tutorialC = GameObject.Find("TutorialCanvas").GetComponent<TutorialCanvas>();

        hasTutorialDisplayed = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!hasTutorialDisplayed && other.gameObject.tag.Equals("Player")) 
        {
            StartCoroutine(TimeTutorialDisplay());
            hasTutorialDisplayed = true;
        }
    }

    IEnumerator TimeTutorialDisplay()
    {
        tutorialC.tmproText.text = runTutorial.GetText();
        yield return new WaitForSeconds(5f);
        tutorialC.tmproText.text = "";

        StopCoroutine(TimeTutorialDisplay());
    }
}
