using UnityEngine;

public class CopyTest : Enemy
{
    public enum State { Nomal, Attack}

    [Header(" 기본 체력, 이동, 점프, 데미지")]
    [SerializeField] private int enemyHp = 3;
    [SerializeField] private float enemySpeed = 5.0f;
    [SerializeField] private int damage = 1;

    [Header("땅 체크")]
    [SerializeField] private float checkLine = 1.0f;
    [SerializeField] private LayerMask groundLayer;

    [Header("플레이어 체크")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackSpeed = 10.0f;
    [SerializeField] private float playerLine = 5.0f;
    [SerializeField] private float findRange = 5.0f;
    [SerializeField] private LayerMask playerLayer;

    private StateMachine stateMachine;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Vector2 startPos;

    private Vector2 groundCheck;
    private Vector2 playerCheck;
    private Vector2 direction;

    private int nextMove;
    public float thinkTime = 5.0f;
    private bool isGrounded;
    private bool isWall;
    private bool isPlayer;

    [SerializeField] private float CheckY = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        
        stateMachine = gameObject.AddComponent<StateMachine>();

        stateMachine.AddState(State.Nomal, new NomalState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));

        stateMachine.InitState(State.Nomal);
    }
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player")?.transform;
        }
        startPos = transform.position;
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
        protected CopyTest owner;

        public EnemyState(CopyTest owner)
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
            get {return owner.rb;}
        }
        protected SpriteRenderer sprite
        {
            get { return owner.sprite; }
        }
        protected float enemyHp
        {
            get { return owner.enemyHp; }
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
        protected Vector2 startPos
        {
            get { return owner.startPos; }
        }
        protected Vector2 groundPos
        {
            get { return owner.groundCheck; }
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
        public NomalState(CopyTest owner) : base(owner) { }

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
        public AttackState(CopyTest owner) : base(owner) { }

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
            rb.velocity = new Vector2 (dir.x * attackSpeed, rb.velocity.y);

            if (!isGrounded || isWall)
            {
                nextMove = nextMove * -1;
                owner.CancelInvoke();
                owner.Invoke("Think", thinkTime);
            }
        }
        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) > findRange)
            {
                owner.Invoke("Think", thinkTime);
                ChangeState(State.Nomal);
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

}
