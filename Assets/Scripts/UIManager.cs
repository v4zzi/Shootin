using UnityEngine;
using TMPro; 
public class UIManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;

    [Header("Referencias del Juego")]
    public Weapon currentWeapon;
    public PlayerHealth playerHealth;

    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    private void Start()
    {
        if (playerHealth == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
    }

    private void Update()
    {
        if (ammoText != null && currentWeapon != null)
        {
            ammoText.text = currentWeapon.currentAmmo + " / " + currentWeapon.totalAmmo;
        }

        if (healthText != null && playerHealth != null)
        {
            healthText.text = "" + Mathf.Max(0, Mathf.CeilToInt(playerHealth.currentHealth));
        }

        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(elapsedTime);
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}