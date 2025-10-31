using UnityEngine;

public class ShotEX : MonoBehaviour
{
    //총알 관련
    public BulletEX bullectEXPrefab;
    public Effect effectPrefab;
    public BulletEXUP bulletEXUPPrefab;

    [SerializeField] private float shotRate = 0.2f;  //발사간격
    [SerializeField] private float nextShotTime;     //다음 발사 가능 시간
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
        anim = playerController.GetComponent<Animator>();  //플레이어의 애니메이션

        playerStats = playerController.GetComponent<PlayerStats>();

    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectEXPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);
    }

    void Update()
    {
        //발사처리
        if (Input.GetKeyDown(KeyCode.D) && Time.time >= nextShotTime)
        {
            if (playerStats.BulletTimes > 0)
            {
                nextShotTime = Time.time + shotRate;  //다음 발사 가능 시간 계산

                if (!playerStats.ExUpYes)
                {
                    PlayerShot(); //발사
                    playerStats.BulletTimes -= useBullet; //특수탄 사용 후 감소
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

        //ShootPoint 빈 게임 오브젝트의 좌우 위치 설정
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

        Vector2 dir = playerSprite.flipX ? Vector2.left : Vector2.right;  //여기까지
    }
    void PlayerShot()
    {
        BulletEX bulletEX = Managers.Pool.GetFromPool(bullectEXPrefab);

        bulletEX.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //플레이어의 방향에 맞게 총알 좌우 뱡향 설정
        bulletEX.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bulletEX.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bulletEX.dir = Vector2.right;
        }                                         //여기까지
    }
    private void PowerExUPShot()
    {
        BulletEXUP bulletEXUP = Managers.Pool.GetFromPool(bulletEXUPPrefab);

        bulletEXUP.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //플레이어의 방향에 맞게 총알 좌우 뱡향 설정
        bulletEXUP.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bulletEXUP.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bulletEXUP.dir = Vector2.right;
        }                                         //여기까지
    }
}
