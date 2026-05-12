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

    [Header("Arcade Drift")]
    public float normalGrip = 1.1f;
    public float highSpeedGrip = 0.85f;

    private WheelControl[] wheels;
    private Rigidbody rigidBody;
    private CarManager manager;
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
        manager = GetComponent<CarManager>();

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

        bool hasFuel = manager.fuel > 0f;
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        bool isDrifting = isHandbrake;

        foreach (var wheel in wheels)
        {
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            WheelFrictionCurve sideways = wheel.WheelCollider.sidewaysFriction;
            float speed = rigidBody.linearVelocity.magnitude;
            speedFactor = Mathf.Clamp01(speed / maxSpeed);

            float targetGrip = isDrifting
                ? Mathf.Lerp(normalGrip, highSpeedGrip, speedFactor)
                : normalGrip;

            sideways.stiffness = targetGrip;
            wheel.WheelCollider.sidewaysFriction = sideways;
            wheel.WheelCollider.sidewaysFriction = sideways;

            if (!hasFuel)
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = 0f;
                continue;
            }

            if (isHandbrake && !wheel.motorized)
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = brakeTorque;
                continue;
            }

            if (isAccelerating)
            {
                if (wheel.motorized)
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;

                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                wheel.WheelCollider.motorTorque = 0f;
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque * 0.5f;
            }
        }
    }
}