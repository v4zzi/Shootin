using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [Header("Configuración de Munición")]
    public int ammoAmount = 30; 

    [Header("Efectos Visuales")]
    public float rotationSpeed = 90f; 

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Weapon weapon = other.GetComponentInChildren<Weapon>();

            if (weapon != null)
            {
                weapon.totalAmmo += ammoAmount;
                Debug.Log($"¡Recogiste {ammoAmount} balas! Munición total: {weapon.totalAmmo}");

                Destroy(gameObject);
            }
        }
    }
}