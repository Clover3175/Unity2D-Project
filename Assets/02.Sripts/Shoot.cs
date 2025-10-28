using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //�Ѿ� ����
    public Bullet bullectPrefab;
    public Effect effectPrefab;

    [SerializeField] private float shootRate = 0.2f;  //�߻簣��
    [SerializeField] private float nextShootTime;     //���� �߻� ���� �ð�
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
        //�߻�ó��
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootRate;  //���� �߻� ���� �ð� ���
            PlayerShoot();  //�߻�     
        }

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
    void PlayerShoot()
    {
        Bullet bullet = Managers.Pool.GetFromPool(bullectPrefab);

        bullet.transform.SetLocalPositionAndRotation(ShootPoint.position, Quaternion.identity);

        //�÷��̾��� ���⿡ �°� �Ѿ� �¿� ���� ����
        bullet.GetComponent<SpriteRenderer>().flipX = playerSprite.flipX;

        if (playerSprite.flipX)
        {
            bullet.dir = Vector2.left;
        }
        else if (!playerSprite.flipX)
        {
            bullet.dir = Vector2.right;
        }                                         //�������
    } 
}
