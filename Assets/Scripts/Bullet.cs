using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public float damage = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("La bala chocó con: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player")) return;

        EnemyScript enemy = collision.gameObject.GetComponentInParent<EnemyScript>();

        if (enemy != null)
        {
            Debug.Log("¡Enemigo detectado! Aplicando daño...");
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("El objeto " + collision.gameObject.name + " NO tiene el script Enemy.");
        }

        Destroy(gameObject);
    }
}