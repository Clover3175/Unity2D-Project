using UnityEngine;
using UnityEngine.SceneManagement;

public class ReStart : MonoBehaviour
{
    public void IsReStart()
    {
        //DontDestroyOnLoad 로 아직 파괴되지 않은 오브젝트들을 찾아서 파괴시킨다
        //이러한 과정이 없으면 씬이 다시 시작했을때 기존에 남아있던 오브젝트들이 방해를 한다.
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.scene.buildIndex == -1) //DontDestroyOnLoad 로 아직 파괴되지 않은 오브젝트들
            {
                Destroy(obj);
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UIManager.Instance.GameOverClick();
    }
}
