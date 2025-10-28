using UnityEngine;

public class Managers
{
    //모든 매니저 오브젝트들이 부모 역할을 할 빈 오브젝트
    private static GameObject _root;

    //풀매니저
    private static PoolManager _pool;

    private static void Init()
    {
        if (_root == null)
        {
            //빈 게임 오브젝트 생성(@Managers)
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad(_root);
        }
    }
    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init();

            //새로운 게임 오브젝트 생성
            GameObject obj = new GameObject(name);

            //해당 오브젝트에 T타입 매니저 컴포넌트 추가
            manager = obj.AddComponent<T>();

            Object.DontDestroyOnLoad(obj);

            //@Managers 밑으로 붙여서 게층 정리
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
