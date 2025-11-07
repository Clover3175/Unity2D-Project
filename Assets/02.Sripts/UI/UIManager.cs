using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private PlayerStats player;

    [Header("UI")]
    [SerializeField] private Slider hpBar;        //HP바
    [SerializeField] private Image[] skillIcon;   //스킬 아이콘
    [SerializeField] private TextMeshProUGUI scoreText;  //스코어 텍스트
    [SerializeField] private TextMeshProUGUI lifeText;  //게임 클리어 후 목숨 텍스트
    [SerializeField] private TextMeshProUGUI isLifeText;  //플레이 화면에 목숨 텍스트
    [SerializeField] private TextMeshProUGUI totalText; //합계 점수 텍스트
    [SerializeField] private GameObject stageClearUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject tutorialUI;

    private int i = 0;

    public int Score { get; set; }

    public Image[] SkillIcon
    {
        get { return skillIcon; }
        set { skillIcon = value; }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var newPlayer = GameObject.FindWithTag("Player");
        if (newPlayer != null)
        {
            player = newPlayer.GetComponent<PlayerStats>();
            ConnectPlayer(player);
        }
    }

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
        player = FindAnyObjectByType<PlayerStats>();
        hpBar.value  = (float)player.PlayerHP / (float)player.MaxHP;

        foreach (Image icon in skillIcon)
        {
            icon.enabled = true;  // .enabled : UI이미지 활성화 여부 설정
        }
 
    }

    void Update()
    {
        HandleHp();
        IsLifeUI();
    }

    private void HandleHp()
    {
        //현재 HP 상황 계산
        hpBar.value = Mathf.Lerp(hpBar.value, (float)player.PlayerHP / (float)player.MaxHP, Time.deltaTime * 10);
    }
    //스킬을 사용했을때 아이콘 변화
    public void UseSkill()
    {
        if (skillIcon != null)
        {
            if (player.BulletTimes >= 0)
            {
                skillIcon[player.BulletTimes].enabled = false;
            }
            
        }
    }
    //아이템을 먹었을때 스킬 아이콘 변화
    public void ReSkillIcon()
    {
        if (skillIcon != null)
        {
            if (i < player.BulletTimes)
            {
                skillIcon[i].enabled = true;
                i++;
            }
        }
    }

    public void ScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {Score}";
        }
    }
    public void AddScore(int amount)
    {
        Score += amount;
    }
    public void ResetScore()
    {
        Score = 0;
    }
    public void LifeUI()
    {
        if (lifeText != null)
        {
            lifeText.text = $"X {player.PlayerLife}";
        }
    }
    public void IsLifeUI()
    {
        if (isLifeText != null)
        {
            isLifeText.text = $"X {player.PlayerLife}";
        }
    }
    public void TotalScoreUI()
    {
        if (totalText != null)
        {
            totalText.text = $"Total Score : {Score * player.PlayerLife}";
        }
    }
    public void StageClearUI()
    {
        stageClearUI.gameObject.SetActive(true);
    }
    public void StageClearClick()
    {
        stageClearUI.gameObject.SetActive(false);
    }
    public void GameOverUI()
    {
        gameOverUI.gameObject.SetActive(true);
    }
    public void GameOverClick()
    {
        gameOverUI.gameObject.SetActive(false);
    }
    public void TutorialUI()
    {
        tutorialUI.gameObject.SetActive(true);
    }
    public void ConnectPlayer(PlayerStats newPlayer)
    {
        player = newPlayer;

        // HP바 초기화
        hpBar.value = (float)player.PlayerHP / player.MaxHP;

        // 스킬 아이콘 초기화
        if (skillIcon != null)
        {
            for (int i = 0; i < skillIcon.Length; i++)
            {
                skillIcon[i].enabled = true;
            }
        }
    }

}
