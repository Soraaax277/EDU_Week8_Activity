using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObstacleManager : MonoBehaviour
{
    public Item obstaclePrefab;
    public PlayerController player;
    public Transform obstacleContainer;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.8f;
    public float moveSpeed = 300f;
    public float startZ = 2000f;
    public float laneWidth = 50f;

    [Header("Collision Settings")]
    public float collisionZRange = 25f;
    public float damageAmount = 5f;

    private List<Item> activeObstacles = new List<Item>();
    private float lastSpawnTime;

    private void Start()
    {
        if (player == null) player = FindFirstObjectByType<PlayerController>();
        lastSpawnTime = Time.time;
    }

    private void Update()
    {
        if (player == null) return;

        if (Time.time - lastSpawnTime > spawnInterval)
        {
            SpawnObstacle();
            lastSpawnTime = Time.time;
        }

        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            var obstacle = activeObstacles[i];
            if (obstacle == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            obstacle.itemPosition.z -= moveSpeed * Time.deltaTime;

            CheckCollision(obstacle);

            if (obstacle.itemPosition.z < -50)
            {
                activeObstacles.RemoveAt(i);
                Destroy(obstacle.gameObject);
            }
        }

        SortObstacles();
    }

    private void SpawnObstacle()
    {
        int lane = Random.Range(0, 3);
        float xPos = (lane - 1) * laneWidth;
        
        var obstacle = Instantiate(obstaclePrefab, obstacleContainer);
        obstacle.itemPosition = new Vector3(xPos, -20f, startZ);
        activeObstacles.Add(obstacle);
    }

    private void CheckCollision(Item obstacle)
    {
        float zDiff = obstacle.itemPosition.z - player.playerPosition.z;

        if (zDiff > -collisionZRange && zDiff < collisionZRange)
        {
            float xDiff = Mathf.Abs(obstacle.itemPosition.x - player.playerPosition.x);
            
            if (xDiff < laneWidth * 0.5f)
            {
                if (!player.IsJumping())
                {
                    player.TakeDamage(damageAmount);
                    activeObstacles.Remove(obstacle);
                    Destroy(obstacle.gameObject);
                }
            }
        }
    }

    private void SortObstacles()
    {
        var sorted = activeObstacles.OrderByDescending(o => o.itemPosition.z).ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }
    }
}
