using System.Collections;
using TMPro;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [Header("savepoint setting")]
    [SerializeField] private string checkPointId = "CP_01"; //세이브 포인트의 ID
    [SerializeField] private Transform[] spawnPoint; //플레이어가 리스폰 할 위치
    [SerializeField] private float saveTextTime = 2f;
    [SerializeField] private SpriteRenderer savePlayer; //저장이 됐는지 확인용 오브젝트
    [SerializeField] private TextMeshProUGUI saveText;  //세이브 텍스트

    

    public string CheckPointId
    {
        get { return checkPointId; } 
    }
    public Transform[] SpawnPoint
    {
        get { return spawnPoint; }
    }
    //플레이어가 세이브 포인트에 도달했을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveCheckPoint(CheckPointId);

                savePlayer.sortingOrder = 2;

                StartCoroutine(SaveText());

                GameManager.Instance.SaveCount = 1;
            }
        }
    }
    IEnumerator SaveText()
    {
        saveText.gameObject.SetActive(true);

        yield return new WaitForSeconds(saveTextTime);

        saveText.gameObject.SetActive(false);
    }
}
