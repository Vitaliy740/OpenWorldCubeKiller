using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : BaseObjectScene
{
    public UnityEvent InteractionEvent;
    public string InteractionText = "Использовать";
    public virtual void Interact() 
    {
        InteractionEvent?.Invoke();
    }
}
