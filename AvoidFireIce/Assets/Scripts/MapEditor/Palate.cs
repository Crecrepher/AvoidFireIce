using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public enum ObjectType
{
    BulletTower,
    RayTower,
    Bullet,
    Ray,
    Wall,
    Glass,
    PlayerMark,
    Star,
}

public class Palate : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D DrawZone;
    public ToggleGroup ObjectPalateGroup;

    public List<GameObject> PalateObjects;

    private GameObject SelectedObject;

    public LayerMask usedLayer;
    private GameObject currentObject;


    private Vector2 pressPosGap;


    private void OnEnable()
    {
        SelectedObject = null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectedObject != null && MouseAvailable())
        {
            if (!ToggleChecker())
            {
                Vector3 mouseDownPos = Input.mousePosition;
                Vector2 pos = Camera.main.ScreenToWorldPoint(mouseDownPos);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 100f, usedLayer);

                if (hit.collider != null)
                {
                    GameObject selectedObject = hit.collider.gameObject;
                    HandleSelection(selectedObject);
                }
            }
            else
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

                if (SelectedObject == PalateObjects[(int)ObjectType.PlayerMark])
                {
                    var playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerStart");
                    if (playerSpawnPoint != null)
                    {
                        Destroy(playerSpawnPoint);
                    }
                }
                GameObject madeObject = Instantiate(SelectedObject, tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2f, Quaternion.identity);
                HandleSelection(madeObject);
            }

        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mouseDownPos = Input.mousePosition;
        //    Vector2 pos = Camera.main.ScreenToWorldPoint(mouseDownPos);
        //    RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 100f, usedLayer);

        //    if (hit.collider != null)
        //    {
        //        GameObject selectedObject = hit.collider.gameObject;
        //        HandleSelection(selectedObject);
        //    }
        //}

        //switch (currentFunction)
        //{
        //    case EditModFunc.Move:
        //        Vector3 mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //        if (currentObject != null && Input.GetMouseButton(0))
        //        {
        //            if (currentObject != null && Input.GetMouseButtonDown(0))
        //            {
        //                pressPosGap = currentObject.transform.position - mouseDownPos;
        //            }
        //            currentObject.transform.position = (Vector2)mouseDownPos - pressPosGap;
        //        }
        //        break;
        //    case EditModFunc.Rotate:
        //        break;

        //}
    }

    private bool ToggleChecker()
    {
        Toggle[] toggles = ObjectPalateGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                return true;
            }
        }
        return false;
    }

    private bool MouseAvailable()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return DrawZone.OverlapPoint(mousePosition);
    }

    public void SelectPalateObjects(int palateCode)
    {
        SelectedObject = PalateObjects[palateCode];
    }

    private void HandleSelection(GameObject selectedObject)
    {
        if (currentObject != null)
        {
            currentObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        currentObject = selectedObject;
        selectedObject.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    //public void ChangeFunc(int funcCode)
    //{
    //    currentFunction = (EditModFunc)funcCode;
    //}

    public void Zoom(float scale)
    {
        Camera.main.orthographicSize += scale;
    }

    public void MoveVerticalCurrentObj(float distance)
    {
        currentObject.transform.position = currentObject.transform.position + new Vector3(0, distance, 0);
    }

    public void MoveHorizontalCurrentObj(float distance)
    {
        currentObject.transform.position = currentObject.transform.position + new Vector3(distance, 0, 0);
    }

    public void RotateCurrentObj(float rotation)
    {
        currentObject.transform.Rotate(0f, 0f, rotation);
    }

    public void FlipXCurrentObj()
    {
        GameObject obj = new GameObject("Parent");
        obj.transform.position = currentObject.transform.position;
        currentObject.transform.SetParent(obj.transform);
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * -1, 1, 1);
        currentObject.transform.SetParent(null);
        DestroyImmediate(obj);
    }
    public void FlipYCurrentObj()
    {
        GameObject obj = new GameObject("Parent");
        obj.transform.position = currentObject.transform.position;
        currentObject.transform.SetParent(obj.transform);
        obj.transform.localScale = new Vector3(1, obj.transform.localScale.y * -1, 1);
        currentObject.transform.SetParent(null);
        DestroyImmediate(obj);
    }

    public void DeleteCurrentObj()
    {
        Destroy(currentObject);
        currentObject = null;
    }

    public void DuplicateCurrentObj()
    {
        GameObject madeObject = Instantiate(currentObject, currentObject.transform.position + new Vector3(0.5f, 0.5f, 0), currentObject.transform.rotation);
        HandleSelection(madeObject);
    }
}
