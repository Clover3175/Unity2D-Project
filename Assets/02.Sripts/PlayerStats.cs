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

    private bool upYes = false;

    private bool exUpYes = false;


    public int PlayerHP
    {
        get {  return playerHP; }
        private set { playerHP = value; }
    }
    public int MaxHP
    {
        get { return maxHP; }
        private set { maxHP = value; }
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
    }
    public void TakePowerUP(BulletUP bulletUP)
    {
        Managers.Pool.CreatPool(bulletUP, 1000);
    }
    public void TakeEXUP(BulletEXUP bulletEXUP)
    {
        Managers.Pool.CreatPool(bulletEXUP, 1000);
    }
}
