using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Referencias")]
    public Camera playerCamera;
    public GameObject bulletPrefab;
    public Transform muzzle;

    [Header("Configuración de Disparo")]
    public float bulletSpeed = 100f;
    public float fireRate = 0.2f;
    public float range = 500f;
    public LayerMask ignoreLayers;

    [Header("Sistema de Munición")]
    public int maxAmmoInMag = 30;
    public int currentAmmo;
    public int totalAmmo = 90;
    public float reloadTime = 1.5f;
    private bool isReloading = false;

    private float nextTimeToFire = 0f;
    private PlayerInputs inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputs();
    }

    private void OnEnable() => inputActions.Player.Enable();
    private void OnDisable() => inputActions.Player.Disable();

    private void Start()
    {
        currentAmmo = maxAmmoInMag;
    }

    private void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0 && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < maxAmmoInMag && totalAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (inputActions.Player.Fire.IsPressed() && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
            else
            {
                Debug.Log("¡Sin munición! Presiona R para recargar.");
            }
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || muzzle == null || playerCamera == null) return;

        currentAmmo--; 

        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, range, ~ignoreLayers))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(range);
        }

        Vector3 directionToTarget = (targetPoint - muzzle.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(directionToTarget));

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = directionToTarget * bulletSpeed;
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recargando...");

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmoInMag - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        isReloading = false;
        Debug.Log("Recarga completada.");
    }
}