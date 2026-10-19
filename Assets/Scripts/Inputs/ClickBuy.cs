using UnityEngine;
using UnityEngine.InputSystem;

public class ClickBuy : MonoBehaviour
{
    [SerializeField] float rayLenght = 50f;
    [SerializeField] LayerMask layerMaskInteract;
    [SerializeField] Transform _camera;
    private GameObject raycastObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputListener.onRightClickEvent += OnRightClick;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return; // ignore started / canceled (release)
        RaycastHit hit;
         Debug.DrawRay( _camera.transform.position, _camera.transform.forward * rayLenght, Color.green);
        if( Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, rayLenght, layerMaskInteract))
        {
            Debug.Log(hit.transform.name);
            IBuyable buyable = hit.transform.gameObject.GetComponent<IBuyable>();
            if(buyable == null) return;
            buyable.BuyThis(this.gameObject);
        }


    }
}
