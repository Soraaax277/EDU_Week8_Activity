using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Lane Settings")]
    public float laneWidth = 50f;
    public int currentLane = 1;
    private float targetX;

    [Header("Jump Settings")]
    public float jumpHeight = 80f;
    public float jumpDuration = 0.4f;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private float currentYOffset = 0f;

    [Header("Positioning")]
    public Vector3 playerPosition;
    private SpriteRenderer spriteRenderer;

    [Header("Health")]
    public float maxHP = 100f;
    public float currentHP = 100f;
    public float regenRate = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerPosition = new Vector3(0, -30, 0);
        targetX = (currentLane - 1) * laneWidth;
    }

    private void Update()
    {
        HandleInput();
        UpdateJumping();
        UpdateMovement();
        ApplyPerspective();
        RegenerateHealth();
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
        {
            currentLane = Mathf.Max(0, currentLane - 1);
        }
        if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
        {
            currentLane = Mathf.Min(2, currentLane + 1);
        }
        if ((keyboard.wKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    private void UpdateJumping()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float progress = jumpTimer / jumpDuration;

            if (progress >= 1f)
            {
                isJumping = false;
                currentYOffset = 0f;
            }
            else
            {
                currentYOffset = 4f * jumpHeight * progress * (1f - progress);
            }
        }
    }
    private void UpdateMovement()
    {
        targetX = (currentLane - 1) * laneWidth;
        playerPosition.x = Mathf.Lerp(playerPosition.x, targetX, Time.deltaTime * 30f);
        playerPosition.y = -20f + currentYOffset;
    }
    private void ApplyPerspective()
    {
        float perspective = CameraComponent.focalLenth / (CameraComponent.focalLenth + playerPosition.z);
        transform.localScale = Vector3.one * perspective * 180f;
        transform.position = new Vector3(playerPosition.x * perspective, playerPosition.y * perspective, playerPosition.z * 0.01f);
        
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(10000 - playerPosition.z);
        }
    }
    private void RegenerateHealth()
    {
        if (currentHP < maxHP)
        {
            currentHP += regenRate * Time.deltaTime;
            currentHP = Mathf.Min(currentHP, maxHP);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);
        if (spriteRenderer != null) StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    public bool IsJumping() => isJumping;
}
