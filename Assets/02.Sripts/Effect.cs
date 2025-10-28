using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    //이펙트를 재생
    public void PlayEffect()
    {
        //오브젝트 활성화
        gameObject.SetActive(true);

        //애니메이션 재생
        anim.Play("Effect");

        StartCoroutine(DisableAnimationCo());


    }
    IEnumerator DisableAnimationCo()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        //애니메이션 재생이 끝나면 풀에 다시 반환 
        Managers.Pool.ReturnPool(this);
    }
}
