using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject aboutTab;
    public GameObject settingsTab;
    public GameObject escapeTab; // Reference to the Escape tab (Resume Tab)
    public AudioClip startSound;
    private AudioSource audioSource;
    public Slider volumeSlider; // Reference to the slider
    public float fadeSpeed = 20.0f; // Speed at which the panels fade out

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Initialize the volume to match the slider value at the start
        audioSource.volume = volumeSlider.value;

        // Add a listener to the slider to update the volume when it changes
        volumeSlider.onValueChanged.AddListener(delegate { OnVolumeChange(); });

        // Ensure the Escape Tab is inactive at the start
        escapeTab.SetActive(false);
        // Ensure the About Tab is inactive at the start
        aboutTab.SetActive(false);
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escapeTab.activeSelf)
            {
                // Activate and reset alpha of Escape Tab when Escape is pressed
                escapeTab.SetActive(true);
                ResetAlpha(escapeTab);
            }
            else
            {
                // Fade out the Escape Tab if it's already active
                StartCoroutine(FadeOut(escapeTab));
            }

            // Deactivate aboutTab if it's active
            if (aboutTab.activeSelf)
            {
                StartCoroutine(FadeOut(aboutTab));
            }
            // Deactivate settingsTab if it's active
            if (settingsTab.activeSelf)
            {
                StartCoroutine(FadeOut(settingsTab));
            }
        }

        // Check if the right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            // Deactivate aboutTab if it's active
            if (aboutTab.activeSelf)
            {
                StartCoroutine(FadeOut(aboutTab));
            }
            // Deactivate settingsTab if it's active
            if (settingsTab.activeSelf)
            {
                StartCoroutine(FadeOut(settingsTab));
            }
            // Deactivate escapeTab if it's active
            if (escapeTab.activeSelf)
            {
                StartCoroutine(FadeOut(escapeTab));
            }
        }
    }

    public void SettingsButton()
    {
        if (!settingsTab.activeSelf)
        {
            settingsTab.SetActive(true);
            ResetAlpha(settingsTab);
        }
        else
        {
            StartCoroutine(FadeOut(settingsTab));
        }
    }

    public void AboutButton()
    {
        if (!aboutTab.activeSelf)
        {
            aboutTab.SetActive(true);
            ResetAlpha(aboutTab);
        }
        else
        {
            StartCoroutine(FadeOut(aboutTab));
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void OnVolumeChange()
    {
        audioSource.volume = volumeSlider.value;
    }

    IEnumerator FadeOut(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed; // Adjust this value to change the fade speed
            yield return null;
        }

        panel.SetActive(false);
    }

    void ResetAlpha(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1;
    }
}