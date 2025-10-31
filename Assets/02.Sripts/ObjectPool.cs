using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Queue<T> pool = new Queue<T>();

    public Transform Root { get; private set; }  //Ǯ�� ��Ƶ� �θ� ������Ʈ 

    //������ ����� ���� �θ� ������Ʈ ����
    //prefab: ������ ������, initcount: ������ ����, parent: Root�� � �θ� ����
    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.prefab = prefab;

        //Ǯ�� ��Ƶ� �θ������Ʈ(Root) ���� -> �̸� : �������̸�_Pool
        Root = new GameObject($"{prefab.name}_Pool").transform;

        if (parent != null)
        {
            Root.SetParent(parent, false);
        }

        //������ ���� ��ŭ ���� ť�� �־�α�
        for (int i = 0; i < initCount; i++)
        {
            var init = Object.Instantiate(prefab, Root); //Root �ڽ����� ����
            init.name = prefab.name;   //������ �̸� �״��
            init.gameObject.SetActive(false);  //�������·�
            pool.Enqueue(init); //ť�� �ֱ�
        }
    }
    //ť���� ������ ���
    public T Dequeue()
    {
        if (pool.Count == 0) return null;

        var inst = pool.Dequeue(); //ť���� �ϳ� ����
        inst.gameObject.SetActive(true);
        return inst;
    }
    //��������� �ٽ� ť�� �ֱ�
    public void Enqueue(T instace)
    {
        if (instace == null) return;

        instace.gameObject.SetActive(false);
        pool.Enqueue(instace);
    }
   
}
