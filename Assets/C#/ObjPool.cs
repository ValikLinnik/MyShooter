using UnityEngine;
using System.Collections.Generic;

public static class PoolExtentions
{
    public static T GetInstance<T>(this T obj) where T : Component
    {
        return ObjPool.Instance.GetClone<T>(obj);
    }

    public static void PutInPool<T>(this T obj) where T : Component
    {
        obj.gameObject.SetActive(false);
    }
}

public class ObjPool
{
    #region SINGLETONE PART

    private static ObjPool _instance;

    public static ObjPool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ObjPool();
            return _instance;
        }
    }

    private ObjPool()
    {
        
    }

    #endregion

    private Dictionary<Component, List<Component>> _pool = new Dictionary<Component, List<Component>>();

    public T GetClone<T>(T prefab) where T : Component
    {
        if(_pool.ContainsKey(prefab))
        {
            var tempList = _pool[prefab];

            foreach (var item in tempList)
            {
                if(!item) continue;
                if(!item.gameObject.activeSelf) 
                {
                    return item as T;
                }
            }

            Component tempInstance = MonoBehaviour.Instantiate(prefab) as T;
            tempList.Add(tempInstance);

            return tempInstance as T;
        }
        else
        {
            _pool.Add(prefab as Component, new List<Component>());
            return GetClone<T>(prefab);
        }
    }

}


