using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBack : MonoBehaviour
{
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        UIManager.Instance.StageClearClick();
    }
}
