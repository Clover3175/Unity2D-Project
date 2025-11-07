using UnityEngine;

public class BulletUP : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;      //총알 속도
    [SerializeField] float lifeTime = 3.0f;    //유지되는 시간
    [SerializeField] int damage = 3;           //데미지

    private Rigidbody2D rb;
    public Vector2 dir = Vector2.right;

    private float spawnTime;

    public Effect effectPrefab;

    private GroundEnemy groundEnemy;
    private PlantEnemy plantEnemy;
    private FlyEnemy flyEnemy;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public int Damage
    {
        get { return damage; }
        private set { damage = value; }
    }

    //총알이 활성화될 때(풀에서 꺼낼때)
    private void OnEnable()
    {
        spawnTime = Time.time;
    }
    //총알 발사 중
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifeTime)
        {
            ReturnPool();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var fx = PoolManager.Instance.GetFromPool(effectPrefab);

        if (fx != null)
        {
            fx.transform.position = transform.position; //총알위치에 배치
            fx.PlayEffect();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            groundEnemy = collision.gameObject.GetComponent<GroundEnemy>();
            plantEnemy = collision.gameObject.GetComponent<PlantEnemy>();
            flyEnemy = collision.gameObject.GetComponent<FlyEnemy>();

            if (groundEnemy != null)
            {
                groundEnemy.TakeDamage(Damage);
            }
            if (plantEnemy != null)
            {
                plantEnemy.TakeDamage(Damage);
            }
            if (flyEnemy != null)
            {
                flyEnemy.TakeDamage(Damage);
            }
            ReturnPool();
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }

    void ReturnPool()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
