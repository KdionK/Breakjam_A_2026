using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private LayerMask hitMask = ~0;

    [Header("Gizmo")]
    [SerializeField] private float hitSphereRadius = 0.15f;

    private Ray lastRay;
    private RaycastHit lastHit;
    private bool hasHit;

    void Awake()
    {
        InputListener.onRightClickEvent += OnRightClick;
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        lastRay = cam.ScreenPointToRay(mousePos);
        hasHit = Physics.Raycast(lastRay, out lastHit, maxDistance, hitMask);

        if (hasHit)
        {
            
        }
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            lastRay = cam.ScreenPointToRay(mousePos);
            hasHit = Physics.Raycast(lastRay, out lastHit, maxDistance, hitMask);

            if (hasHit)
            {
                GameObject go = lastHit.collider.gameObject;
                IBuyable buyable = go.GetComponent<IBuyable>();
                if (buyable != null && buyable.IsBuyable())
                {
                    buyable.BuyThis(null);
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || cam == null) return;

        if (hasHit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lastRay.origin, lastHit.point);
            
            Gizmos.DrawSphere(lastHit.point, hitSphereRadius);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(lastHit.point, lastHit.point + lastHit.normal);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(lastRay.origin, lastRay.direction * maxDistance);
        }
    }

    
}