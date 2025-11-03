using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private PlayerStats player;

    [Header("UI")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image[] skillIcon;

    private int i = 0;

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
        hpBar.value = Mathf.Lerp(hpBar.value, (float)player.PlayerHP / (float)player.MaxHP, Time.deltaTime * 10);
    }
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


}
