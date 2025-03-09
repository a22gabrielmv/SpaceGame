using UnityEngine;
using System.Collections;
using TMPro;

public class GethAI : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 10f;
    public float rotationSpeed = 5f;
    public float fireRate = 2f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileMaxDistance = 500f;

    public AudioSource shootAudioSource; 
    public AudioClip shootSound; 

    public AudioSource deathAudioSource; 
    public AudioClip deathSound; 

    private Transform player;
    private bool canShoot = true;

    private int hitsTaken = 0;
    public int hitsToDestroy = 3; 

    public GameObject explosionPrefab; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (projectilePrefab != null) projectilePrefab.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        MoveTowardsPlayer();
        RotateTowardsPlayer();
        TryToShoot();
    }

    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void TryToShoot()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= stoppingDistance && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

        if (shootAudioSource != null && shootSound != null)
        {
            shootAudioSource.PlayOneShot(shootSound);
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.SetActive(true);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * 15f;
        }

        yield return new WaitForSeconds(fireRate);
        canShoot = true;

        Destroy(projectile, projectileMaxDistance / 15f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            hitsTaken++;
            Destroy(other.gameObject); 

            if (hitsTaken == hitsToDestroy)
            {
                deathAudioSource.PlayOneShot(deathSound);

                if (explosionPrefab != null)
                {
                    GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                    Destroy(explosionInstance, 2f);
                }

                Destroy(gameObject); 


                GameObject points = GameObject.FindGameObjectWithTag("PlayerPoints");
            
                if (points != null)
                {

                    TextMeshProUGUI pointsText = points.GetComponent<TextMeshProUGUI>();

                    if (pointsText != null)
                    {
                        string currentText = pointsText.text;  
                        int currentPoints = int.Parse(currentText.Split(' ')[1]);

                        currentPoints = Mathf.Max(currentPoints + 100, 0);
                        
                        pointsText.text = "Points: " + currentPoints;
                    }
                }
            }
        }
    }
}
