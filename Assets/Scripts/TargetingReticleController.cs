using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingReticleController : MonoBehaviour
{
    [Header("References")]
    
    [Tooltip("The RectTransform that defines the screen-space area used to position the targeting reticule.")]
    [SerializeField]
    private RectTransform canvasRectTransform;

    [Tooltip("The camera used to project the targeting reticle")]
    [SerializeField]
    private Camera gameplayCamera;

    [Tooltip("The Player root used to preview the calculated aim direction")]
    [SerializeField]
    private Transform aimOrigin;

    [Tooltip("The RectTransform moved to represent the player's current aim position.")]
    [SerializeField]
    private RectTransform targetingReticle;

    [Header("Gamepad Input")] 
    [SerializeField]
    private float gamepadReticleSpeed = 800f;

    [Range(0f, 1f)]
    [SerializeField]
    private float gamepadDeadzone = 0.15f;

    [Header("World-Space Aiming")] 
    
    [Tooltip("How far along the camera ray the world aim point is placed.")]
    [SerializeField]
    private float aimDistance = 100f;

    private Vector3 worldAimPoint;

    // Update is called once per frame
    void Update()
    {
        if (!MoveReticleWithGamepad())      //equivlent to: if (MoveReticleWithGamepad() == false)
        {
            MoveReticleWithMouse();
        }

        UpdateWorldAimPoint();
    }

    public Vector3 GetAimDirection(Vector3 origin)
    {
        return (worldAimPoint - origin).normalized;
    }
    
    private void UpdateWorldAimPoint()
    {
        Vector2 reticleScreenPosition = RectTransformUtility.WorldToScreenPoint(null, targetingReticle.position);
        Ray cameraRay = gameplayCamera.ScreenPointToRay(reticleScreenPosition);
        worldAimPoint = cameraRay.GetPoint(aimDistance);
        
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * aimDistance, Color.yellow);

        if (aimOrigin != null)
        {
            Vector3 aimDirection = GetAimDirection(aimOrigin.position);
            Debug.DrawLine(aimOrigin.position, worldAimPoint, Color.cyan);
        }
        else
        {
            Debug.LogWarning("Skipped Debug.DrawLine because aimOrigin was null.");
            targetingReticle.gameObject.SetActive(false);
        }
    }

    private bool MoveReticleWithGamepad()
    {
        if (Gamepad.current == null)
        {
            return false;
        }

        Vector2 gamepadInput = Gamepad.current.rightStick.ReadValue();

        if (gamepadInput.magnitude < gamepadDeadzone)
        {
            return false;
        }

        Vector2 reticleMovement = gamepadInput * gamepadReticleSpeed * Time.deltaTime;

        Vector2 nextPosition = targetingReticle.anchoredPosition + reticleMovement;

        targetingReticle.anchoredPosition = ClampReticlePosition(nextPosition);

        return true;
    }

    private void MoveReticleWithMouse()
    {
        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.delta.ReadValue().sqrMagnitude == 0f)
        {
            return;
        }

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        bool founderCanvasPosition = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, mouseScreenPosition, null, out Vector2 mouseCanvasPosition);

        if (founderCanvasPosition == false)
        {
            return;
        }

        targetingReticle.anchoredPosition = ClampReticlePosition(mouseCanvasPosition);
    }

    private Vector2 ClampReticlePosition(Vector2 position)
    {
        Rect canvasRect = canvasRectTransform.rect;

        Vector2 reticleHalfSize = targetingReticle.rect.size * 0.5f;

        position.x = Mathf.Clamp(position.x, canvasRect.xMin + reticleHalfSize.x, canvasRect.xMax - reticleHalfSize.x);
        position.y = Mathf.Clamp(position.y, canvasRect.yMin + reticleHalfSize.y, canvasRect.yMax - reticleHalfSize.y);

        return position;
    }
}
