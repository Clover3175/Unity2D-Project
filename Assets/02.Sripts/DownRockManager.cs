using System.Collections;
using UnityEngine;

public class DownRockManager : MonoBehaviour
{
    public static DownRockManager instance { get; private set; }

    private void Awake()
    {
       if (instance == null)
       {
           instance = this;
           DontDestroyOnLoad(gameObject);
       }
       else
       {
           Destroy(gameObject);
       }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReDownRock(DownRock downRock, float reTime)
    {
        StartCoroutine(ReDownRockCo(downRock, reTime));
    }

    IEnumerator ReDownRockCo(DownRock downRock, float reTime)
    {
        yield return new WaitForSeconds(reTime);

        downRock.gameObject.SetActive(true);

        downRock.transform.position = downRock.startPos;
        downRock.rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
