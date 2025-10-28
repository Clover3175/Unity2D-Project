using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10.0f;      //총알 속도
    [SerializeField] float lifeTime = 3.0f;    //유지되는 시간
    [SerializeField] int damage = 1;           //데미지

    private Rigidbody2D rb;
    public Vector2 dir = Vector2.right;

    private float spawnTime;

    public Effect effectPrefab;

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
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifeTime)
        {
            ReturnPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        var fx = PoolManager.Instance.GetFromPool(effectPrefab);

        if (fx != null)
        {
            fx.transform.position = transform.position; //총알위치에 배치
            fx.PlayEffect();
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
