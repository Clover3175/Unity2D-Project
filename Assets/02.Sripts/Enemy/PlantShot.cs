using System.Collections;
using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlantShot : MonoBehaviour
{
    [Header("������")]
    [SerializeField] private PlantBullet plantBulletPrefab;

    [Header("�߻�����, �߻� ����, ���� �߻� �ð�")]
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float shotRate = 0.2f;
    [SerializeField] private float nextShotTime;

    [Header("�߻� ���� ��, �߻� ��� ���̾�")]
    [SerializeField] private float shotLine = 5.0f;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Transform target;

    private SpriteRenderer sprite;

    private SpriteRenderer plantSprite;

    private PlantEnemy plantEnemy;
    private Animator anim;

    private bool isPlayer;

    private Vector2 dir = Vector2.left;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        plantSprite = transform.parent.GetComponent<SpriteRenderer>();

        plantEnemy = transform.parent.GetComponent<PlantEnemy>();
        anim = plantEnemy.GetComponent<Animator>();
    }

    void Start()
    {
        Managers.Pool.CreatPool(plantBulletPrefab, 100);
    }

    void Update()
    {
       sprite.flipX = plantSprite.flipX;
       
       Vector2 localPosition = shotPoint.localPosition;
       
       if (plantSprite.flipX)
       { 
           localPosition.x = Mathf.Abs(localPosition.x);
       }
       else if (!plantSprite.flipX)
       {
           localPosition.x = -Mathf.Abs(localPosition.x);
       }
       
       shotPoint.localPosition = localPosition;
       
       dir = sprite.flipX ? Vector2.right : Vector2.left;


    }
    private void FixedUpdate()
    {
        isPlayer = Physics2D.Raycast(shotPoint.position, dir, shotLine, playerLayer);

        if (isPlayer && Time.time >= nextShotTime)
        {
            nextShotTime= Time.time + shotRate;
            StartCoroutine(AttackAnim());

        }
        if (!isPlayer)
        {
            anim.SetBool("PlantAttack", false);

        }
    }
    private void PlantShotting()
    {
        PlantBullet plantBullet = Managers.Pool.GetFromPool(plantBulletPrefab);

        plantBullet.transform.SetLocalPositionAndRotation(shotPoint.position, Quaternion.identity);

        plantBullet.GetComponent<SpriteRenderer>().flipX = plantSprite.flipX;

        if (plantSprite.flipX)
        {
            plantBullet.dir = Vector2.right;
        }
        else if (!plantSprite.flipX)
        {
            plantBullet.dir = Vector2.left;
        }
    }
    IEnumerator AttackAnim()
    {
      
            anim.SetBool("PlantAttack", true);

            yield return new WaitForSeconds(0.7f);

            PlantShotting();
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(shotPoint.position, dir * shotLine);
    }

}
