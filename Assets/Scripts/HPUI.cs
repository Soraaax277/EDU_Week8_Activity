using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPUI : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI hpText;
    public Image hpBar;

    private void Update()
    {
        if (player != null)
        {
            if (hpText != null)
            {
                hpText.text = $"HP: {Mathf.CeilToInt(player.currentHP)} / {player.maxHP}";
            }       
            if (hpBar != null)
            {
                hpBar.fillAmount = player.currentHP / player.maxHP;
            }
        }
    }
}
