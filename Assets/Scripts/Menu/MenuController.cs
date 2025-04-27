using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Navegação")]
    public GameObject firstSelectedButton;

    [Header("Botões para Escala")]
    public List<Selectable> buttons;
    public Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1f);
    public Vector3 normalScale = new Vector3(1f, 1f, 1f);

    [Header("Sons")]
    public AudioClip buttonChangeSound;  // Som a ser tocado
    private AudioSource audioSource;      // Fonte de áudio

    private GameObject previousSelectedButton;  // Para armazenar o botão anterior

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        audioSource = GetComponent<AudioSource>();  // Inicializa o AudioSource
        previousSelectedButton = firstSelectedButton;
    }

    void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        // Verifica se o botão selecionado mudou
        if (current != previousSelectedButton)
        {
            if (audioSource != null && buttonChangeSound != null)
            {
                audioSource.PlayOneShot(buttonChangeSound);  // Toca o som
            }

            previousSelectedButton = current;  // Atualiza o botão anterior
        }

        foreach (Selectable btn in buttons)
        {
            if (btn != null)
            {
                Transform t = btn.transform;
                if (current == btn.gameObject)
                    t.localScale = Vector3.Lerp(t.localScale, selectedScale, Time.deltaTime * 10f);
                else
                    t.localScale = Vector3.Lerp(t.localScale, normalScale, Time.deltaTime * 10f);
            }
        }
    }
}
