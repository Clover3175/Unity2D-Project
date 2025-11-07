using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLast : MonoBehaviour
{

    public void LastStage()
    {
        SceneManager.LoadScene("Stage03");
        UIManager.Instance.StageClearClick();
    }
}
