using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private PlayerStats player;

    [SerializeField] private int damage;

    [SerializeField] private Transform playerTrans;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        player = collision.gameObject.GetComponent<PlayerStats>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            
        }
    }
}
