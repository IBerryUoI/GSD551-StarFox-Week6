using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The Box Collider that defines the world-space area the Player is allowed to move within.")]
    [SerializeField]
    private BoxCollider movementBounds;

    [Tooltip("The child Transform rotated for pitch, yaw, banking, and barrel-roll visuals without rotating the Player root.")]
    [SerializeField]
    private Transform playerVisual;

    [Space]

    [Header("Movement")]

    [Tooltip("The Player's horizontal and vertical movement speed in world units per second.")]
    [SerializeField]
    private float movementSpeed = 5.0f;

    [Space]

    [Header("Visual Rotation")]

    [Tooltip("The maximum number of degrees the ship can pitch up or down in response to vertical movement input.")]
    [SerializeField]
    private float maximumPitchAngle = 20.0f;

    [Tooltip("The maximum number of degrees the ship can turn left or right in response to horizontal movement input.")]
    [SerializeField]
    private float maximumYawAngle = 25.0f;

    [Tooltip("The maximum number of degrees the ship can bank while a bank input is held.")]
    [SerializeField]
    private float maximumBankAngle = 90.0f;

    [Tooltip("Controls how quickly the PlayerVisual approaches its target pitch, yaw, and bank rotation.")]
    [SerializeField]
    private float visualRotationSpeed = 8.0f;

    [Space]

    [Header("Barrel Roll")]

    [Tooltip("The maximum time in seconds allowed between two presses in the same bank direction to trigger a barrel roll.")]
    [SerializeField]
    private float doubleTapWindow = 0.3f;

    [Tooltip("The time in seconds required for the ship to complete one full barrel-roll rotation.")]
    [SerializeField]
    private float barrelRollDuration = 0.5f;

    [Tooltip("The initial horizontal speed added to the Player when a barrel roll begins.")]
    [SerializeField]
    private float barrelRollMomentum = 10.0f;

    [Tooltip("Controls how quickly the added barrel-roll momentum returns to zero.")]
    [SerializeField]
    private float barrelRollMomentumRecovery = 20.0f;
    
    

    private float lastLeftBankPressTime = float.NegativeInfinity;
    private float lastRightBankPressTime = float.NegativeInfinity;

    private bool isBarrelRolling = false;
    private float barrelRollDirection;
    private float barrelRollElapsed;
    private float barrelRollAngle;
    private float currentBarrelRollMomentum;
    
    private Vector2 movementInput;
    private float horizontalInput;
    private float verticalInput;
    private float bankInput;

    private Quaternion currentFlightRotation;

    private void Start()
    {
        currentFlightRotation = playerVisual.localRotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        ReadMovementInput();
        ReadBankInput();
        DetectBarrelRollInput();
        
        UpdateBarrelRoll();
        Move();
        RotatePlayerVisual();
    }

    private void UpdateBarrelRoll()
    {
        if (isBarrelRolling == false)
        {
            return;
        }

        barrelRollElapsed += Time.deltaTime;

        float rollProgress = Mathf.Clamp01(barrelRollElapsed / barrelRollDuration);

        barrelRollAngle = -barrelRollDirection * 360f * rollProgress;

        if (rollProgress >= 1f)
        {
            isBarrelRolling = false;
            barrelRollElapsed = 0f;
            barrelRollAngle = 0f;
        }
    }

    private void DetectBarrelRollInput()
    {
        bool leftBankPressed = Keyboard.current.qKey.wasPressedThisFrame;
        bool rightBankPressed = Keyboard.current.eKey.wasPressedThisFrame;

        if (Gamepad.current != null)
        {
            leftBankPressed = leftBankPressed || Gamepad.current.leftShoulder.wasPressedThisFrame;
            rightBankPressed = rightBankPressed || Gamepad.current.rightShoulder.wasPressedThisFrame;
        }

        if (isBarrelRolling == true)
        {
            return;
        }

        if (leftBankPressed == true)
        {
            float time = Time.time - lastLeftBankPressTime;
            if (time <= doubleTapWindow)
            {
                StartBarrelRoll(-1f);
                lastLeftBankPressTime = float.NegativeInfinity;
            }
            else
            {
                lastLeftBankPressTime = Time.time;
            }
        }
        else if (rightBankPressed == true)
        {
            float time = Time.time - lastRightBankPressTime;
            if (time <= doubleTapWindow)
            {
                StartBarrelRoll(1f);
                lastRightBankPressTime = float.NegativeInfinity;
            }
            else
            {
                lastRightBankPressTime = Time.time;
            }
        }
    }

    private void StartBarrelRoll(float rollDirection)
    {
        isBarrelRolling = true;
        barrelRollDirection = rollDirection;

        barrelRollElapsed = 0f;
        barrelRollAngle = 0;

        currentBarrelRollMomentum = rollDirection * barrelRollMomentum;
    }

    private void ReadBankInput()
    {
        bankInput = Keyboard.current.eKey.ReadValue() - Keyboard.current.qKey.ReadValue();

        if (Gamepad.current != null)
        {
            bankInput += Gamepad.current.rightShoulder.ReadValue() - Gamepad.current.leftShoulder.ReadValue();
        }

        bankInput = Mathf.Clamp(bankInput, -1f, 1f);
    }

    private void RotatePlayerVisual()
    {
        float targetPitch = -movementInput.y * maximumPitchAngle;
        float targetYaw = movementInput.x * maximumYawAngle;
        float targetBank = -bankInput * maximumBankAngle;

        Quaternion targetFlightRotation = Quaternion.Euler(targetPitch, targetYaw, targetBank);
        currentFlightRotation = Quaternion.Lerp(currentFlightRotation, targetFlightRotation, visualRotationSpeed * Time.deltaTime);

        Quaternion barrelRollRotation = Quaternion.Euler(0, 0, barrelRollAngle);
        playerVisual.localRotation = currentFlightRotation * barrelRollRotation;

    }

    private void ReadMovementInput()
    {
        horizontalInput = Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue();
        verticalInput = Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue();
        
        if (Gamepad.current != null)
        {
            Vector2 leftStickInput = Gamepad.current.leftStick.ReadValue();
            horizontalInput += leftStickInput.x;
            verticalInput += leftStickInput.y;
        }
        
        movementInput = new Vector2(horizontalInput, verticalInput);
        movementInput = Vector2.ClampMagnitude(movementInput, 1f);
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(movementInput.x, movementInput.y, 0f);
        Vector3 movementAmount = movementDirection * movementSpeed * Time.deltaTime;

        movementAmount.x += currentBarrelRollMomentum * Time.deltaTime;
        currentBarrelRollMomentum =
            Mathf.MoveTowards(currentBarrelRollMomentum, 0f, barrelRollMomentumRecovery * Time.deltaTime);

        Vector3 targetPosition = transform.position + movementAmount;
        Bounds bounds = movementBounds.bounds;

        targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);

        transform.position = targetPosition;
    }
    
}
