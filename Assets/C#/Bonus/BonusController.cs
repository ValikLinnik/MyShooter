using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour 
{
	#region SERIALIZE FIELDS

    [SerializeField]
    private Bonus[] _bonusPrefabs;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private float _minDelay = .1f;

    [SerializeField]
    private float _maxDelay = 3f;

    #endregion

    private void Start()
    {
        StartBonus();
    }

    private void StartBonus()
    {
        if(!gameObject.activeSelf || !gameObject.activeInHierarchy) 
        {
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "Obj dont active.");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(WaitAndSetBonus(Random.Range(_minDelay, _maxDelay)));
    }

    private IEnumerator WaitAndSetBonus(float  time)
    {
        yield return new WaitForSeconds(time);

        if(_spawnPoints.IsNullOrEmpty() || _bonusPrefabs.IsNullOrEmpty()) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "spawn points is null");
            yield break;
        }

        var tempTransform = _spawnPoints.GetRandomItem<Transform>();

        if(tempTransform == null) 
        {
            yield break;
        }

        var tempBonus = _bonusPrefabs.GetRandomItem<Bonus>();

        if(tempBonus == null) 
        {
            yield break;
        }

        tempBonus = tempBonus.GetInstance<Bonus>();

        if(!tempBonus) yield break;

        var pos = tempBonus.transform.position;
        pos.x = tempTransform.position.x;
        pos.z = tempTransform.position.z;
        tempBonus.transform.position = pos;
        tempBonus.gameObject.SetActive(true);

        if(tempBonus.Type == BonusType.Ammo) 
        {
            AmmoType temp = AmmoType.BombGun;
            tempBonus.AmmoType = temp.GetRandomItem<AmmoType>();
            if(tempBonus.AmmoType == AmmoType.BombGun) tempBonus.Value = 5;
            else tempBonus.Value = 10;
        }
        else if(tempBonus.Type == BonusType.Cargo)
        {
            tempBonus.Value = 1;
        }
        else
        {
            tempBonus.Value = 10;
        }

        tempBonus.OnBonusTake += OnBonusTake;
    }

    private void OnBonusTake(Bonus sender)
    {
        if(!sender) return;
        sender.OnBonusTake -= OnBonusTake;
        StartBonus();
    }
}
