using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Components
    private TurretSpawner turretSpawner;
    private TurretController turretController;

    // Cached references
    private Transform turretPrefab;
    private GameObject draggedObject;
    [SerializeField] private Transform border;
    private Transform cube;
    private Material cubeMaterial;

    // Configuration
    [SerializeField] private LayerMask layerMask;
    private int indexTurret;
    private int cost;
    private bool isDragging = false;

    private Vector3 placementPosition;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        border = transform.GetChild(0);
        cube = BuildingSystem.instance.transform.Find("Cube");
        cubeMaterial = cube.GetComponent<Renderer>().material;
        border.gameObject.SetActive(false);
        cube.gameObject.SetActive(false);

        turretSpawner = TurretSpawner.instance;
        indexTurret = transform.GetSiblingIndex();
        turretPrefab = turretSpawner.GetPrefabByIndex(indexTurret);
        cost = turretPrefab.GetComponent<TurretController>().turretDataSO.cost;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanAffordTurret()) return;

        StartDragging();
    }

    private void StartDragging()
    {
        isDragging = true;

        draggedObject = Instantiate(turretPrefab.gameObject, transform.position, Quaternion.identity);
        draggedObject.SetActive(true);
        draggedObject.transform.SetAsLastSibling();

        turretController = draggedObject.GetComponent<TurretController>();
        turretController.enabled = false;

        cube.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (TryGetHitPosition(eventData, out RaycastHit hit))
        {
            UpdateCubeAndTurretPosition(hit);
            UpdateCubeColor(hit.collider.gameObject.layer);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (TryGetHitPosition(eventData, out RaycastHit hit) && hit.collider.gameObject.layer == 9)
        {
            PlaceTurret();
            PlayerData.instance.ReceiveMoney(-cost);
            hit.collider.gameObject.layer = 10;
        }

        EndDragging();
    }
    private bool TryGetHitPosition(PointerEventData eventData, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }

    private void UpdateCubeAndTurretPosition(RaycastHit hit)
    {
        placementPosition = hit.transform.position + new Vector3(0, hit.point.y, 0);
        cube.position = placementPosition + new Vector3(0, 0.75f, 0);
        draggedObject.transform.position = placementPosition;
    }

    private void UpdateCubeColor(int layer)
    {
        Color color = layer == 9 ? Color.green : Color.red;
        color.a = 0.5f;
        cubeMaterial.color = color;
    }


    private void PlaceTurret()
    {
        turretSpawner.Spawn(turretPrefab, placementPosition, Quaternion.identity).gameObject.SetActive(true);
        
    }

    private void EndDragging()
    {
        isDragging = false;
        cube.gameObject.SetActive(false);
        Destroy(draggedObject);
        turretController = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        border.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        border.gameObject.SetActive(false);
    }

    private bool CanAffordTurret()
    {
        if (PlayerData.instance.GetMoney() < cost)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        return true;
    }
}
