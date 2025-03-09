using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToStart : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadGameScene);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
