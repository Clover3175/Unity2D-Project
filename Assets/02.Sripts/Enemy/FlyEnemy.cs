using System;
using UnityEngine;

public class FlyEnemy : MonoBehaviour
{
    public enum State { Nomal, Attack, Return }

    [Header(" 기본 체력, 속도, 탐지범위, 데미지, 상대 지정, 점수")]
    [SerializeField] private int enemyHP = 2;
    [SerializeField] float attackSpeed;
    [SerializeField] float backSpeed;
    [SerializeField] float findRange;
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform target;  //플레이어 트랜스폼
    [SerializeField] private int plusScore = 100; //처치시 점수

    private StateMachine stateMachine;
    private Vector2 startPos;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private PlayerStats player;    //플레이어 정보

    private float damageCount = 0f;  // 플레이어가 얼마만큼 닿아있으면 데미지를 입는지
    private bool reAttack = false;   //Return 중 다시 범위에 들어오면 재공격 하기 위한 bool변수

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        stateMachine = gameObject.AddComponent<StateMachine>();

        //상태등록
        stateMachine.AddState(State.Nomal, new NomalState(this));   //기본 상태
        stateMachine.AddState(State.Attack, new AttackState(this)); //공격 상태
        stateMachine.AddState(State.Return, new ReturnState(this)); //돌아가는 상태

        //첫 상태로 Nomal(기본) 상태로 설정
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

    private class FlyEnemyState : BaseState 
    {
        protected FlyEnemy owner;

        public FlyEnemyState(FlyEnemy owner)
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
        protected Vector2 startPos
        {
            get { return owner.startPos; }
        }
        protected float attackSpeed
        {
            get { return owner.attackSpeed; }
        }
        protected float backSpeed
        {
            get { return owner.backSpeed; }
        }
        protected float findRange
        {
            get { return owner.findRange; }
        }
        protected bool reAttack
        {
            get { return owner.reAttack; }
            set { owner.reAttack = value; }
        }
    }
    //Nomal: 플레이어를 발견하기 전까지의 기본 상태
    private class NomalState : FlyEnemyState
    {
        public NomalState(FlyEnemy owner) : base(owner) { }

        public override void Transition()
        {
            //플레이어가 일정거리 안으로 들어오면 공격함
            if (Vector2.Distance(target.position, transform.position) < findRange)
            {
                ChangeState(State.Attack);
            }
        }

    }
    //Attack : 적(플레이어)를 따라가 공격
    private class AttackState : FlyEnemyState
    {
        public AttackState(FlyEnemy owner) : base(owner) { }

        public override void Update()
        {
            //방향계산
            Vector2 dir = (target.position - transform.position).normalized;
            //이동
            transform.Translate(dir * attackSpeed * Time.deltaTime, Space.World);
        }
        public override void Transition()
        {
            if (Vector2.Distance(target.position, transform.position) > findRange)
            {
                ChangeState(State.Return);
            }
        }
    }
    //Return : 공격하는 영역에 벗어낫을 때 원래 위치로 돌아감
    private class ReturnState : FlyEnemyState
    {
        public ReturnState(FlyEnemy owner) : base(owner) { }

        public override void Update()
        {
            Vector2 dir = ((Vector3)startPos - transform.position).normalized;

            Vector2 reAttackDir = (target.position - transform.position).normalized;

            if (Vector2.Distance(target.position, transform.position) < findRange)
            {
                reAttack = true;
            }
            else if (Vector2.Distance(target.position, transform.position) > findRange)
            {
                reAttack = false;
            }

            if (!reAttack)
            {
                transform.Translate(dir * backSpeed * Time.deltaTime, Space.World);
            }
            else if (reAttack)
            {
                //이동
                transform.Translate(reAttackDir * attackSpeed * Time.deltaTime, Space.World);
            }
        }
        public override void Transition()
        {
            if (Vector3.Distance(startPos, transform.position) < 0.1f)
            {
                ChangeState(State.Nomal);
            }
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerStats>();

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
    public void TakeDamage(int damage)
    {
        enemyHP -= damage;

        if (enemyHP <= 0)
        {
            gameObject.SetActive(false);
            UIManager.Instance.AddScore(plusScore);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, findRange);
    }
}
