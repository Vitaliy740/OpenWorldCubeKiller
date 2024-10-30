using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : BasePickUp
{
    [SerializeField]
    private int _weaponToId;
    [SerializeField]
    private int _ammoToAdd;
    private WeaponComponent _playerWeapons;
    [SerializeField]
    private bool isPicked=true;
    // Start is called before the first frame update
    void Start()
    {
        _playerWeapons = FindObjectOfType<WeaponComponent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            if (_playerWeapons) 
            {
                if (_playerWeapons.GetWeaponById(_weaponToId,out int k).IsMaxAmmo) return;
                _playerWeapons.AddWeapon(_weaponToId, _ammoToAdd, isPicked);
                Destroy(gameObject, 5f);
                Manager.PlaySound(Manager.PickUpSound);
                gameObject.SetActive(false);

            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
