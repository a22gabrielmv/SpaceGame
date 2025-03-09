using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainGame : MonoBehaviour
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
        SceneManager.LoadScene("MainGame");
    }
}
