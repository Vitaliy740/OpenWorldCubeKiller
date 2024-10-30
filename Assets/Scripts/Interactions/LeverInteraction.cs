using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteraction : Interaction
{
    public EItems Item;
    public PlayerInventory Inventory;
    protected override void Awake()
    {
        base.Awake();
        Inventory = FindObjectOfType<PlayerInventory>();
    }
    public override void Interact()
    {
        //base.Interact();
        if (Inventory.CheckItem(Item))
        {
            InteractionEvent?.Invoke();
            GetComponent<Collider>().enabled = false;

        }
    }
}
