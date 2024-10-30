using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChangeShapeController : MonoBehaviour
{
    public PlayerInputs Inputs;
    public PositionConstraint CapsuleConstraint;
    public GameObject CapsuleRootView;
    public GameObject SphereObject;
    public GameObject CapsuleObject;
    public GameObject CapsuleCinemachine;
    public GameObject SphereCinemachine;
    private Transform _mainCameraTransform;
    private EShapes _currentShape=EShapes.Capsule;
    
    // Start is called before the first frame update
    void Awake()
    {
        Inputs.TransformIntoSphereEvent += TransformIntoSphere;
        Inputs.TransformIntoNormalShapeEvent += TransformIntoNormalShape;
        _mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    private void TransformIntoSphere() 
    {
        Debug.Log("Transforming sphere");
        if (_currentShape == EShapes.Sphere) return;
        SphereObject.transform.parent = null;
        SphereObject.SetActive(true);
        CapsuleConstraint.constraintActive = true;
        CapsuleRootView.SetActive(false);
        CapsuleObject.SetActive(false);
        CapsuleCinemachine.SetActive(false);
        SphereCinemachine.SetActive(true);
        _currentShape = EShapes.Sphere;

    }
    private void TransformIntoNormalShape() 
    {
        Debug.Log("Transforming normal");
        if (_currentShape == EShapes.Capsule) return;
        SphereObject.transform.parent = transform;
        SphereObject.SetActive(false);
        SphereCinemachine.SetActive(false);
        CapsuleCinemachine.SetActive(true);
        CapsuleConstraint.constraintActive = false;
        StartCoroutine(MoveCameraToNormalFormView());
    }
    private IEnumerator MoveCameraToNormalFormView() 
    {
        while (Vector3.Distance(CapsuleRootView.transform.position, _mainCameraTransform.position) > 1f) 
        {
            yield return null;
        }
        CapsuleRootView.SetActive(true);

        CapsuleObject.SetActive(true);
        _currentShape = EShapes.Capsule;
    }
}
public enum EShapes 
{
    Capsule,
    Sphere,
    Cone
}
public delegate void OnTransformIntoSphereDelegate();
public delegate void OnTransformIntoNormalShapeDelegate();
