using UnityEngine;
using UnityEngine.Windows.Speech;

public class LadderDown : MonoBehaviour
{
    public Collider2D platformCollider;
    public Collider2D platformColliderTwo;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformCollider, true);
                Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformColliderTwo, true);
 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformCollider, false);
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), platformColliderTwo, false);
        }
    }
}
