using UnityEngine;
using System.Collections.Generic;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnInterval = 2f;
    public int maxBalloons = 20;

    private float timer = 0f;
    private List<GameObject> activeBalloons = new List<GameObject>();

    void Update()
    {
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
        Camera cam = Camera.main;

        // Get screen bounds in world space
        Vector2 min = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = cam.ViewportToWorldPoint(new Vector2(1, 1));

        float margin = 0.5f; // keeps balloons from clipping at edges

        float x = Random.Range(min.x + margin, max.x - margin);
        float y = Random.Range(min.y + margin, max.y - margin);

        Vector3 spawnPos = new Vector3(x, y, 0f);

        GameObject balloon = Instantiate(balloonPrefab, spawnPos, Quaternion.identity);
        balloon.transform.localScale = Vector3.zero;
        balloon.AddComponent<BalloonBehavior>();
        activeBalloons.Add(balloon);
    }
}
