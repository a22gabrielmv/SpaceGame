using UnityEngine;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    [SerializeField] private Canvas mainCanvas;      
    [SerializeField] private Canvas controlsCanvas;  
    [SerializeField] private Button controlsButton;  
    [SerializeField] private Button returnButton;    

    void Start()
    {
        
        mainCanvas.gameObject.SetActive(true);
        controlsCanvas.gameObject.SetActive(false);

        
        if (controlsButton != null)
            controlsButton.onClick.AddListener(ShowControls);

        if (returnButton != null)
            returnButton.onClick.AddListener(ShowMainMenu);
    }

    void ShowControls()
    {
        mainCanvas.gameObject.SetActive(false);
        controlsCanvas.gameObject.SetActive(true);
    }

    void ShowMainMenu()
    {
        mainCanvas.gameObject.SetActive(true);
        controlsCanvas.gameObject.SetActive(false);
    }
}
