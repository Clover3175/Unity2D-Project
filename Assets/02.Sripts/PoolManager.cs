
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
        if (Instance == null)               //ó�� ����� ���� �⺻������ Instance�� null�̴�.
        {
            Instance = this;                //�� �ڽ��� �̱������� ���
            DontDestroyOnLoad(gameObject);  //���� �ٲ� �ı����� ����
        }
        else
        {
            Destroy(gameObject);            //�ߺ��� ����
        }
    }
    //Ǯ ���
    public void CreatPool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return;  

        string key = prefab.name;   //key�� ������ �̸�����
        if (pools.ContainsKey(key)) return;  //���� �̸�(key)�� Ǯ�� ������ ����x 

        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent)); //���ο� Ǯ�� ����� ��ųʸ��� ���
    }
    //Ǯ���� �ϳ� ����
    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        //����Ҷ� ���� ������ �̸����� Ǯ�� ã��
        //ã���� �ش��ϴ� ��(����)�� box��� ������ ���, ������ null
        if(!pools.TryGetValue(prefab.name, out var box))
        {
            return null;       
        }

        var pool = box as ObjectPool<T>;  //oject ���� Ÿ������ ����� Ǯ�� ���� ��׸� Ÿ������ ĳ���� 

        if (pool != null)
        {
            //Ÿ�� ��ȯ�� �����ߴٸ� Dequeue()�� �ϳ������� Ȱ��ȭ�� ä�� ��ȯ
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
            //��� Ǯ���� ������ �ʴ´ٸ� �׳� ����
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
