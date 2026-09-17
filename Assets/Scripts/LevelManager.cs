using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuración de Victoria")]
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;
    public GameObject victoryPanel;

    private int activeEnemies = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (portalPrefab != null) portalPrefab.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    public void EnemyDefeated()
    {
        activeEnemies--;
        Debug.Log("Enemigos restantes: " + activeEnemies);

        if (activeEnemies <= 0)
        {
            SpawnPortal();
        }
    }

    private void SpawnPortal()
    {
        Debug.Log("¡Todos los enemigos eliminados! Portal activado.");

        if (portalPrefab != null)
        {
            if (portalSpawnPoint != null)
            {
                portalPrefab.transform.position = portalSpawnPoint.position;
                portalPrefab.transform.rotation = portalSpawnPoint.rotation;
            }
            portalPrefab.SetActive(true);
        }
    }

    public void WinGame()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
}