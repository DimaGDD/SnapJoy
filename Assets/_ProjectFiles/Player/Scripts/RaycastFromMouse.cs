using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastFromMouse : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _itemLayer;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public bool CastRayFromCursor(States playerStates)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(screenPosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxDistance))
        {
            DefaultItem item = hit.collider.GetComponent<DefaultItem>();

            if (item == playerStates.CurrentItem)
            {
                return true;
            }
        }

        return false;
    }
}
