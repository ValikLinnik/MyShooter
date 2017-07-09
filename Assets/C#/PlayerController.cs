using UnityEngine;
using System.Collections;
using MyNamespace;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
    [SerializeField]
    private BaseGun[] _guns;

    [SerializeField]
    private HealthComponent _healthComponent;

    [SerializeField]
    private FirstPersonController _firstPersonController;

    [SerializeField]
    private HeadLight _torge;

    private int _currentGunIndex;
    private Queue<Bonus> _cargos = new Queue<Bonus>();
    private int _hpQuantity;

    private void Start()
    {
        if(_healthComponent) _healthComponent.OnDamageGet += OnDamageGet;  
        if(UIController.Instance) UIController.Instance.AddMessage(string.Format("Player:{0} join the game", gameObject.name));
        SetActiveGun(0,true);
    }

    private void OnDestroy()
    {
        if(_healthComponent) _healthComponent.OnDamageGet -= OnDamageGet;  
    }

    private void OnDamageGet (float arg1, float arg2)
    {
        if(UIController.Instance)
        { 
            UIController.Instance.SetLifeValue(arg2);
            if (arg1 > 0) 
            {
                UIController.Instance.BloodyScreen();
                UIController.Instance.AddMessage(string.Format("Player got damage:{0}", arg1));
            }

            if(arg2 <= 0) 
            {
                UIController.Instance.SetActiveGameOverScreen(true);
                if(_firstPersonController) _firstPersonController.Cursor(false);
            }
        }
    }

    private void LateUpdate()
    {
        ScrollWheelHandler();
        UserInputHandler();
    }

    private void UserInputHandler()
    {
        if(Input.GetKeyDown(KeyCode.L) && _torge) _torge.IsOn = !_torge.IsOn;

        if(Input.GetKeyDown(KeyCode.H) && _hpQuantity > 0) 
        {
            _hpQuantity--;
            if(_healthComponent) _healthComponent.CurrentHealthValue = _healthComponent.MaxHealthValue;
            if(UIController.Instance) 
            {
                UIController.Instance.SetLifeValue(_healthComponent.MaxHealthValue);
                UIController.Instance.SetHP(_hpQuantity);
            }
        }

        if(Input.GetKeyDown(KeyCode.C) && _cargos.Count > 0) 
        {
            var temp = _cargos.Dequeue();

            if(temp == null) return;
            temp.SetRandomValue();

            GetBonus(temp);

            if(UIController.Instance) 
            {
                UIController.Instance.SetCargoQuantity(_cargos.Count);
            }
        }
    }

    private void ScrollWheelHandler()
    {
        if(_guns == null || _guns.Length < 2)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "array not set");
            return;
        }

        var temp = Input.GetAxis("Mouse ScrollWheel");

        if(temp == 0) return;

        SetActiveGun(_currentGunIndex, false);

        _currentGunIndex = temp > 0 ? _currentGunIndex + 1: _currentGunIndex - 1;
        _currentGunIndex = _currentGunIndex < 0 ? _guns.Length - 1 : _currentGunIndex;
        _currentGunIndex = _currentGunIndex >= _guns.Length ? 0 : _currentGunIndex;

        SetActiveGun(_currentGunIndex, true);

    }

    private void SetActiveGun(int index, bool flag)
    {
        if(_guns == null || _guns.Length == 0 || index < 0 || index >= _guns.Length)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "shit happens");
            return;
        }

        _guns[index].gameObject.SetActive(flag);

        if(flag && UIController.Instance) 
        {
            UIController.Instance.SetWeaponName(_guns[index].Name);
            UIController.Instance.SetBulletsQuantity(_guns[index].CurrentBulletQuantity, _guns[index].CurrentBulletsCage);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!other) return;

        var bonus = other.gameObject.GetComponent<Bonus>();

        if(!bonus) return;

        GetBonus(bonus);
    }

    private void GetBonus(Bonus bonus)
    {
        if(!bonus) return;

        ShowBonusInfo(bonus);

        if(bonus.Type == BonusType.Ammo)
        {
            if(_guns != null) _guns[(int)bonus.AmmoType].TotalBulletsQuantity += (int)bonus.Value;
        }   
        else 
            if(bonus.Type == BonusType.Cargo)
            {
                _cargos.Enqueue(bonus);

                if(UIController.Instance) UIController.Instance.SetCargoQuantity(_cargos.Count);
            }
            else
                if(bonus.Type == BonusType.HP)  
                {
                    _hpQuantity++; 
                    if(UIController.Instance) UIController.Instance.SetHP(_hpQuantity);
                }

        bonus.PickedBonus();
        bonus.PutInPool();

    }

    private void ShowBonusInfo(Bonus bonus)
    {
        if(!bonus) return;

        if(UIController.Instance) UIController.Instance.AddMessage(string.Format(" You picked:{0} {1}:{2}", bonus.Type, (bonus.Type == BonusType.Ammo ? bonus.AmmoType.ToString() : string.Empty), bonus.Value.ToString()));
    }

}
