using System;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private int playerHp = 3;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHp -= 1;
        }
    }
    public void Die()
    {
        if (playerHp < 0)
        {
            Destroy(gameObject);
        }
    }
}
