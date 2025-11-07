using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("CheckPoint")]
    //[SerializeField] private BeginningSpawn
    [SerializeField] private SavePoint[] checkPoint;

    [Header("Player Info")]
    [SerializeField] private string defualtPlayerName = "Kim";

    [Header("처음 시작 장소")]
    [SerializeField] private Transform reStartObject;

    public string LastCheckPointId { get; private set; }

    private int saveCount = 0;

    public int SaveCount
    {
        get { return saveCount; }
        set { saveCount = value; }
    }
   //private void OnEnable()
   //{
   //    SceneManager.sceneLoaded += OnSceneLoaded;
   //}
   //
   //private void OnDisable()
   //{
   //    SceneManager.sceneLoaded -= OnSceneLoaded;
   //}
   //
   //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
   //{
   //    var newPlayer = GameObject.FindWithTag("Player")?.GetComponent<PlayerStats>();
   //    if (newPlayer != null)
   //        UIManager.Instance.ConnectPlayer(newPlayer);
   //}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        TelePortCheckPoint(saveCount);
    }

    void Start()
    {
        if (SaveSystem.TryLoad(out var loaded))
        {
            UIManager.Instance.Score = 0;
            loaded.score = UIManager.Instance.Score;
            LastCheckPointId = loaded.lastCheckPointId;

            UIManager.Instance.ScoreUI();
            //TelePortCheckPoint(0);
        }
        else 
        {
            UIManager.Instance.Score = 0;
            LastCheckPointId = null;
            UIManager.Instance.ScoreUI();
        }
    }
    //세이브 포인트에 도착했을때 호출되는 메서드
    //현재 플래이어의 정보
    public void SaveCheckPoint(string checkPointId)
    {
        LastCheckPointId = checkPointId;

        var data = new GameData
        {
            playerName = defualtPlayerName,
            score = UIManager.Instance.Score,
            lastCheckPointId = LastCheckPointId,
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        };
        SaveSystem.Save(data);
    }
    //체크포인트ID에 해당하는 지점을 찾아서 플레이어를 그 위치로 이동
    public void TelePortCheckPoint(int number)
    {
        if (string.IsNullOrEmpty(LastCheckPointId)) return;
        if (checkPoint==null || checkPoint.Length == 0) return;
        if (player == null) return;

        for (int i = 0; i < checkPoint.Length; i++)
        {
            var cp = checkPoint[i];
            if (cp == null) continue;
            if (cp.CheckPointId != LastCheckPointId) continue;

            if (cp.SpawnPoint != null)
            {
                player.position = cp.SpawnPoint[number].position;
            }
            else
            {
                player.position = cp.transform.position;
            }
            break;
        }
        
    }
}
