using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public AudioClip[] audioClip;
    private AudioSource audioSource;

    void Start()
    {
        sceneTransition = FindObjectOfType<SceneTransition>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        sceneTransition.StartFade("SampleScene");
        audioSource.PlayOneShot(audioClip[0]);
    }

    public void ExitGame()
    {
        audioSource.PlayOneShot(audioClip[1]);
        StartCoroutine(ExitAfterFade());
    }

    private IEnumerator ExitAfterFade()
    {
        yield return sceneTransition.StartFadeOnly();  // Aguarda o fade sem troca de cena
        Application.Quit();
    }
}
