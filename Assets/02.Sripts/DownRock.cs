using System.Collections;
using UnityEngine;

public class DownRock : MonoBehaviour
{
    [SerializeField] private float downTime;
    [SerializeField] private float outTime;
    [SerializeField] private float reTime;

    public Rigidbody2D rb;

    public Vector2 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Foot"))
        {
            StartCoroutine(IsDownRock());
        }
    }
    IEnumerator IsDownRock()
    {
        yield return new WaitForSeconds(downTime);

        rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(outTime);

        gameObject.SetActive(false);

        DownRockManager.instance.ReDownRock(this, reTime);

        
    }
}
