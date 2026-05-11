using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("Car Properties")]
    public float motorTorque = 2000f;
    public float brakeTorque = 2000f;
    public float maxSpeed = 20f;
    public float steeringRange = 30f;
    public float steeringRangeAtMaxSpeed = 10f;
    public float centreOfGravityOffset = -1f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;

    private CarInputActions carControls;

    void Awake()
    {
        carControls = new CarInputActions(); 
    }

    void OnEnable()
    {
        carControls.Enable();
    }

    void OnDisable()
    {
        carControls.Disable();
    }
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        Vector3 centerOfMass = rigidBody.centerOfMass;
        centerOfMass.y += centreOfGravityOffset;
        rigidBody.centerOfMass = centerOfMass;

        wheels = GetComponentsInChildren<WheelControl>();
    }

    void FixedUpdate()
    {
        Vector2 inputVector = carControls.Car.Movement.ReadValue<Vector2>();

        bool isHandbrake = carControls.Car.Handbrake.IsPressed();

        float vInput = inputVector.y; 
        float hInput = inputVector.x; 
        
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed)); 

        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isHandbrake && !wheel.motorized)
            {
                WheelFrictionCurve sideways = wheel.WheelCollider.sidewaysFriction;
                sideways.stiffness = 0.4f; 
                wheel.WheelCollider.sidewaysFriction = sideways;

                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = brakeTorque * 1.5f; 
            } else {
                WheelFrictionCurve sideways = wheel.WheelCollider.sidewaysFriction;
                sideways.stiffness = 1.5f;
                wheel.WheelCollider.sidewaysFriction = sideways;
                if (isAccelerating)
                {
                    if (wheel.motorized)
                    {
                        wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                    }
                    wheel.WheelCollider.brakeTorque = 0f;
                }
                else
                {
                    wheel.WheelCollider.motorTorque = 0f;
                    wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                }
            }
        }
    }
}
