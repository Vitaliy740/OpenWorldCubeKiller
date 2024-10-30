using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimeDestroyer : MonoBehaviour
{
    public float TimeToDestruct = 15f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, TimeToDestruct);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
