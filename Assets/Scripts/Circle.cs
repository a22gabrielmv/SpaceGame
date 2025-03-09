using UnityEngine;

public class Circle : MonoBehaviour
{
    [Header("Circle Settings")]
    [SerializeField] int segments = 10;
    [SerializeField] float radius = 5f;
    [SerializeField] float lineWidth = 0.1f;
    [SerializeField] Material material;

    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        float angleStep = 2 * Mathf.PI / segments;
        lineRenderer.positionCount = segments;   
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = material;
        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Sin(angleStep * i) * radius;
            float y = Mathf.Cos(angleStep * i) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
