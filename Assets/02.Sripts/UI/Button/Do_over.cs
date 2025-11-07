using UnityEngine;
using UnityEngine.SceneManagement;

public class Do_over : MonoBehaviour
{
    private PlayerStats player;

    public void Over()
    {
        if (SaveSystem.TryLoad(out var data))
        {
            SceneManager.LoadScene(data.sceneName);
        }
    }
}
