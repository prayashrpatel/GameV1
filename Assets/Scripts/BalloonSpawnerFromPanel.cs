using UnityEngine;
using System.Collections.Generic;

public class BalloonSpawnerFromPanel : MonoBehaviour
{
    public bool autoStart = false;
    public bool stopSpawning = true; // 🔸 Initially true to prevent spawning

    int maxAttempts = 10;
    float spawnRadius = 0.6f;
    public GameObject balloonPrefab;
    public RectTransform panelRect;
    public float spawnInterval = 2f;
    public int maxBalloons = 20;

    private float timer = 0f;
    private List<GameObject> activeBalloons = new List<GameObject>();

    void Start()
    {
        if (autoStart)
        {
            StartSpawning(); // 🔹 Optional auto-start
        }
    }

    void Update()
    {
        if (stopSpawning) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeBalloons.Count < maxBalloons)
        {
            timer = 0f;
            SpawnBalloon();
        }

        activeBalloons.RemoveAll(balloon => balloon == null);
    }

    void SpawnBalloon()
    {
        int attempts = 0;
        bool validPosition = false;
        Vector3 spawnPos = Vector3.zero;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        float margin = 0.5f;

        while (attempts < maxAttempts && !validPosition)
        {
            float x = Random.Range(min.x + margin, max.x - margin);
            float y = Random.Range(min.y + margin, max.y - margin);
            spawnPos = new Vector3(x, y, 0f);

            Collider2D[] hits = Physics2D.OverlapCircleAll(spawnPos, spawnRadius);
            if (hits.Length == 0)
            {
                validPosition = true;
            }

            attempts++;
        }

        if (!validPosition) return;

        GameObject balloon = Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
        balloon.transform.localScale = Vector3.zero;

        float randomMaxScale = Random.Range(0.3f, 0.6f);
        float randomGrowSpeed = Random.Range(3f, 6f);

        BalloonBehavior behavior = balloon.GetComponent<BalloonBehavior>();
        if (behavior != null)
        {
            behavior.maxScale = randomMaxScale;
            behavior.growSpeed = randomGrowSpeed;
        }

        activeBalloons.Add(balloon);
    }

    // 🔹 Public method to start spawning
    public void StartSpawning()
    {
        stopSpawning = false;
        timer = 0f;
    }
}
