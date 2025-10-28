using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    //����Ʈ�� ���
    public void PlayEffect()
    {
        //������Ʈ Ȱ��ȭ
        gameObject.SetActive(true);

        //�ִϸ��̼� ���
        anim.Play("Effect");

        StartCoroutine(DisableAnimationCo());


    }
    IEnumerator DisableAnimationCo()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        //�ִϸ��̼� ����� ������ Ǯ�� �ٽ� ��ȯ 
        Managers.Pool.ReturnPool(this);
    }
}
