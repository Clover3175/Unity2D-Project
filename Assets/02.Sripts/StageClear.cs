using UnityEngine;

public class StageClear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.StageClearUI();

            UIManager.Instance.ScoreUI();

            UIManager.Instance.LifeUI();

            UIManager.Instance.TotalScoreUI();
        }
    }
}
