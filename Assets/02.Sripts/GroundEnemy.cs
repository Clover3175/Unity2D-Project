using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("체력, 이동, 점프, 데미지")]
    [SerializeField] private int enemyHp = 3;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump = 2.0f;
    [SerializeField] private int damage = 1;

    [Header("땅 체크")]
    [SerializeField] private float checkLine = 1.0f;
    [SerializeField] private LayerMask groundLayer;

    //[Header("플레이어 포착")]
    //[SerializeField] private float playerCheck = 5.0f;

    private Rigidbody2D rb;

    private Vector2 groundVec;

    private int nextMove;
    public float thinkTime = 5;
    private bool isGrounded;
    private bool isWall;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("Think", thinkTime);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        EnemyMove();
    }

    private void EnemyMove()
    {
        //이동
        rb.velocity = new Vector2(nextMove * speed, rb.velocity.y);

        //아래 Ground 체크
        groundVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        isGrounded = Physics2D.Raycast(groundVec, Vector2.down, checkLine, groundLayer);

        //앞에 Ground 체크
        isWall = Physics2D.Raycast(rb.position, Vector2.right * nextMove, checkLine, groundLayer);

        //낭떨어지거나 앞에 Ground가 있으면 돌아가기
        if (!isGrounded || isWall)
        {
            nextMove = nextMove * -1;
            CancelInvoke();  //현재 작동 중인 모든 Invoke함수를 멈추는 함수이다.
            Invoke("Think", thinkTime); //낭떨어지에서 방향을 바꾸고 다시 Invoke
        }
    }
    //재귀합수 : 자신을 스스로 호출하는 함수
    private void Think()
    {
        nextMove= Random.Range(-1, 2);

        Invoke("Think", thinkTime);   //재귀함수는 딜레이 없이 사용하는 것은 위험

    }
    private void OnDrawGizmosSelected()
    {
        if (rb == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawRay(groundVec, Vector3.down * checkLine);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(rb.position, Vector2.right * nextMove * checkLine);
        
    }

}
