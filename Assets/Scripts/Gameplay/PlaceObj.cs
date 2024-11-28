using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlaceObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{   
    // component
    private TurretSpawner turretSpawner;
    private TurretController turretController;
    private BuildingSystem buildingSystem;
    //
    private GameObject draggedObject;
    [SerializeField] private Transform border;
    [SerializeField] private LayerMask layerMask;
    private Transform turretPrefab;
    private int indexTurret;
    private int cost;
    private void Start()
    {
        indexTurret = transform.GetSiblingIndex();
        buildingSystem = BuildingSystem.instance;
        turretSpawner = TurretSpawner.instance;
        turretPrefab = turretSpawner.GetPrefabByIndex(indexTurret);
        cost = turretPrefab.GetComponent<TurretController>().cost;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (!CanBuyTurret())
        {
            Debug.Log("Dont enough money");
            return;
        }
        draggedObject = Instantiate(turretPrefab.gameObject, transform.position,Quaternion.identity);
        draggedObject.SetActive(true);
        draggedObject.transform.SetAsLastSibling();
        turretController = draggedObject.GetComponent<TurretController>();
        turretController.isShoot = false;
        buildingSystem.ShowGrid();
    }

    public void OnDrag(PointerEventData eventData)
    {   
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity,layerMask))
        {
            //GridManager grid = hit.collider.gameObject.GetComponent<GridManager>();
            int index = hit.transform.GetSiblingIndex();
            Vector3 a = buildingSystem.gridManager[index].GetPos(hit.point);
            draggedObject.transform.position = a;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity,layerMask))
        {
            int index = hit.transform.GetSiblingIndex();
            if (buildingSystem.gridManager[index].CanPlace(hit.point))
            {
                Vector3 a = buildingSystem.gridManager[index].GetPos(hit.point);
                turretSpawner.Spawn(turretSpawner.GetPrefabByIndex(indexTurret), a,Quaternion.identity).gameObject.SetActive(true);
            }
            Destroy(draggedObject);
            turretController = null;
        }
        foreach (GridManager grid in buildingSystem.gridManager)
        {
            grid.HideAndShowGrid(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        border.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        border.gameObject.SetActive(false);
    }
    public bool CanBuyTurret()
    {
        Debug.Log("Buy");
        if (GameController.instance.GetMoney() < cost)
            return false;
        GameController.instance.MinusAmountCost(cost);

        return true;

    }
}