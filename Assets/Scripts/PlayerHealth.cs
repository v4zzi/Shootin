using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Game Over")]
    public GameObject gameOverPanel;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Salud del jugador: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;
        Debug.Log("¡Has muerto!");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}