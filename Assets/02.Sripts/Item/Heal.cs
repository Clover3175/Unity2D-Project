using UnityEngine;

public class Heal : MonoBehaviour
{
    private PlayerStats player;

    [SerializeField] private int heal = 1;

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
            if(player.PlayerHP < player.MaxHP)
            {
                player.TakeHeal(heal);
                gameObject.SetActive(false);
            }
        }
    }
}
