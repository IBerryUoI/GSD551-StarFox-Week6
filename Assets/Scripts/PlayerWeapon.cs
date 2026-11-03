using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("References")] 
    
    [Tooltip("The projectile prefab")]
    [SerializeField]
    private Projectile projectilePrefab;

    [Tooltip("the controller that converts the targeting rectle into a world-space aiming direction")]
    [SerializeField]
    private TargetingReticleController targetingReticleController;

    [SerializeField]
    private Transform leftWeaponPoint;
    [SerializeField]
    private Transform rightWeaponPoint;

    [Header("Firing")]
    
    [Tooltip("The number of projectile volleys the player can fire each second.")]
    [Min(0.1f)]
    [SerializeField]
    private float shotPerSecond = 4f;

    private float nextFireTime = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFireInputHeld())
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Fire();

        nextFireTime = Time.time + (1f / shotPerSecond);
    }

    private bool IsFireInputHeld()
    {
        bool mouseFireHeld = Mouse.current != null && Mouse.current.leftButton.isPressed;
        bool gamepadFireHeld = Gamepad.current != null && Gamepad.current.rightTrigger.isPressed;

        return mouseFireHeld || gamepadFireHeld;
    }
    
    
    private void Fire()
    {
        CreateProjectile(leftWeaponPoint);
        CreateProjectile(rightWeaponPoint);
    }

    private void CreateProjectile(Transform weaponPoint)
    {
        
        Projectile newProjectile = Instantiate(projectilePrefab, weaponPoint.position, weaponPoint.rotation);
        Vector3 aimDirection = targetingReticleController.GetAimDirection(weaponPoint.position);
        
        newProjectile.Initialize(aimDirection);
    }
    
    
}
