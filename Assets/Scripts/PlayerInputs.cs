using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public OpenMenuDelegate Delegate;
    public Vector2 move=new Vector2();
    public Vector2 look;
    public bool jump=false;
    public bool crouch=false;
    public bool shoot=false;
    public bool nextWeapon = false;
    public bool previousWeapon = false;
    public bool interact = false;
    public event OnTransformIntoSphereDelegate TransformIntoSphereEvent;
    public event OnTransformIntoNormalShapeDelegate TransformIntoNormalShapeEvent;
    public bool sprint = false;
#if ENABLE_INPUT_SYSTEM
    private void OnMove( InputValue value)
    {
        move = value.Get<Vector2>();
        //Debug.Log(value.Get<Vector2>());
    }
    private void OnChangeWeapon(InputValue value) 
    {
        var vec=value.Get<Vector2>();
        //Debug.Log(vec.y<0f);
        nextWeapon = vec.y > 0f;
        previousWeapon = vec.y < 0f;
    }
    private void OnRun(InputValue value) 
    {
        sprint = value.isPressed;
        //Debug.Log("Run is : " + sprint);
    }
    private void OnLook(InputValue value) 
    {
        look = value.Get<Vector2>();
    }
    private void OnInteract(InputValue value) 
    {
        interact = value.isPressed;
    }
    private void OnJump(InputValue value) 
    {
        jump = value.isPressed;
        //Debug.Log("jump is : " + sprint);
    }
    private void OnTransformIntoBall(InputValue value) 
    {
        Debug.Log("Transform into sphere");
        TransformIntoSphereEvent?.Invoke();
    }
    private void OnTransformIntoCone(InputValue value) 
    {
        //Debug.Log("Transform into cone");

    }
    private void OnTransformIntoNormalShape(InputValue value) 
    {
        Debug.Log("Transform into NormalShape");
        TransformIntoNormalShapeEvent?.Invoke();
    }
    
    private void OnCrouch(InputValue value) 
    {
        crouch = value.isPressed;
    }
    private void OnShoot( InputValue value) 
    {
        shoot = value.isPressed;
    }
#endif
}
public delegate void OpenMenuDelegate() ;
