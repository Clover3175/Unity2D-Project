using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("ü��/�̵�/����/�뽬")]
    [SerializeField] private int playerHP = 5;
    [SerializeField] private float moveSpeed = 5.0f;  //�̵��ӵ�
    [SerializeField] private float jumpForce = 7.0f;  //��������
    [SerializeField] private float moveDash = 7.0f;   //�뽬�ӵ�

    [Header("�� üũ")]
    public Transform groundCheck;                         //�� Ȯ�ο� �� ������Ʈ
    [SerializeField] private float groundCheckRadius = 0.15f;   //���� ������
    [SerializeField] private LayerMask groundLayer;

    [Header("�� ����")]
    public Transform wallCheck;
    [SerializeField] private float wallCheckLine = 0.15f;
    [SerializeField] private float slidingSpeed;           //���� ������� �������� �ӵ�
    [SerializeField] private float wallJumpForce = 5.0f;        //������ �����ϴ� ����
    [SerializeField] private LayerMask wallLayer;


    [Header("��ٸ� ���� �� �߷� �ۿ� ũ��")]
    [SerializeField] private float ladderGravity = 1.0f;

    //���� ������Ʈ
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private float inputX;     //����Ű �¿�
    private float inputY;     //����Ű ����
    private bool isGrounded;  //�� üũ Ȯ��
    private bool isJumped;    //���� ���� Ȯ��
    private bool isLadder;    //��ٸ� Ȯ��
    private bool ladderUp;    //�� ����Ű�� �������� ��ٸ� Ÿ���ϱ�
    private bool isWall;      //�� Ȯ��
    private bool isWallJump;
    private float isRight = 1;    //����



    //�ִϸ��̼�
    private static readonly int walkHash = Animator.StringToHash("Walk"); 
    private static readonly int jumpHash = Animator.StringToHash("Jump"); 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0)
        {
            if (!isWallJump)
            {
                if (inputX < 0)   //����
                {
                    sprite.flipX = true;
                    isRight = isRight * -1;
                }
                else  //������
                {
                    sprite.flipX = false;
                    isRight = isRight * -1;
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
        PlayerMove();
        
        PlayerDash();

        PlayerWall();

        //��ٸ�Ÿ��
        if (isLadder)
        { 
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                ladderUp = true;
            }
            if (ladderUp)
            {
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x, inputY * moveSpeed);
            }
            else
            {
                rb.gravityScale = ladderGravity;
            }
        }
        else
        {
            PlayerJump();
            ladderUp = false;
            rb.gravityScale = ladderGravity;
        }
    }
    //�÷��̾� �̵�
    private void PlayerMove()
    {
        if (!isWallJump)
        {
            rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);
        }
    }
    //�÷��̾� ����
    private void PlayerJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isJumped && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //anim.SetBool(jumpHash, true);
        }
        isJumped = false;

        if (isGrounded && rb.velocity.y <= 0.05f)
        {
            anim.SetBool(jumpHash, false);
        }
    }
    //�뽬(ĳ���� �̼� ����)
    private void PlayerDash()
    {
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(inputX * moveDash, rb.velocity.y);
        }
    }
    //�÷��̾� �� ����
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
                Invoke("FreezeX", 0.3f);  //Invoke() : �־��� �ð��� ���� ��, ������ �Լ��� ����
                rb.velocity = new Vector2(-isRight * wallJumpForce, 0.9f * wallJumpForce);
                sprite.flipX = !sprite.flipX;
                isRight = isRight * - 1; 
            }
        }
    }
    private void FreezeX()
    {
        isWallJump = false;
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
        //��ٸ��� ������ true
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //��ٸ����� ����� false
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;  
            ladderUp = false;
        }
    }
    public void TakeDamage(int damage)
    {
        playerHP -= damage;

        if (playerHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
