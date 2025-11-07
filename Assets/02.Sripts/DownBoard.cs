using System.Collections;
using UnityEngine;

public class DownBoard : MonoBehaviour
{
    [SerializeField] private float downSpeed = 3.0f;
    [SerializeField] private float backSpeed = 1f;
    [SerializeField] private float stopSpeed = 2f;

    private Vector2 strPos;

    private Rigidbody2D rb;

    private bool isUP = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        strPos = transform.position;
    }

    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Foot"))
        {
            rb.velocity = Vector2.down * downSpeed;

            isUP = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Foot"))
        {
            isUP = true;

            rb.velocity = Vector2.zero;

            StartCoroutine(UpBoard());

        }
    }
    IEnumerator UpBoard()
    {
        yield return new WaitForSeconds(stopSpeed);

        if (!isUP)
        {
            yield break;
        }

        while (Vector2.Distance(transform.position, strPos) > 0.05f)
        {
            if (!isUP) 
            { 
                yield break; 
            }

            transform.position = Vector2.Lerp(transform.position, strPos, backSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = strPos;
    }
}
