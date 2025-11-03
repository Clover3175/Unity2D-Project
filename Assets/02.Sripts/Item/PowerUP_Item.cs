using UnityEngine;

public class PowerUP_Item : MonoBehaviour
{
    private PlayerStats playerStats;

    public BulletUP bulletUPPrefab;

    public BulletEXUP bulletEXUPPrefab;

    void Start()
    {

    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.TakePowerUP(bulletUPPrefab);
            playerStats.TakeEXUP(bulletEXUPPrefab);
            gameObject.SetActive(false);
            playerStats.UpYes = true;
            playerStats.ExUpYes = true;
            if (playerStats.UpTime <= playerStats.UpTimeMax )
            {
                playerStats.UpTime = 0.0f;
            }
        }
    }
}