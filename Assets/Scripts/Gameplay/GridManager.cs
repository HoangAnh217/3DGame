using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private float gridHeight, gridWidth;
    private float height, width;
    [SerializeField] private float cellSizeX, cellSizeZ;
    private bool[,] arrBool;
    //Vector2 originPos;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Transform holderLine;
    // private this grid
    private Vector2 originPos;
    private Vector2Int point;

    private void Start()
    {
        width = 10 * transform.localScale.x;
        height = 10 * transform.localScale.z;
        int rows = (int)(width / cellSizeX);
        int columns = (int)(height / cellSizeZ);
        gridWidth = cellSizeX*rows;
        gridHeight = cellSizeZ*columns;
        arrBool = new bool[rows, columns];
        originPos = new Vector2(transform.position.x - width / 2 + (width - gridWidth) / 2, transform.position.z - height / 2 + (height - gridHeight) / 2);
        CreateGridLines();
        HideAndShowGrid(false);
    }
    public void SetArr(Vector2Int pointSet,bool a)
    {   
        arrBool[pointSet.x, pointSet.y] = a;
    }
    public void HideAndShowGrid(bool a)
    {
        holderLine.gameObject.SetActive(a);
    }
    public Vector3 GetPos(Vector3 mousePosition)
    {
        GetMouseToArr(mousePosition);
        // Debug.Log(row+"  "+ column);
        return new Vector3(point.x * cellSizeX - cellSizeX / 2 + originPos.x, 0, point.y* cellSizeZ - cellSizeZ / 2 + originPos.y);
    }

    public Vector2Int GetMouseToArr(Vector3 mousePosition)
    {
        float x = mousePosition.x - originPos.x;
        float z = mousePosition.z - originPos.y;
        point.x = Mathf.FloorToInt(x / cellSizeX) + 1;
        point.y = Mathf.FloorToInt(z / cellSizeZ + 1);
        point.x = Mathf.Clamp(point.x, 1, Mathf.FloorToInt(gridWidth / cellSizeX)); // Giới hạn từ 1 đến số hàng tối đa
        point.y = Mathf.Clamp(point.y, 1, Mathf.FloorToInt(gridHeight / cellSizeZ));
        return point;
    }

    public bool CanPlace(Vector3 mousePosition)
    {
        GetMouseToArr(mousePosition);
        if (arrBool[point.x -1,point.y-1])
        {
            return false;
        } else
        {
            arrBool[point.x -1,point.y-1] = true;
            return true;
        }
    }
    private void CreateGridLines()
    {
        for (float x = 0; x <= gridWidth; x += cellSizeX)
        {
            CreateLine(
                originPos + new Vector2(x, 0),
                originPos + new Vector2(x, gridHeight)
            );
        }

        for (float z = 0; z <= gridHeight; z += cellSizeZ)
        {
            CreateLine(
                originPos + new Vector2(0, z),
                originPos + new Vector2(gridWidth, z)
            );
        }
    }

    private void CreateLine(Vector2 start, Vector2 end)
    {
        Vector3 st = new Vector3(start.x, 0.2f, start.y);
        Vector3 ed = new Vector3(end.x, 0.2f, end.y);
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(holderLine);
        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, st);
        lineRenderer.SetPosition(1, ed  );
    }
}
