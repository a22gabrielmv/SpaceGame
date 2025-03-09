using UnityEngine;
using TMPro; 

public class GethProjectile : MonoBehaviour
{

    public AudioSource hitSource; 
    public AudioClip hitSound; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GameObject healthTextObject = GameObject.FindGameObjectWithTag("PlayerHealth");
            
            if (healthTextObject != null)
            {
                
                TextMeshProUGUI healthText = healthTextObject.GetComponent<TextMeshProUGUI>();

                if (healthText != null)
                {
                    hitSource.PlayOneShot(hitSound);

                    
                    string currentText = healthText.text;  
                    int currentHealth = int.Parse(currentText.Split(' ')[1]); 

                    
                    currentHealth = Mathf.Max(currentHealth - 10, 0);
                    
                    
                    healthText.text = "Health: " + currentHealth;
                }
            }

            
            Destroy(gameObject);
        }
    }
}
