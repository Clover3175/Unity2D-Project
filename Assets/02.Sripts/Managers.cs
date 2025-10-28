using UnityEngine;

public class Managers
{
    //��� �Ŵ��� ������Ʈ���� �θ� ������ �� �� ������Ʈ
    private static GameObject _root;

    //Ǯ�Ŵ���
    private static PoolManager _pool;

    private static void Init()
    {
        if (_root == null)
        {
            //�� ���� ������Ʈ ����(@Managers)
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }
    }
    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init();

            //���ο� ���� ������Ʈ ����
            GameObject obj = new GameObject(name);

            //�ش� ������Ʈ�� TŸ�� �Ŵ��� ������Ʈ �߰�
            manager = obj.AddComponent<T>();

            Object.DontDestroyOnLoad(obj);

            //@Managers ������ �ٿ��� ���� ����
            obj.transform.SetParent(_root.transform);
        }
    }
    public static PoolManager Pool
    {
        get 
        {
            CreateManager(ref _pool, "PoolManager");
            return _pool; 
        }
    }

}
