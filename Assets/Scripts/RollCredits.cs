using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RollCredits : MonoBehaviour
{

    [SerializeField]
    private TextAsset JsonText;

    [System.Serializable]
    public class CreditLine
    {
        public Credit[] creditLine;
    }

    [System.Serializable]
    public class Credit
    {
        public string credit;
    }

    [SerializeField]
    private TextMeshProUGUI leadCredits;
    [SerializeField]
    private TextMeshProUGUI resourcesCredits;

    [SerializeField]
    CreditLine gameCredits;

    // Start is called before the first frame update
    void Start()
    {
        gameCredits = JsonUtility.FromJson<CreditLine>(JsonText.text);

        StartCoroutine(RollTheCredits());
    }

    IEnumerator RollTheCredits()
    {
        leadCredits.enabled = true;
        resourcesCredits.enabled = false;
        yield return new WaitForSeconds(3f);
        leadCredits.enabled = false;
        resourcesCredits.enabled = true;

        foreach(Credit credit in gameCredits.creditLine)
        {
            resourcesCredits.text = credit.credit;
            yield return new WaitForSeconds(3f);
        }

        resourcesCredits.text = "";
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("Menu");

        StopCoroutine(RollTheCredits());
    }
}
