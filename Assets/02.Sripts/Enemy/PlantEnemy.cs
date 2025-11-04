using UnityEngine;

public class PlantEnemy : Enemy
{
    public enum State { Nomale, Attack }

    [Header("플레이어 체크")]
    [SerializeField] private Transform target;
    [SerializeField] private float checkLine = 5.0f;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private int plantHP = 3;
    [SerializeField] private int damage = 1;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private bool isPlayer;

    private PlayerStats player;

    private float damageCount = 0f;

    [SerializeField] private int plusScore = 100; //처치시 점수

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    private class EnemyState : BaseState
    {
        protected PlantEnemy owner;

        protected EnemyState(PlantEnemy owner)
        {
            this.owner = owner;
        }
        protected Transform target
        {
            get { return owner.target; }
        }
    }
    public void TakeDamage(int damage)
    {
        plantHP -= damage;

        if (plantHP <= 0)
        {
            gameObject.SetActive(false);
            UIManager.Instance.AddScore(plusScore);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerStats>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
