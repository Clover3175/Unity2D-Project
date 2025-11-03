using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player/UI")]
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Player Info")]
    [SerializeField] private string defualtPlayerName = "Kim";
    [SerializeField] private int defualtScore = 1;

    public int Score { get; private set; }
    public string LastCheckPointId { get; private set; }

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
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
