using System;
using UnityEngine;
using Zenject;
//using UnityStandardAssets.CrossPlatformInput;

public class BallUserControl : MonoBehaviour
{
    private Ball ball; // Reference to the ball controller.
    private PlayerInputs _inputs;
    private Vector3 move;
    // the world-relative desired move direction, calculated from the camForward and user input.

    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera
    private float _threshold = 0.01f;
    private float _cinemachineTargetPitch;
    private float _rotationVelocity;
    public Transform CinemachineCameraTarget;
    public float RotationSpeed = 1.0f;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 90.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -90.0f;
    
    [Inject]
    private void Construct(PlayerInputs inputs) 
    {
        _inputs = inputs;
    }
    private void Awake()
    {
        // Set up the reference.
        ball = GetComponent<Ball>();


        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use world-relative controls in this case, which may not be what the user wants, but hey, we warned them!
        }
    }
    private void LateUpdate()
    {
        //Debug.Log(Inputs.look.sqrMagnitude);
        if (_inputs.look.sqrMagnitude >= _threshold) 
        {
            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetPitch += _inputs.look.y * RotationSpeed * deltaTimeMultiplier;
            _rotationVelocity += _inputs.look.x * RotationSpeed * deltaTimeMultiplier;


            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);


            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, _rotationVelocity, 0.0f);

        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void Update()
    {
        // Get the axis and jump input.

        float h = _inputs.move.x;
        float v = _inputs.move.y;

        // calculate move direction
        if (cam != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = (v*camForward + h*cam.right).normalized;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = (v*Vector3.forward + h*Vector3.right).normalized;
        }
    }


    private void FixedUpdate()
    {
        // Call the Move function of the ball controller
        ball.Move(move, _inputs.jump,_inputs.crouch,_inputs.sprint);
        //jump = false;
    }
}

