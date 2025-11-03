using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("체력/필살 횟수")]
    [SerializeField] private int playerHP = 5;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private int bulletTimes = 5;
    [SerializeField] private int bulletMax = 5;

    [Header("탄환 업그레이드 유지 시간")]
    [SerializeField] private float upTime = 0.0f;
    [SerializeField] private float upTimeMax = 20.0f;

    [Header("데미지를 받았을때 무적 시간/튕기는 거리")]
    [SerializeField] private float invincibleTime = 3.0f;
    [SerializeField] private int bounceDis = 7;

    private bool upYes = false;

    private bool exUpYes = false;

    private Rigidbody2D rb;

    private SpriteRenderer sprite;

    private PlayerController playerController;

    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        playerController = GetComponent<PlayerController>();
    }

    public int PlayerHP
    {
        get {  return playerHP; }
        set { playerHP = value; }
    }
    public int MaxHP
    {
        get { return maxHP; }
        set { maxHP = value; }
    }
    public int BulletTimes
    {
        get { return bulletTimes; }
        set { bulletTimes = value; }
    }
    public int BulletMax
    {
        get { return bulletMax; }
        private set { bulletMax = value; }
    }
    public bool UpYes
    {
        get { return upYes; }
        set { upYes = value; }
    }
    public bool ExUpYes
    {
        get { return exUpYes; }
        set { exUpYes = value; }
    }
    public float UpTime
    {
        get { return upTime; }
        set { upTime = value; }
    }
    public float UpTimeMax
    {
        get { return upTimeMax; }
        private set { upTimeMax = value; }
    }
    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        if (playerHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void TakeHeal(int heal)
    {
        playerHP += heal;

        if (playerHP >= maxHP)
        {
            playerHP = maxHP;
        }
    }
    public void TakeBulllet(int bulletLoad)
    {
        bulletTimes += bulletLoad;

        if (bulletTimes >= bulletMax)
        {
            bulletTimes = bulletMax;
        }

        UIManager.Instance.ReSkillIcon();
    }
    public void TakePowerUP(BulletUP bulletUP)
    {
        Managers.Pool.CreatPool(bulletUP, 1000);
    }
    public void TakeEXUP(BulletEXUP bulletEXUP)
    {
        Managers.Pool.CreatPool(bulletEXUP, 1000);
    }
    private void OffDamaged()
    {
        gameObject.layer = 3;
        sprite.color = new Color(1, 1, 1, 1);
    }
    IEnumerator OnDamaged(Vector2 enemyPos)
    {
        playerController.isBouncing = true;

        gameObject.layer = 8;
       
        sprite.color = new Color(1, 1, 1, 0.4f);
       
        int dirc = transform.position.x - enemyPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * bounceDis, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.5f);

        playerController.isBouncing = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        { 
            StartCoroutine(OnDamaged(collision.transform.position));

            Invoke("OffDamaged", invincibleTime);
        }
    }
}
