using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : BasePickUp
{
    public EItems ItemToAdd;
    public PlayerInventory Inventory;
    // Start is called before the first frame update
    
    void Start()
    {
        Inventory = FindObjectOfType<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Inventory.AddItem(ItemToAdd);
            PickEvent?.Invoke();
            //gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
