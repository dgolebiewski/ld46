using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string[] introductionSegments;
    [SerializeField]
    private TMP_Text introductionText;
    [SerializeField]
    private Animator introductionAnimator;
    [SerializeField]
    private float segmentTime = 4.5f;

    private bool introductionInProgress = false;
    private int currentIntroSegment = 0;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if(introductionInProgress && Input.GetButtonDown("Interact"))
            LoadGame();
    }

    public void Play()
    {
        introductionInProgress = true;
        StartCoroutine(NextIntroSegment());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    private IEnumerator NextIntroSegment()
    {
        introductionText.text = introductionSegments[currentIntroSegment];
        currentIntroSegment++;

        introductionAnimator.SetTrigger("show_text");

        yield return new WaitForSeconds(segmentTime);

        if(currentIntroSegment < introductionSegments.Length)
            StartCoroutine(NextIntroSegment());
        else
            LoadGame();
    }
}
