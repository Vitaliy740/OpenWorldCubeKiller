using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    private bool _enabled;
    public bool Enabled 
    {
        get { return _enabled; }
        private set { }
    }
    public virtual void On ()
    {
        _enabled = true;
    }
    public virtual void Off() 
    {
        _enabled = false;
    }
}
