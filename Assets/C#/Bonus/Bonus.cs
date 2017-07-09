using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public enum BonusType
{
    Ammo,
    HP,
    Cargo
}

public enum AmmoType
{
    SimpleGun,
    RayCastGun,
    BombGun
}

public class Bonus : MonoBehaviour 
{
    [SerializeField]
    private BonusType _type;

    [SerializeField]
    private AmmoType _ammoType;

    [SerializeField]
    private float _value;

    public float Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
        }
    }

    public BonusType Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    public AmmoType AmmoType
    {
        get
        {
            return _ammoType;
        }

        set
        {
            _ammoType = value;
        }
    }

    public event Action<Bonus> OnBonusTake;

    private void OnBonusTakeHandler()
    {
        if(OnBonusTake != null) OnBonusTake(this);
    }

    public void PickedBonus()
    {
        OnBonusTakeHandler();
    }

    public void SetRandomValue()
    {
        _type = (BonusType)Random.Range(0,2);
        if(_type == BonusType.Ammo) 
        {
            _ammoType = (AmmoType)Random.Range(0, 3);
            _value = Random.Range(10,20);
        }

        Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", ToString());
    }

    public override string ToString()
    {
        return string.Format("[Bonus: Type={0}, AmmoType={1}]", Type, AmmoType);
    }
}
