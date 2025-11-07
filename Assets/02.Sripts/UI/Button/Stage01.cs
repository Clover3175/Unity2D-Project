using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage01 : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Stage01");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        if (player != null)
        {
            UIManager.Instance.ConnectPlayer(player);
        }
    }

   
}
