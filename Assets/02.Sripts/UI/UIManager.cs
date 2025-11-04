using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private PlayerStats player;

    [Header("UI")]
    [SerializeField] private Slider hpBar;        //HP바
    [SerializeField] private Image[] skillIcon;   //스킬 아이콘
    [SerializeField] private TextMeshProUGUI scoreText;  //스코어 텍스트
    [SerializeField] private GameObject stageClearUI;
    [SerializeField] private GameObject gameOverUI;

    private int i = 0;

    public int Score { get; set; }

    public Image[] SkillIcon
    {
        get { return skillIcon; }
        set { skillIcon = value; }
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
                skillIcon[ player.BulletTimes ].enabled = false;
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
    public void AddScore(int amount)
    {
        Score += amount;
    }
    public void ResetScore()
    {
        Score = 0;
    }
    public void ScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {Score}";
        }
    }
    public void StageClear()
    {
        stageClearUI.gameObject.SetActive(true);
    }
    public void StageClearClick()
    {
        stageClearUI.gameObject.SetActive(false);
    }
    public void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
    }
    public void GameOverClick()
    {
        gameOverUI.gameObject.SetActive(false);
    }
}
