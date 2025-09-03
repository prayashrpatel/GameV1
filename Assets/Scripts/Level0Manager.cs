using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level0Manager : MonoBehaviour
{
    public static Level0Manager instance;

    [Header("Game Settings")]
    public int popGoal = 10;
    private int currentPops = 0;

    [Header("UI Elements")]
    public GameObject startButton;
    public GameObject continueButton;
    public GameObject playAgainButton;
    public TextMeshProUGUI mouseDirectionsText;
    public TextMeshProUGUI scoreboardText;

    [Header("Balloon Spawner")]
    public GameObject spawnerObject;

    [Header("Scoreboard Animation")]
    public Vector2 scoreboardStartPos = new Vector2(0, 200);
    public Vector2 scoreboardEndPos = new Vector2(0, -50);
    public float scoreboardFloatDuration = 1.0f;

    [Header("Button Animation")]
    public RectTransform continueButtonRect;
    public RectTransform playAgainButtonRect;
    public Vector2 buttonStartPos = new Vector2(0, -200);
    public Vector2 buttonEndPos = new Vector2(0, 50);
    public float buttonRiseDuration = 1.0f;

    [Header("Sounds")]
    public AudioClip[] popSounds;
    public AudioClip vineBoomSound;

    private RectTransform scoreboardRect;

    void Awake()
    {
        instance = this;
    }

    public void StartLevel0()
    {
        startButton.SetActive(false);
        if (mouseDirectionsText != null)
            mouseDirectionsText.gameObject.SetActive(false);

        scoreboardText.text = $"Balloons popped: 0/{popGoal}";
        scoreboardText.gameObject.SetActive(true);

        scoreboardRect = scoreboardText.GetComponent<RectTransform>();
        StartCoroutine(ScoreboardSequence());
    }

    private IEnumerator ScoreboardSequence()
    {
        yield return StartCoroutine(FloatScoreboardDown());
        yield return new WaitForSeconds(2f);

        spawnerObject.SetActive(true);
        var spawner = spawnerObject.GetComponent<BalloonSpawnerFromPanel>();
        if (spawner != null)
            spawner.StartSpawning();
    }

    private IEnumerator FloatScoreboardDown()
    {
        float duration = scoreboardFloatDuration;
        float elapsed = 0f;

        float horizontalOffset = Random.Range(-40f, 40f);
        Vector2 overshootPos = scoreboardEndPos + new Vector2(horizontalOffset, -20f);
        scoreboardRect.anchoredPosition = scoreboardStartPos;

        // Step 1: Drop down with ease-out
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float smoothT = 1f - Mathf.Pow(1f - t, 2f);
            scoreboardRect.anchoredPosition = Vector2.Lerp(scoreboardStartPos, overshootPos, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        scoreboardRect.anchoredPosition = overshootPos;

        // Step 2: Float slightly back up
        float reboundTime = 0.5f;
        elapsed = 0f;
        Vector2 settlePos = scoreboardEndPos + new Vector2(horizontalOffset, 0f);

        while (elapsed < reboundTime)
        {
            float t = elapsed / reboundTime;
            float smoothT = Mathf.Sin(t * Mathf.PI);
            scoreboardRect.anchoredPosition = Vector2.Lerp(overshootPos, settlePos, smoothT);
            elapsed += Time.deltaTime;
            yield return null;
        }

        scoreboardRect.anchoredPosition = settlePos;
    }

    public void BalloonPopped(Vector3 position)
    {
        currentPops++;

        if (currentPops < popGoal)
        {
            int index = Random.Range(0, popSounds.Length);
            AudioSource.PlayClipAtPoint(popSounds[index], position, 0.8f);
            scoreboardText.text = $"Balloons popped: {currentPops}/{popGoal}";
        }
        else if (currentPops == popGoal)
        {
            AudioSource.PlayClipAtPoint(vineBoomSound, position, 1f);
            scoreboardText.text = "Well done!";

            var spawner = FindObjectOfType<BalloonSpawnerFromPanel>();
            if (spawner != null)
                spawner.stopSpawning = true;

            foreach (var balloon in GameObject.FindGameObjectsWithTag("Balloon"))
            {
                Destroy(balloon);
            }

            StartCoroutine(AnimateButtonsUp());
        }
    }

    private IEnumerator AnimateButtonsUp()
    {
        continueButton.SetActive(true);
        playAgainButton.SetActive(true);

        continueButtonRect.anchoredPosition = buttonStartPos;
        playAgainButtonRect.anchoredPosition = buttonStartPos;

        float elapsed = 0f;
        while (elapsed < buttonRiseDuration)
        {
            float t = elapsed / buttonRiseDuration;
            float smoothT = 1f - Mathf.Pow(1f - t, 2f);

            continueButtonRect.anchoredPosition = Vector2.Lerp(buttonStartPos, buttonEndPos, smoothT);
            playAgainButtonRect.anchoredPosition = Vector2.Lerp(buttonStartPos, buttonEndPos + new Vector2(0, -60), smoothT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        continueButtonRect.anchoredPosition = buttonEndPos;
        playAgainButtonRect.anchoredPosition = buttonEndPos + new Vector2(0, -60);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
