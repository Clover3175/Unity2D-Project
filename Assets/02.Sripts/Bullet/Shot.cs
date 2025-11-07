using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;

public class Shot : MonoBehaviour
{
    //총알 관련
    public Bullet bullectPrefab;
    public Effect effectPrefab;
    public BulletUP bulletUPPrefab;

    [SerializeField] private float shotRate = 0.2f;  //발사간격
    [SerializeField] private float nextShotTime;     //다음 발사 가능 시간
    public Transform ShotPoint;

    [Header("공격시 멈추는 시간")]
    [SerializeField] private float stopTime = 0.4f;

    private SpriteRenderer playerSprite;

    private SpriteRenderer sprite;

    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private Animator anim;

    private PlayerStats playerStats;

    private static readonly int shotHash = Animator.StringToHash("Shot");

    private void Awake()
    {
        playerSprite = transform.parent.GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();

        playerController = transform.parent.GetComponent<PlayerController>();
        playerRb = playerController.GetComponent<Rigidbody2D>();
        anim = playerController.GetComponent<Animator>();  //플레이어의 애니메이션

        playerStats = playerController.GetComponent<PlayerStats>();
    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);

        playerStats.UpYes = false;
    }

    void Update()
    {
        //bulletUPPrefab = FindAnyObjectByType<BulletUP>();

        //발사처리
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shotRate;  //다음 발사 가능 시간 계산

            if (!playerStats.UpYes)
            {
                PlayerShot();  //발사
            }
            else if (playerStats.UpYes)
            {
                PowerUPShot();
            }

            anim.SetTrigger(shotHash);

            StartCoroutine(StopMove());
        }
        if (playerStats.UpYes)
        {
            playerStats.UpTime += Time.deltaTime;

            if (playerStats.UpTime >= playerStats.UpTimeMax)
            {
                playerStats.UpTime = 0.0f;

                playerStats.UpYes = false;
            }
        }

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
        Bullet bullet = Managers.Pool.GetFromPool(bullectPrefab);

        bullet.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //플레이어의 방향에 맞게 총알 좌우 뱡향 설정
        bullet.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bullet.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bullet.dir = Vector2.right;
        }                                         //여기까지
    }
    private void PowerUPShot()
    {
        BulletUP bulletUP = Managers.Pool.GetFromPool(bulletUPPrefab);

        bulletUP.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

        //플레이어의 방향에 맞게 총알 좌우 뱡향 설정
        bulletUP.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bulletUP.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bulletUP.dir = Vector2.right;
        }                                         //여기까지
    }
    IEnumerator StopMove()
    {
        playerController.playerStop = true;

        playerRb.velocity = Vector2.zero;

        yield return new WaitForSeconds(stopTime);

        playerController.playerStop = false;
    }
}
