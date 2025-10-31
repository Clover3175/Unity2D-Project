using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Queue<T> pool = new Queue<T>();

    public Transform Root { get; private set; }  //풀을 담아둘 부모 오브젝트 

    //프리팹 만들고 넣을 부모 오브젝트 생성
    //prefab: 복제할 프리팹, initcount: 복제할 갯수, parent: Root를 어떤 부모에 둘지
    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.prefab = prefab;

        //풀을 담아둘 부모오브젝트(Root) 생성 -> 이름 : 프리팹이름_Pool
        Root = new GameObject($"{prefab.name}_Pool").transform;

        if (parent != null)
        {
            Root.SetParent(parent, false);
        }

        //복제할 갯수 만큼 만들어서 큐에 넣어두기
        for (int i = 0; i < initCount; i++)
        {
            var init = Object.Instantiate(prefab, Root); //Root 자식으로 생성
            init.name = prefab.name;   //프리팹 이름 그대로
            init.gameObject.SetActive(false);  //꺼진상태로
            pool.Enqueue(init); //큐에 넣기
        }
    }
    //큐에서 꺼내서 사용
    public T Dequeue()
    {
        if (pool.Count == 0) return null;

        var inst = pool.Dequeue(); //큐에서 하나 빼고
        inst.gameObject.SetActive(true);
        return inst;
    }
    //사용했으면 다시 큐에 넣기
    public void Enqueue(T instace)
    {
        if (instace == null) return;

        instace.gameObject.SetActive(false);
        pool.Enqueue(instace);
    }
   
}
