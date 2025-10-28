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
        
    }

    void Update()
    {
        
    }
}
//모든 상태의 공통부모, 개별상태는 이 클래스를 상속해서 필요한 메서드만 재정의
public class BaseState
{
    //자신이 속한 상태머신, 전이(ChangeState)를 하려면 상태머신이 필요함
    private StateMachine StateMachine;

    //외부에서 자기가 소속된 상태머신을 연결해주는 녀석이 있어야하니깐 그걸 메서드로 만듬

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.StateMachine = stateMachine;
    }
    //상태전이

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void LateUpdata() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
    public virtual void Transition() { }
}
