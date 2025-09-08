using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(AudioSource))]
public class ButtonManager : MonoBehaviour
{
    [Header("Tabs")]
    public GameObject aboutTab;
    public GameObject settingsTab;
    public GameObject escapeTab;       // Pause/Resume tab

    [Header("Audio")]
    public AudioClip startSound;       // (optional) existing sound
    public AudioClip quitSound;        // (optional) plays before quitting
    private AudioSource audioSource;
    public Slider volumeSlider;        // (optional) volume slider

    [Header("Fading")]
    public float fadeSpeed = 20.0f;    // CanvasGroup fade speed

    [Header("Quit UI (optional)")]
    public GameObject quitConfirmTab;  // Assign a panel with Yes/No buttons

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (volumeSlider != null)
        {
            audioSource.volume = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(_ => OnVolumeChange());
        }

        if (escapeTab) escapeTab.SetActive(false);
        if (aboutTab)  aboutTab.SetActive(false);
        if (quitConfirmTab) quitConfirmTab.SetActive(false);
    }

    void Update()
    {
        // Toggle Escape tab
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escapeTab && !escapeTab.activeSelf) { escapeTab.SetActive(true); ResetAlpha(escapeTab); }
            else if (escapeTab)                     { StartCoroutine(FadeOut(escapeTab)); }

            if (aboutTab   && aboutTab.activeSelf)    StartCoroutine(FadeOut(aboutTab));
            if (settingsTab && settingsTab.activeSelf) StartCoroutine(FadeOut(settingsTab));
        }

        // RMB closes any open tabs
        if (Input.GetMouseButtonDown(1))
        {
            if (aboutTab    && aboutTab.activeSelf)    StartCoroutine(FadeOut(aboutTab));
            if (settingsTab && settingsTab.activeSelf) StartCoroutine(FadeOut(settingsTab));
            if (escapeTab   && escapeTab.activeSelf)   StartCoroutine(FadeOut(escapeTab));
            if (quitConfirmTab && quitConfirmTab.activeSelf) StartCoroutine(FadeOut(quitConfirmTab));
        }
    }

    // --- Buttons you already have ---
    public void SettingsButton()
    {
        if (!settingsTab) return;
        if (!settingsTab.activeSelf) { settingsTab.SetActive(true); ResetAlpha(settingsTab); }
        else                          StartCoroutine(FadeOut(settingsTab));
    }

    public void AboutButton()
    {
        if (!aboutTab) return;
        if (!aboutTab.activeSelf) { aboutTab.SetActive(true); ResetAlpha(aboutTab); }
        else                        StartCoroutine(FadeOut(aboutTab));
    }

    void OnVolumeChange()
    {
        audioSource.volume = volumeSlider ? volumeSlider.value : audioSource.volume;
    }

    // --- Quit workflow ---
    // Hook this to your Quit button
    public void QuitButton()
    {
        if (quitConfirmTab != null)
        {
            quitConfirmTab.SetActive(true);
            ResetAlpha(quitConfirmTab);
        }
        else
        {
            QuitNow();
        }
    }

    // Hook these to the Yes / No buttons on the confirm panel
    public void QuitConfirmYes() { QuitNow(); }
    public void QuitConfirmNo()
    {
        if (quitConfirmTab) StartCoroutine(FadeOut(quitConfirmTab));
    }

    void QuitNow()
    {
        if (quitSound) audioSource.PlayOneShot(quitSound);
        StartCoroutine(QuitAfterAudio());
    }

    IEnumerator QuitAfterAudio()
    {
        // use unscaled so it still waits if your game is paused
        float wait = (quitSound != null) ? Mathf.Min(quitSound.length, 0.35f) : 0f;
        if (wait > 0f) yield return new WaitForSecondsRealtime(wait);

        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;   // stops Play Mode
        #else
        Application.Quit();                     // exits the app
        #endif
    }

    // --- Helpers you already use ---
    IEnumerator FadeOut(GameObject panel)
    {
        if (!panel) yield break;
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();

        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.unscaledDeltaTime * fadeSpeed;
            yield return null;
        }
        panel.SetActive(false);
        canvasGroup.alpha = 1f; // reset for next time it opens
    }

    void ResetAlpha(GameObject panel)
    {
        if (!panel) return;
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip) audioSource.PlayOneShot(clip);
    }
}
