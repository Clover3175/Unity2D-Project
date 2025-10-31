
using UnityEngine;

public class BulletLoad : MonoBehaviour
{
    private PlayerStats player;

    [SerializeField] private int bulletLoad = 1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        player = collision.GetComponent<PlayerStats>();

        if (player != null)
        {
            if (player.BulletTimes < player.BulletMax)
            {
                player.TakeBulllet(bulletLoad);
                gameObject.SetActive(false);

            }
        }
    }
}
