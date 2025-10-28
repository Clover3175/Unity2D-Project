using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [Header("ü��, �̵�, ����, ������")]
    [SerializeField] private int enemyHp = 3;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jump = 2.0f;
    [SerializeField] private int damage = 1;

    [Header("�� üũ")]
    [SerializeField] private float checkLine = 1.0f;
    [SerializeField] private LayerMask groundLayer;

    //[Header("�÷��̾� ����")]
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
        //�̵�
        rb.velocity = new Vector2(nextMove * speed, rb.velocity.y);

        //�Ʒ� Ground üũ
        groundVec = new Vector2(rb.position.x + nextMove, rb.position.y);
        isGrounded = Physics2D.Raycast(groundVec, Vector2.down, checkLine, groundLayer);

        //�տ� Ground üũ
        isWall = Physics2D.Raycast(rb.position, Vector2.right * nextMove, checkLine, groundLayer);

        //���������ų� �տ� Ground�� ������ ���ư���
        if (!isGrounded || isWall)
        {
            nextMove = nextMove * -1;
            CancelInvoke();  //���� �۵� ���� ��� Invoke�Լ��� ���ߴ� �Լ��̴�.
            Invoke("Think", thinkTime); //������������ ������ �ٲٰ� �ٽ� Invoke
        }
    }
    //����ռ� : �ڽ��� ������ ȣ���ϴ� �Լ�
    private void Think()
    {
        nextMove= Random.Range(-1, 2);

        Invoke("Think", thinkTime);   //����Լ��� ������ ���� ����ϴ� ���� ����

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
