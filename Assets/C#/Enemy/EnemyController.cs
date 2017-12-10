using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour 
{
	[SerializeField]
    private Enemy[] _enemyPrefabs;

    [SerializeField]
    private Renderer[] _spawnPoints;

    [SerializeField, Range(1,10)]
    private int _quantityOfEnemys = 3;

    [SerializeField]
    private Transform _playerTransform;

    //private List<Enemy> _enemys = new List<Enemy>();
    private List<Transform> _usedTransforms = new List<Transform>();
    private int _quantity;

    private void LateUpdate()
    {
        EnemyHandler();
    }

    private void EnemyHandler()
    {
//        if(_enemys == null)
//        {
//            _enemys = new List<Enemy>();
//        }

//        if(_enemys == null) 
//        {
//            return;
//        }

//        foreach (var item in _enemys)
//        {
//            if(!item) continue;
//
//            if(item.gameObject.activeSelf) quantity++;
//        }

        if(_quantity >= _quantityOfEnemys)
        {
            return;
        }

        _usedTransforms.Clear();

        for (int i = _quantity; i < _quantityOfEnemys; i++)
        {
            AddEnemy();
        }
    }

    private void AddEnemy()
    {
        if(_enemyPrefabs == null || _enemyPrefabs.Length == 0) return;

        var tempTransform = GetPosition();

        if(!tempTransform)
        {
                Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "transform is null");
            return;
        }

        _usedTransforms.Add(tempTransform);

        var temp = GetEnemy();
        if(!temp) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "temp is null");
            return;
        }

        temp.Initialize(tempTransform, _playerTransform);
        if(UIController.Instance) UIController.Instance.AddMessage(string.Format("Added {0} enemys, current quatnity:{1}", _quantity, _quantityOfEnemys));
    }

    private Enemy GetEnemy()
    {
        var prefab = _enemyPrefabs.GetRandomItem();

        if(!prefab)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "prefab is null");
            return null;
        }

        var temp = prefab.GetInstance<Enemy>();
        if(!temp) return null;

        temp.EnemyDie += EnemyDie;
        _quantity++;
        return temp;
    }

    private void EnemyDie (Enemy enemy)
    {
        if(!enemy) return;
        enemy.EnemyDie -= EnemyDie;
        _quantity--;
        _quantityOfEnemys++;
    }

    private Transform GetPosition()
    {
        if(_spawnPoints.IsNullOrEmpty()) return null;

        foreach(var item in _spawnPoints)
        {
            if(!item) continue;
            if(!item.isVisible && !_usedTransforms.Contains(item.transform)) return item.transform;
        }

        return null;
    }
}
