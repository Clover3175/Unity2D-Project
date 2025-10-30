using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Shot : MonoBehaviour
{
    //�Ѿ� ����
    public Bullet bullectPrefab;
    public Effect effectPrefab;

    [SerializeField] private float shotRate = 0.2f;  //�߻簣��
    [SerializeField] private float nextShotTime;     //���� �߻� ���� �ð�
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
        anim = playerController.GetComponent<Animator>();  //�÷��̾��� �ִϸ��̼�
    }

    void Start()
    {
        Managers.Pool.CreatPool(bullectPrefab, 100);
        Managers.Pool.CreatPool(effectPrefab, 100);
    }

    void Update()
    {
        //�߻�ó��
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shotRate;  //���� �߻� ���� �ð� ���
            PlayerShot();  //�߻�     
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
    void PlayerShot()
    {
        Bullet bullet = Managers.Pool.GetFromPool(bullectPrefab);

        bullet.transform.SetLocalPositionAndRotation(ShotPoint.position, Quaternion.identity);

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
