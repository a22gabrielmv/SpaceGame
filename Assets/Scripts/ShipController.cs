using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipController : MonoBehaviour
{
    public float moveSpeed = 10f;  
    public Camera mainCamera;     
    public Vector3 cameraOffset = new Vector3(0, 10, -5); 
    public float cameraSmoothSpeed = 5f; 

    public GameObject projectilePrefab; 
    public Transform projectileSpawnPoint; 
    public float projectileSpeed = 15f; 
    public float projectileMaxDistance = 500f; 

    public AudioSource shootAudioSource;
    public AudioClip shootSound;

    private Transform[] trails; 
    private Vector3[] initialTrailOffsets; 

    private void Start()
    {
        GameObject[] trailObjects = GameObject.FindGameObjectsWithTag("Trail");
        trails = new Transform[trailObjects.Length];
        initialTrailOffsets = new Vector3[trailObjects.Length];

        for (int i = 0; i < trailObjects.Length; i++)
        {
            trails[i] = trailObjects[i].transform;
            initialTrailOffsets[i] = trails[i].position - transform.position;
        }

        GameObject projectileObject = GameObject.FindGameObjectWithTag("PlayerProjectile");
        if (projectileObject != null)
        {
            projectilePrefab = projectileObject;
            projectilePrefab.SetActive(false);
            projectileSpawnPoint = projectileObject.transform;
        }
    }

    private void Update()
    {
        MoveShip();
        UpdateTrails();
        HandleShooting();
        CheckHealth();
    }

    void MoveShip()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePos = ray.GetPoint(distance);
            worldMousePos.y = transform.position.y;

            Vector3 direction = (worldMousePos - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }

            if (Input.GetMouseButton(0))
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    void UpdateTrails()
    {
        if (trails == null) return;

        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].position = transform.position + transform.rotation * initialTrailOffsets[i];
        }
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.F) && projectilePrefab != null && projectileSpawnPoint != null)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (shootAudioSource != null && shootSound != null)
        {
            shootAudioSource.PlayOneShot(shootSound);
        }

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.SetActive(true);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = projectileSpawnPoint.forward * projectileSpeed;
        }

        Destroy(projectile, projectileMaxDistance / projectileSpeed);
    }

    void CheckHealth()
    {
        GameObject healthTextObject = GameObject.FindGameObjectWithTag("PlayerHealth");

        if (healthTextObject != null)
        {
            TextMeshProUGUI healthText = healthTextObject.GetComponent<TextMeshProUGUI>();

            if (healthText != null)
            {
                int currentHealth = int.Parse(healthText.text.Split(' ')[1]);

                if (currentHealth <= 0)
                {
                    DestroyShip();
                }
            }
        }
    }

    void DestroyShip()
    {
        SceneManager.LoadScene("GameOver");
    }
}
