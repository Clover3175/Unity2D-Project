using System;
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
        curState.Enter();
    }

    void Update()
    {
        //���� ���°� �ؾ��� �ൿ ����
        curState.Update();
        //���� ����(����) ����Ȯ���ϰ�, �ʿ��ϸ� �ٸ� ���·� �ٲٴ� ������ �Ǵ�
        curState.Transition();
    }

    private void LateUpdate()
    {
        curState.LateUpdata();
    }

    private void FixedUpdate()
    {
        curState.FixedUpdate();
    }
    public void InitState(string stateName)
    {
        //��ųʸ����� �ش� �̸��� ��ü�� ã�Ƽ� ���� ���·� �����Ѵ�.
        curState = stateDic[stateName];
        curState.Awake();
    }
    //���¸� ���¸ӽſ� ���
    public void AddState(string stateName, BaseState state)
    {
        //�� ���°� ��� ���¸ӽ��� �Ҽ�����
        state.SetStateMachine(this);
        //�����̸��̶� ��ü�� ���
        stateDic.Add(stateName, state);
    }
    //����
    public void ChangeState(string stateName)
    {
        //���� ���¿��� ������
        curState.Exit();
        //��ųʸ����� �� ���¸� ���� ���� ���·� ��ü
        curState = stateDic[stateName];
        //��ü�� ���¸� ����
        curState.Enter();
    }
    public void InitState<T>(T stateType) where T : Enum
    {
        //���� InitState�� ȣ���� �� ���� ���ڿ��� �Ű������� �ִ� ��� Enum�� ����ϰ� �˾Ƽ� ���ڿ��� ��ȯ��
        //�̷��� ��Ÿ�� ���輺�� �پ��. �ֳ��ϸ� ���ڿ��� �ٸ��� ��Ÿ�� ġ�� �̸� �˷���
        InitState(stateType.ToString());
    }
    public void AddState<T>(T stateType, BaseState state) where T : Enum
    {
        AddState(stateType.ToString(), state);
    }
    public void ChangeState<T>(T stateType) where T : Enum
    {
        ChangeState(stateType.ToString());
    }
}
//��� ������ ����θ�, �������´� �� Ŭ������ ����ؼ� �ʿ��� �޼��常 ������
public class BaseState
{
    //�ڽ��� ���� ���¸ӽ�, ����(ChangeState)�� �Ϸ��� ���¸ӽ��� �ʿ���
    private StateMachine stateMachine;

    //�ܺο��� �ڱⰡ �Ҽӵ� ���¸ӽ��� �������ִ� �༮�� �־���ϴϱ� �װ� �޼���� ����

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    //��������
    protected void ChangeState(string stateName)
    {
        //���ο� ����� ���¸ӽ����� �� �̸� ���·� �ٲ�
        stateMachine.ChangeState(stateName);
    }
    protected void ChangeState<T>(T stateType) where T : Enum
    {
        ChangeState(stateType.ToString());
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void LateUpdata() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
    public virtual void Transition() { }
    public virtual void Awake() { }
}
