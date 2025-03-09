using UnityEngine;

public class PlanetSelector : MonoBehaviour
{
    public float detectionRange = 5f; 
    private Transform selectedRing;   
    private Transform currentPlanet;  

    void Start()
    {
        
        GameObject ringObject = GameObject.FindGameObjectWithTag("SelectedPlanet");
        if (ringObject != null)
        {
            selectedRing = ringObject.transform;
            selectedRing.gameObject.SetActive(false); 
        }
    }

    void Update()
    {
        DetectPlanets();
    }

    void DetectPlanets()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        Transform closestPlanet = null;
        float closestDistance = detectionRange;

        foreach (GameObject planet in planets)
        {
            float distance = Vector3.Distance(transform.position, planet.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlanet = planet.transform;
            }
        }

        if (closestPlanet != null)
        {
            
            if (currentPlanet != closestPlanet)
            {
                currentPlanet = closestPlanet;
                selectedRing.gameObject.SetActive(true);
                selectedRing.position = currentPlanet.position;
                selectedRing.localScale = currentPlanet.localScale * 1.1f; 
            }
        }
        else
        {
            
            currentPlanet = null;
            selectedRing.gameObject.SetActive(false);
        }
    }
}
