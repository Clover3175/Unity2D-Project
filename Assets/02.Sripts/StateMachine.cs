using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //���� �̸��� Ű, ������ ��ü�� ������ ����
    public Dictionary<string, BaseState> stateDic = new Dictionary<string, BaseState>();

    private BaseState curState;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
//��� ������ ����θ�, �������´� �� Ŭ������ ����ؼ� �ʿ��� �޼��常 ������
public class BaseState
{
    //�ڽ��� ���� ���¸ӽ�, ����(ChangeState)�� �Ϸ��� ���¸ӽ��� �ʿ���
    private StateMachine StateMachine;

    //�ܺο��� �ڱⰡ �Ҽӵ� ���¸ӽ��� �������ִ� �༮�� �־���ϴϱ� �װ� �޼���� ����

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.StateMachine = stateMachine;
    }
    //��������

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void LateUpdata() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
    public virtual void Transition() { }
}
