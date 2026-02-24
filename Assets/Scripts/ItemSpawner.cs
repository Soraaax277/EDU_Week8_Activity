using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 0.01f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    public float maxHorizontal;
    public float minHorizontal;
    public float maxVertical;
    public float minVertical;

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, 0.3f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);
        spawnedItem.itemPosition = GetRandomLocation();
        items.Add(spawnedItem);
    }

    private Vector3 GetRandomLocation()
    {
        var xRand = Random.Range(minHorizontal, maxHorizontal);
        var yRand = Random.Range(minVertical, maxVertical);
        return new Vector3(xRand, yRand, 0);
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            if (item == null)
            {
                items.RemoveAt(i);
                continue;
            }

            ItemMover(item);

            if (item.itemPosition.z < -CameraComponent.focalLenth + 10)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }

        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        
        var sortedChildren = children.OrderByDescending(x => {
            var item = x.GetComponent<Item>();
            return item != null ? item.itemPosition.z : 0;
        }).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            sortedChildren[i].SetSiblingIndex(i);
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed * Time.deltaTime * 1000f;
    }
}
