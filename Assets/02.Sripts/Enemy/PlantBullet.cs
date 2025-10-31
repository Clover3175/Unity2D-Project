using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    [Header("�Ѿ� �ӵ�, �����Ǵ� �ð�, ������")]
    [SerializeField] float speed = 10.0f;      //�Ѿ� �ӵ�
    [SerializeField] float lifeTime = 3.0f;    //�����Ǵ� �ð�
    [SerializeField] int damage = 1;           //������

    private PlayerStats player;

    private Rigidbody2D rb;
    public Vector2 dir = Vector2.left;

    private float spawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  
    }
    public int Damage
    {
        get { return damage; }
        private set { damage = value; }
    }
    private void OnEnable()
    {
        spawnTime = Time.time;
    }
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
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerStats>();

        if (player != null)
        {
            player.TakeDamage(Damage);

            ReturnPool();
        }
  
    }
    private void ReturnPool()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.ReturnPool(this);
        }
    }
}
