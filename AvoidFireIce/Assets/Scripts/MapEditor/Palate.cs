using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



public class Palate : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D DrawZone;
    public ToggleGroup ObjectPalateGroup;
    public GameObject InfoButton;
    public InfoWindow infoWindow;

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
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
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
                else
                {
                    if (!IsPointerOverUIObject())
                    {
                        ReleaseSelection();
                    }
                }
            }
            else
            {
                if (SelectedObject != null && MouseAvailable())
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
                else
                {
                    if (!IsPointerOverUIObject())
                    {
                        ReleaseSelection();
                    }
                }
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

    bool IsPointerOverUIObject()
    {
        List<RaycastResult> results = new List<RaycastResult>();
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
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
        ReleaseSelection();
        currentObject = selectedObject;
        selectedObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        selectedObject.layer = 0;
        InfoButton.SetActive(true);
    }

    private void ReleaseSelection()
    {
        if (currentObject != null)
        {
            if (Defines.instance.isHaveElement(currentObject.GetComponent<MarkerInfo>().ObjectType))
            {
                currentObject.GetComponent<DangerObject>().SetColor();
            }
            else
            {
                currentObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            currentObject.layer = 6;
        }
        InfoButton.SetActive(false);
        infoWindow.CloseWindow();
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
        InfoButton.SetActive(false);
    }

    public void DuplicateCurrentObj()
    {
        GameObject madeObject = Instantiate(currentObject, currentObject.transform.position + new Vector3(0.5f, 0.5f, 0), currentObject.transform.rotation);
        HandleSelection(madeObject);
    }

    public void OpenCurrentObjectInfo() 
    {
        infoWindow.OpenWindow(currentObject);
    }
}
