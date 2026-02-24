using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = GetRandomColor();
        }
    }

    private void Update()
    {
        float perspective = CameraComponent.focalLenth / (CameraComponent.focalLenth + itemPosition.z);
        
        transform.localScale = Vector3.one * perspective * 250f;
        transform.position = new Vector3(itemPosition.x * perspective, itemPosition.y * perspective, itemPosition.z * 0.01f);
        
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(10000 - itemPosition.z);
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
