using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemInteraction : Interaction
{
    public EItems Item;
    public PlayerInventory Inventory;

    protected override void Awake()
    {
        base.Awake();
        Inventory = FindObjectOfType<PlayerInventory>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Interact()
    {
        base.Interact();
        Inventory.AddItem(Item);
        InteractionEvent?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
