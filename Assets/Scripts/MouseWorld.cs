using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private LayerMask unitLayerMask;
    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position = GetPosition();
    }

    public Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, mousePlaneLayerMask);
        return raycastHit.point;
    }
    public Unit GetUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask);

        if(raycastHit.collider != null){
            return raycastHit.collider.GetComponent<Unit>();
        }
        return null;
    }
}
