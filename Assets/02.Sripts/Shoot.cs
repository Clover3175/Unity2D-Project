using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //총알 관련
    public Bullet bullectPrefab;
    public Effect effectPrefab;

    [SerializeField] private float shootRate = 0.2f;  //발사간격
    [SerializeField] private float nextShootTime;     //다음 발사 가능 시간
    public Transform ShootPoint;

    private SpriteRenderer playerSprite;

    private SpriteRenderer sprite;

    //public Bullet bullet;

    private void Awake()
    {
        playerSprite = transform.parent.GetComponent<SpriteRenderer>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);
    }

    void Update()
    {
        //발사처리
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootRate;  //다음 발사 가능 시간 계산
            PlayerShoot();  //발사     
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
    void PlayerShoot()
    {
        Bullet bullet = Managers.Pool.GetFromPool(bullectPrefab);

        bullet.transform.SetLocalPositionAndRotation(ShootPoint.position, Quaternion.identity);

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
