using UnityEngine;
using TMPro;
using System.Collections;

public class ReaperAI : MonoBehaviour
{
    public float initialSpeed = 2f;
    public float maxSpeed = 10f;
    public float acceleration = 0.5f;
    public float rotationSpeed = 5f;

    private Transform player;
    private BoxCollider reaperCollider;
    private BoxCollider playerCollider;
    private float speed;
    private bool isDamagingPlayer = false;

    private int hitsTaken = 0; 
    public int hitsToDestroy = 5; 

    public AudioSource audioSource; 
    public AudioClip explosionSound; 

    public AudioSource hitSource; 
    public AudioClip hitSound; 

    public GameObject explosionPrefab; 


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        reaperCollider = GetComponent<BoxCollider>();
        playerCollider = player.GetComponent<BoxCollider>();
        speed = initialSpeed;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>(); 
        }

        if (hitSource == null)
        {
            hitSource = GetComponent<AudioSource>(); 
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(180, -90, 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        speed = Mathf.Min(speed + acceleration * Time.deltaTime, maxSpeed);
        Vector3 targetPosition = CalculateTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.15f)
        {
            if (!isDamagingPlayer)
            {
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private Vector3 CalculateTargetPosition()
    {
        Vector3 reaperCenter = reaperCollider.transform.TransformPoint(reaperCollider.center);
        Vector3 playerCenter = playerCollider.transform.TransformPoint(playerCollider.center);
        Vector3 reaperSize = Vector3.Scale(reaperCollider.size, reaperCollider.transform.lossyScale);
        Vector3 playerSize = Vector3.Scale(playerCollider.size, playerCollider.transform.lossyScale);
        float distanceToTouch = (reaperSize.z / 4) + (playerSize.z / 4);
        Vector3 direction = (playerCenter - reaperCenter).normalized;
        Vector3 targetPosition = playerCenter - direction * distanceToTouch;
        targetPosition.y = transform.position.y;
        return targetPosition;
    }

    private IEnumerator DamagePlayer()
    {
        isDamagingPlayer = true;
        Vector3 targetPosition = CalculateTargetPosition();

        while (Vector3.Distance(transform.position, targetPosition) < 0.15f)
        {
            GameObject healthTextObject = GameObject.FindGameObjectWithTag("PlayerHealth");

            if (healthTextObject != null)
            {
                TextMeshProUGUI healthText = healthTextObject.GetComponent<TextMeshProUGUI>();

                if (healthText != null)
                {
                    hitSource.PlayOneShot(hitSound);

                    int currentHealth = int.Parse(healthText.text.Split(' ')[1]);
                    currentHealth = Mathf.Max(currentHealth - 5, 0);
                    healthText.text = "Health: " + currentHealth;

                    if (currentHealth == 0) break;
                }
            }

            yield return new WaitForSeconds(1f);
        }

        isDamagingPlayer = false;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            hitsTaken++;
            Destroy(other.gameObject); 

            if (hitsTaken == hitsToDestroy)
            {
                audioSource.PlayOneShot(explosionSound);

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

                        
                        currentPoints = Mathf.Max(currentPoints + 300, 0);
                        
                        
                        pointsText.text = "Points: " + currentPoints;
                    }
                }
            }
        }
    }
}
