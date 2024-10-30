using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class WeaponComponent : MonoBehaviour
{
    public PlayerInputs Inputs;
    public List<Weapons> _weapons;
    private Weapons _currentWeapon;
    private int _currentWeaponIndex=0;
    private Transform _cameraTransform;
    public TextMeshProUGUI CurrentAmmoTMP;
    public TextMeshProUGUI MaxAmmoTMP;

    private void Awake()
    {
        _weapons =  GetComponentsInChildren<Weapons>(true).ToList();
        _currentWeapon = _weapons[_currentWeaponIndex];
        _cameraTransform = Camera.main.transform;
    }
    
    private void SwitchWeapon(int indexDelta) 
    {
        int nextIndex=_currentWeaponIndex + indexDelta;


        if (nextIndex >= _weapons.Count) 
        {
            nextIndex = 0;
        }
        if (nextIndex < 0) 
        {
            nextIndex = _weapons.Count - 1;
        }
        int cnt = _weapons.Count * 2;
        while (!_weapons[nextIndex].IsPicked) 
        {
            cnt -= 1;
            nextIndex += indexDelta;
            if (nextIndex >= _weapons.Count)
            {
                nextIndex = 0;
            }
            if (nextIndex < 0)
            {
                nextIndex = _weapons.Count - 1;
            }
            if (cnt == 0) 
            {
                return;
            }
        }
        _currentWeaponIndex = nextIndex;
        _currentWeapon.gameObject.SetActive(false);
        _currentWeapon.ResetWeaponState();
        _currentWeapon = _weapons[_currentWeaponIndex];
        _currentWeapon.gameObject.SetActive(true);
    }
    public void AddWeapon(int id, int Ammount,bool IsPicked) 
    {
        int index;
        var wp = GetWeaponById(id,out index);
    
        if (IsPicked) 
        {
            if (!wp.IsPicked)
            {
                wp.IsPicked = true;
                _currentWeaponIndex = index;
                SwitchWeapon(0);

            }

        }
        wp.AddAmmo(Ammount);
    }
    public Weapons GetWeaponById(int id, out int index) 
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (_weapons[i].Id == id)
            {
                index = i;
                return _weapons[i];
            }
        }
        index = 0;
        return null;
    }
    public void Shoot()
    {
        _currentWeapon.Fire();
    }
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 50f))
        {
                    Debug.DrawLine(_cameraTransform.position, hit.point, Color.blue);
           // if (Vector3.Distance(_cameraTransform.position, hit.point) > 1f)
            //{
                _currentWeapon.FirePoint.LookAt(hit.point);
                //Debug.Log("LookingHit");
           // }
        }
        else
        {
            _currentWeapon.FirePoint.LookAt(_cameraTransform.position + (_cameraTransform.forward * 30f));
        }
        if (Inputs.shoot) 
        {
            Shoot();
        }
        if (Inputs.nextWeapon) 
        {

            SwitchWeapon(1);
        }
        if (Inputs.previousWeapon) 
        {
            SwitchWeapon(-1);
        }
        if (CurrentAmmoTMP) 
        {
            CurrentAmmoTMP.text = _currentWeapon.CurrentAmmo.ToString();
        }
        if (MaxAmmoTMP) 
        {
            MaxAmmoTMP.text = _currentWeapon.MaxAmmp.ToString();
        }
    }

}
