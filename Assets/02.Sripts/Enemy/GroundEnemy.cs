using System.Collections;
using UnityEngine;

public class GroundEnemy : Enemy
{
    public enum State { Normal, Attack }

    [Header(" 기본 체력, 이동, 점프, 데미지")]
    [SerializeField] private int enemyHP = 3;
    [SerializeField] private float enemySpeed = 5.0f;
    [SerializeField] private int damage = 1;

    [Header("땅 체크")]
    [SerializeField] private float checkLine = 1.0f;
    [SerializeField] private LayerMask groundLayer;

    [Header("플레이어 체크")]
    [SerializeField] private Transform target;           //플레이어
    [SerializeField] private float attackSpeed = 10.0f;  //몬스터가 플레이어를 발견했을때 속도
    [SerializeField] private float playerLine = 5.0f;    //플레이어를 발견하는 레이캐스트 길이
    [SerializeField] private float findRange = 5.0f;     //플레이어 발견 후 따라가는 범위
    [SerializeField] private LayerMask playerLayer;

    private StateMachine stateMachine;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private Vector2 groundCheck;
    private Vector2 playerCheck;
    private Vector2 direction;

    private int nextMove;           //노멀 상태에서 방향좌표를 -1, 0, 1 중 랜덤으로 한다
    public float thinkTime = 5.0f;  //Invoke로 특정 함수를 thinkTime 후에 실행
    private bool isGrounded;        //땅 확인
    private bool isWall;            //벽 확인
    private bool isPlayer;          //플레이어 확인

    [SerializeField] private float CheckY = 1.0f;   //플레이어 발견하는 레이캐스트의 Y축 높이

    private static readonly int idleHash = Animator.StringToHash("DogRun");  //달려가는 애니메이션

    private PlayerController player;

    private float damageCount = 0f;  // 플레이어가 얼마만큼 닿아있으면 데미지를 입는지

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.Normal, new NomalState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));

        stateMachine.InitState(State.Normal);
    }
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (nextMove == 0)
        {
            sprite.flipX = false;
        }
        else if (nextMove < 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }

        anim.SetFloat(idleHash, Mathf.Abs(rb.velocity.x)); //달려가는 애니메이션 실행
    }
    private void FixedUpdate()
    {
        //아래 Ground 체크
        groundCheck = new Vector2(rb.position.x + nextMove, rb.position.y);
        isGrounded = Physics2D.Raycast(groundCheck, Vector2.down, checkLine, groundLayer);

        //앞에 Ground 체크
        isWall = Physics2D.Raycast(rb.position, Vector2.right * nextMove, checkLine, groundLayer);

        //앞 플레이어 체크
        playerCheck = new Vector2(rb.position.x, rb.position.y + CheckY);
        direction = sprite.flipX ? Vector2.right : Vector2.left;
        isPlayer = Physics2D.Raycast(playerCheck, direction, playerLine, playerLayer);
    }
    private void Think()
    {
        nextMove = Random.Range(-1, 2);
        Invoke("Think", thinkTime);   //재귀함수는 딜레이 없이 사용하는 것은 위험

    }
    //모든 적 상태의 공통 부모 클래스
    private class EnemyState : BaseState
    {
        protected GroundEnemy owner;

        public EnemyState(GroundEnemy owner)
        {
            this.owner = owner;
        }

        protected Transform transform
        {
            get { return owner.transform; }
        }
        protected Transform target
        {
            get { return owner.target; }
        }
        protected Rigidbody2D rb
        {
            get { return owner.rb; }
        }
        protected SpriteRenderer sprite
        {
            get { return owner.sprite; }
        }
        protected Animator anim
        {
            get { return owner.anim; }
        }
        protected float enemyHp
        {
            get { return owner.enemyHP; }
        }
        protected float enemySpeed
        {
            get { return owner.enemySpeed; }
        }
        protected float damage
        {
            get { return owner.damage; }
        }
        protected float checkLine
        {
            get { return owner.checkLine; }
        }
        protected LayerMask groundLayer
        {
            get { return owner.groundLayer; }
        }
        protected int nextMove
        {
            get { return owner.nextMove; }

            set { owner.nextMove = value; }
        }
        protected float attackSpeed
        {
            get { return owner.attackSpeed; }
        }
        protected float thinkTime
        {
            get { return owner.thinkTime; }
        }
        protected float findRange
        {
            get { return owner.findRange; }
        }
        protected bool isGrounded
        {
            get { return owner.isGrounded; }
        }
        protected bool isWall
        {
            get { return owner.isWall; }
        }
        protected bool isPlayer
        {
            get { return owner.isPlayer; }
        }
    }
    private class NomalState : EnemyState
    {
        public NomalState(GroundEnemy owner) : base(owner) { }

        public override void Awake()
        {
            owner.Invoke("Think", thinkTime);
        }
        public override void Update()
        {
            //기본 동작(가만히 있거나 돌아다님)
            rb.velocity = new Vector2(nextMove * enemySpeed, rb.velocity.y);

            //낭떨어지거나 앞에 Ground가 있으면 돌아가기
            if (!isGrounded || isWall)
            {
                nextMove = nextMove * -1;
                owner.CancelInvoke();
                owner.Invoke("Think", thinkTime);
            }
        }
        public override void Transition()
        {
            if (isPlayer)
            {
                owner.CancelInvoke();
                ChangeState(State.Attack);
            }
        }

    }
    private class AttackState : EnemyState
    {
        public AttackState(GroundEnemy owner) : base(owner) { }

        public override void Update()
        {
            //방향계산
            Vector2 dir = (target.position - transform.position).normalized;

            //타겟 방향으로 바라보기
            if (target.position.x < transform.position.x)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

            //이동
            rb.velocity = new Vector2(dir.x * attackSpeed, rb.velocity.y);

        }
        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) > findRange || !isGrounded)
            {
                owner.Think();
                if (nextMove == 0)
                {
                    if (sprite.flipX)
                    {
                        nextMove = -1;
                    }
                    else
                    {
                        nextMove = +1;
                    }
                }
                ChangeState(State.Normal);
            }
          
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            damageCount += Time.deltaTime;

            if (damageCount >= 2f)
            {
                player.TakeDamage(damage);
                damageCount = 0f;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (rb == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(groundCheck, Vector3.down * checkLine);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(rb.position, Vector2.right * nextMove * checkLine);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCheck, direction * playerLine);
    }
    public void TakeDamage(int damage)
    {
        enemyHP -= damage;

        if (enemyHP >= 0)
        {
            gameObject.SetActive(false);
        }
    }

}

