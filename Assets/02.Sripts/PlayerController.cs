using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동/점프/대쉬/")]
    [SerializeField] private float moveSpeed = 5.0f;  //이동속도
    [SerializeField] private float jumpForce = 7.0f;  //점프높이
    [SerializeField] private float moveDash = 7.0f;   //대쉬속도

    [Header("땅 체크")]
    public Transform groundCheck;                         //땅 확인용 빈 오브젝트
    [SerializeField] private float groundCheckRadius = 0.15f;   //원의 반지름
    [SerializeField] private LayerMask groundLayer;

    [Header("벽 점프")]
    public Transform wallCheck;
    [SerializeField] private float wallCheckLine = 0.15f;
    [SerializeField] private float slidingSpeed;           //벽에 닿았을때 내려가는 속도
    [SerializeField] private float wallJumpForce = 5.0f;        //벽에서 점프하는 높이
    [SerializeField] private LayerMask wallLayer;

    [Header("사다리 종료 후 중력 작용 크기")]
    [SerializeField] private float thisGravity = 1.0f;

    [Header("낙하 피해/데미지 높이")]
    [SerializeField] private float damageY = -5.0f;
    [SerializeField] private float deathY = -10.0f;
    private float fallSpeed;

    //내부 컴포넌트
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private PlayerStats playerStats;

    private float inputX;     //방향키 좌우
    private float inputY;     //방향키 상하
    private bool isGrounded;  //땅 체크 확인
    private bool isJumped;    //점프 실행 확인
    private bool isLadder;    //사다리 확인
    private bool ladderUp;    //위 방향키를 눌렀을때 사다리 타게하기
    private bool isWall;      //벽 확인
    private bool isWallJump;
    private float isRight = 1;    //방향

    public bool playerStop = false;  // 피격되거나 공격할 때 멈추게 하기
    private bool wasGround;
    //private bool landed;

    //애니메이션
    private static readonly int walkHash = Animator.StringToHash("Walk"); 
    private static readonly int jumpHash = Animator.StringToHash("Jump"); 
    private static readonly int runHash = Animator.StringToHash("Run"); 
    private static readonly int clingHash = Animator.StringToHash("Cling"); 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }
    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        playerStop = false;
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0)
        {
            if (!isWallJump)
            {
                if (inputX < 0)   //왼쪽
                {
                    sprite.flipX = true;
                    isRight = -1;
                }
                else  //오른쪽
                {
                    sprite.flipX = false;
                    isRight = 1;
                }
            }
        }

        anim.SetFloat(walkHash, Mathf.Abs(rb.velocity.x));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumped = true;
        }

    }
    private void FixedUpdate()
    {
        if (playerStop) return;

        PlayerMove();
        
        PlayerDash();

        PlayerWall();

        PlayerFall();

        //사다리타기
        if (isLadder)
        { 
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                ladderUp = true;

                anim.SetBool(jumpHash, false);
            }
            if (ladderUp)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, inputY * moveSpeed);
            }
            else
            {
                rb.gravityScale = thisGravity;
            }
        }
        else
        {
            PlayerJump();
            ladderUp = false;
            rb.gravityScale = thisGravity;
        }
    }
    //플레이어 이동
    private void PlayerMove()
    {
        if (!isWallJump)
        {
            rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        }
    }
    //플레이어 점프
    private void PlayerJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isJumped && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetBool(jumpHash, true);
        }
        isJumped = false;

        if (isGrounded && rb.velocity.y <= 0.05f)
        {
            anim.SetBool(jumpHash, false);
        }
    }
    //대쉬(캐릭터 이속 증가)
    private void PlayerDash()
    {
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(inputX * moveDash, rb.velocity.y);

            anim.SetBool(runHash, true);
        }
        else
        {
            anim.SetBool(runHash, false);
        }
    }
    //플레이어 벽 점프
    private void PlayerWall()
    {
        isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * isRight, wallCheckLine, wallLayer);

        if (isWall)
        {
            isWallJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slidingSpeed);

            if (Input.GetAxis("Jump") != 0)
            {
                isWallJump = true;
                Invoke("FreezeX", 0.3f);  //Invoke() : 주어진 시간이 지난 뒤, 지정된 함수를 실행
                rb.velocity = new Vector2(-isRight * wallJumpForce, 0.9f * wallJumpForce);
                sprite.flipX = !sprite.flipX;
                isRight = isRight * - 1; 
            }
        }
        anim.SetBool(clingHash, isWall);
    }
    private void FreezeX()
    {
        isWallJump = false;
    }
    //낙하 데미지/사망
    private void PlayerFall()
    { 
        if (!isGrounded)
        {
            if (rb.velocity.y < fallSpeed)
            {
                fallSpeed = rb.velocity.y;
            }
        }
        else if (!wasGround && isGrounded)
        {
            if (fallSpeed < damageY)
            {
                playerStats.TakeDamage(1);
            }
            if (fallSpeed < deathY)
            {
                playerStats.TakeDamage(5);
            }
            fallSpeed = 0f;
        }
        wasGround = isGrounded;

        if (transform.position.y < -100)
        {
            playerStats.TakeDamage(5);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(wallCheck.position, Vector2.right * isRight * wallCheckLine);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //사다리에 닿으면 true
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //사다리에서 벗어나면 false
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;  
            ladderUp = false;
        }
    }
}
