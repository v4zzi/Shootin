using UnityEngine;

public class VictoryPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.WinGame();
            }
        }
    }
}