using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Shot : MonoBehaviour
{
    //총알 관련
    public Bullet bullectPrefab;
    public Effect effectPrefab;

    [SerializeField] private float shotRate = 0.2f;  //발사간격
    [SerializeField] private float nextShotTime;     //다음 발사 가능 시간
    public Transform ShotPoint;

    private SpriteRenderer playerSprite;

    private SpriteRenderer sprite;

    private PlayerController playerController; 
    private Animator anim;

    //public Bullet bullet;

    private void Awake()
    {
        playerSprite = transform.parent.GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();

        playerController = transform.parent.GetComponent<PlayerController>();
        anim = playerController.GetComponent<Animator>();  //플레이어의 애니메이션
    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);
    }

    void Update()
    {
        //발사처리
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shotRate;  //다음 발사 가능 시간 계산
            PlayerShot();  //발사     
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
}
