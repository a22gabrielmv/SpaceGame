using UnityEngine;
using UnityEngine.UI;
public class ExitGame : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(QuitGame);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}