
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, object> pools = new Dictionary<string, object>();

    public Dictionary<string, object> Pools
    {
        get { return pools; }
        private set { pools = value; } 
    }

    private void Awake()
    {
        if (Instance == null)               //처음 실행될 때는 기본적으로 Instance는 null이다.
        {
            Instance = this;                //내 자신을 싱글톤으로 등록
            DontDestroyOnLoad(gameObject);  //씬이 바뀌어도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);            //중복은 삭제
        }
    }
    //풀 등록
    public void CreatPool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return;  

        string key = prefab.name;   //key를 프리팹 이름으로
        if (pools.ContainsKey(key)) return;  //같은 이름(key)의 풀이 있으면 생성x 

        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent)); //새로운 풀을 만들어 딕셔너리에 등록
    }
    //풀에서 하나 꺼냄
    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        //등록할때 썻던 프리팹 이름으로 풀을 찾음
        //찾으면 해당하는 값(내용)을 box라는 변수로 출력, 없으면 null
        if(!pools.TryGetValue(prefab.name, out var box))
        {
            return null;       
        }

        var pool = box as ObjectPool<T>;  //oject 변수 타입으로 저장된 풀을 원래 재네릭 타입으로 캐스팅 

        if (pool != null)
        {
            //타입 변환에 성공했다면 Dequeue()로 하나꺼내서 활성화된 채로 반환
            return pool.Dequeue();
        }
        else 
        {
            return null;
        }
    }
    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        if (instance == null) return;

        if (!pools.TryGetValue(instance.gameObject.name, out var box))
        {
            //어느 풀에도 속하지 않는다면 그냥 제거
            Destroy(instance.gameObject);
            return;
        }

        var pool = box as ObjectPool<T>;

        if (pool != null)
        {
            pool.Enqueue(instance);
        }
    }
}
