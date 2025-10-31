using UnityEngine;

public class ShotEX : MonoBehaviour
{
    //�Ѿ� ����
    public BulletEX bullectEXPrefab;
    public Effect effectPrefab;
    public BulletEXUP bulletEXUPPrefab;

    [SerializeField] private float shotRate = 0.2f;  //�߻簣��
    [SerializeField] private float nextShotTime;     //���� �߻� ���� �ð�
    public Transform ShotPoint;

    private SpriteRenderer playerSprite;

    private SpriteRenderer sprite;

    private PlayerController playerController;
    private Animator anim;

    private PlayerStats playerStats;

    private int useBullet = 1;

    private void Awake()
    {
        playerSprite = transform.parent.GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();

        playerController = transform.parent.GetComponent<PlayerController>();
        anim = playerController.GetComponent<Animator>();  //�÷��̾��� �ִϸ��̼�

        playerStats = playerController.GetComponent<PlayerStats>();

    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectEXPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);
    }

    void Update()
    {
        //�߻�ó��
        if (Input.GetKeyDown(KeyCode.D) && Time.time >= nextShotTime)
        {
            if (playerStats.BulletTimes > 0)
            {
                nextShotTime = Time.time + shotRate;  //���� �߻� ���� �ð� ���

                if (!playerStats.ExUpYes)
                {
                    PlayerShot(); //�߻�
                    playerStats.BulletTimes -= useBullet; //Ư��ź ��� �� ����
                }
                else if (playerStats.ExUpYes)
                {
                    PowerExUPShot();
                    playerStats.BulletTimes -= useBullet;
                }
                    
            }
            
        }
        if (playerStats.ExUpYes)
        {  
            if (playerStats.UpTime >= playerStats.UpTimeMax - 0.1f)
            {
                playerStats.ExUpYes = false;  
            }
        }
        //if (playerStats.UpYes)
        //{
        //    if (shot.UpTime >= shot.UpTimeMax)
        //    {
        //        playerStats.UpYes = false;
        //    }
        //}

        //ShootPoint �� ���� ������Ʈ�� �¿� ��ġ ����
        sprite.flipX = playerSprite.flipX;

        Vector2 localPosition = transform.localPosition;

        if (playerSprite.flipX)
        {
            localPosition.x = -Mathf.Abs(localPosition.x);
        }
        else if (!playerSprite.flipX)
        {
            localPosition.x = Mathf.Abs(localPosition.x);
        }

        transform.localPosition = localPosition;

        Vector2 dir = playerSprite.flipX ? Vector2.left : Vector2.right;  //�������
    }
    void PlayerShot()
    {
        BulletEX bulletEX = Managers.Pool.GetFromPool(bullectEXPrefab);

        bulletEX.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //�÷��̾��� ���⿡ �°� �Ѿ� �¿� ���� ����
        bulletEX.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bulletEX.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bulletEX.dir = Vector2.right;
        }                                         //�������
    }
    private void PowerExUPShot()
    {
        BulletEXUP bulletEXUP = Managers.Pool.GetFromPool(bulletEXUPPrefab);

        bulletEXUP.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //�÷��̾��� ���⿡ �°� �Ѿ� �¿� ���� ����
        bulletEXUP.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bulletEXUP.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bulletEXUP.dir = Vector2.right;
        }                                         //�������
    }
}
