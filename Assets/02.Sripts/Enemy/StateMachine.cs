using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //상태 이름을 키, 상태의 객체를 값으로 저장
    public Dictionary<string, BaseState> stateDic = new Dictionary<string, BaseState>();

    private BaseState curState;

    void Start()
    {
        curState.Enter();
    }

    void Update()
    {
        //현재 상태가 해야할 행동 실행
        curState.Update();
        //상태 전이(변경) 조건확인하고, 필요하면 다른 상태로 바꾸는 로직을 판단
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
        //딕셔너리에서 해당 이름의 객체를 찾아서 현재 상태로 설정한다.
        curState = stateDic[stateName];
        curState.Awake();
    }
    //상태를 상태머신에 등록
    public void AddState(string stateName, BaseState state)
    {
        //이 상태가 어느 상태머신의 소속인지
        state.SetStateMachine(this);
        //상태이름이랑 객체를 등록
        stateDic.Add(stateName, state);
    }
    //변경
    public void ChangeState(string stateName)
    {
        //현재 상태에서 나가기
        curState.Exit();
        //딕셔너리에서 새 상태를 꺼내 현재 상태로 교체
        curState = stateDic[stateName];
        //교체한 상태를 실행
        curState.Enter();
    }
    public void InitState<T>(T stateType) where T : Enum
    {
        //위의 InitState을 호출할 때 직접 문자열을 매개변수로 넣는 대신 Enum을 사용하고 알아서 문자열로 반환함
        //이러면 오타의 위험성이 줄어듬. 왜냐하면 문자열과 다르게 오타를 치면 미리 알려줌
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
//모든 상태의 공통부모, 개별상태는 이 클래스를 상속해서 필요한 메서드만 재정의
public class BaseState
{
    //자신이 속한 상태머신, 전이(ChangeState)를 하려면 상태머신이 필요함
    private StateMachine stateMachine;

    //외부에서 자기가 소속된 상태머신을 연결해주는 녀석이 있어야하니깐 그걸 메서드로 만듬

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    //상태전이
    protected void ChangeState(string stateName)
    {
        //내부에 저장된 상태머신한테 이 이름 상태로 바꿈
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
