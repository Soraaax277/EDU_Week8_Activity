using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public Item floorSegmentPrefab;
    public float segmentLength = 1000f;
    public int segmentCount = 5;
    public float scrollSpeed = 150f;
    public float startZ = 0f;

    private List<Item> segments = new List<Item>();

    private void Start()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            SpawnSegment(startZ + i * segmentLength);
        }
    }

    private void SpawnSegment(float z)
    {
        if (floorSegmentPrefab == null) return;
        var segment = Instantiate(floorSegmentPrefab, transform);
        segment.itemPosition = new Vector3(0, -50, z);
        segments.Add(segment);
    }

    private void Update()
    {
        var manager = FindFirstObjectByType<ObstacleManager>();
        if (manager != null) scrollSpeed = manager.moveSpeed;

        for (int i = segments.Count - 1; i >= 0; i--)
        {
            var segment = segments[i];
            segment.itemPosition.z -= scrollSpeed * Time.deltaTime;

            if (segment.itemPosition.z < -segmentLength)
            {
                float maxZ = 0;
                foreach (var s in segments) if (s.itemPosition.z > maxZ) maxZ = s.itemPosition.z;
                segment.itemPosition.z = maxZ + segmentLength;
            }
        }
    }
}
