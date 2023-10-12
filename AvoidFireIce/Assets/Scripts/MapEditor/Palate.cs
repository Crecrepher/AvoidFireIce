using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject PlaceMod;
    public GameObject SetLoopMod;
    public GameObject LoopTableViewContent;
    public GameObject LoopTableGrid;
    public GameObject LoopTableBars;
    public List<GameObject> LoopLines;
    public List<GameObject> LoopBlocks;

    public List<GameObject> PalateObjects;
    public InfoMoveLoopWindow infoMoveLoopWindow;
    public GameObject MainLoopInfo;

    private GameObject SelectedObject;

    public LayerMask usedLayer;
    private GameObject currentObject;

    private EditMode editMode;
    private LoopType currentLoopEdit;

    private void OnEnable()
    {
        SelectedObject = null;
        editMode = EditMode.Place;
        infoMoveLoopWindow = GetComponent<InfoMoveLoopWindow>();
    }

    private void Update()
    {
        switch (editMode)
        {
            case EditMode.Place:
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
                }
                break;
            case EditMode.Loop:
                {
                    if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
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
                }
                break;
        }

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
        if (editMode == EditMode.Place)
        {
            InfoButton.SetActive(true);
        }
        if (editMode == EditMode.Loop)
        {
            SelectOnLoopMod();
        }
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
        if (editMode == EditMode.Loop)
        {
            SetLoopMod.SetActive(false);
        }
    }

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

    public void SetActivePlaceMod(bool on)
    {
        PlaceMod.SetActive(on);
        SetLoopMod.SetActive(!on);
        if (on) { editMode = EditMode.Place; }
        if (currentObject != null)
        {
            InfoButton.SetActive(true);
        }

    }


    //Loop

    public void SetActiveLoopMod(bool on)
    {
        PlaceMod.SetActive(!on);
        if (currentObject != null)
        {
            SetLoopMod.SetActive(on);
            infoWindow.CloseWindow();
        }
        if (on)
        {
            editMode = EditMode.Loop;
            currentLoopEdit = LoopType.Move;
            SelectLoopType(0);
            DrawLoopTableLine();
            UpdateLoopTable();
        }
        InfoButton.SetActive(false);
    }

    public void SelectLoopType(int type)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == type)
            {
                var bar = LoopTableBars.transform.GetChild(i);
                bar.GetComponent<LayoutElement>().preferredHeight = 50;
                var rect = LoopLines[i].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 13f);
            }
            else
            {
                var bar = LoopTableBars.transform.GetChild(i);
                bar.GetComponent<LayoutElement>().preferredHeight = 25;
                var rect = LoopLines[i].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 3f);
            }
        }
        currentLoopEdit = (LoopType)type;
    }

    private void DrawLoopTableLine()
    {
        var oldLines = GameObject.FindGameObjectsWithTag("LoopTableGrid");
        foreach (var line in oldLines)
        {
            Destroy(line.gameObject);
        }

        int count = (int)(LoopTableBars.GetComponent<RectTransform>().rect.width - 30) / 50;
        for (int i = 0; i < count + 1; i++)
        {
            var line = Instantiate(LoopTableGrid, new Vector3(30 + (i * 50), 0, 0), Quaternion.identity);
            line.transform.SetParent(LoopTableViewContent.transform, false);
        }
    }

    public void CreateMoveLoopB()
    {
        if (currentObject == null) { return; }
        MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();
        LoopBlocksList lbl = currentObject.GetComponent<LoopBlocksList>();
        if (ml == null)
        {
            currentObject.AddComponent<MoveLoopData>();
            ml = currentObject.GetComponent<MoveLoopData>();
            ml.ml = new MoveLoop();
            ml.ml.loopList = new List<MoveLoopBlock>();
            ml.ml.loopTime = 10f;
        }
        if (lbl == null)
        {
            currentObject.AddComponent<LoopBlocksList>();
            lbl = currentObject.GetComponent<LoopBlocksList>();
        }

        //맨끝 블럭 무한생성안되게 처리
        var button = Instantiate(LoopBlocks[0], LoopLines[0].transform);
        MoveLoopBlock block = new MoveLoopBlock();
        List<MoveLoopBlock> blocks = ml.ml.loopList;
        if (blocks.Count == 0)
        {
            block.startTime = 0;
            block.startPos = currentObject.transform.position;
        }
        else
        {
            block.startTime = blocks[blocks.Count - 1].startTime + blocks[blocks.Count - 1].playTime;
            block.startPos = blocks[blocks.Count - 1].endPos;
        }
        block.playTime = 1f;
        float maxTime = LoopLines[0].GetComponent<RectTransform>().sizeDelta.x / 50f;
        if (maxTime < block.playTime)
        {
            block.playTime = maxTime - block.startTime;
            if (block.playTime - block.startTime <= 0)
            {
                Destroy(button);
                Debug.Log("TooMuchButtons!");
                return;
            }
        }

        block.endPos = new Vector2(block.startPos.x + 1f, block.startPos.y);
        ml.ml.loopList.Add(block);


        RectTransform rect = button.GetComponent<RectTransform>();
        rect.position = new Vector2(LoopLines[0].transform.position.x + block.startTime * 50, LoopLines[0].transform.position.y);
        rect.sizeDelta = new Vector2(block.playTime * 50, rect.rect.height);
        lbl.moveLoopBlocks.Add(button);

        button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow.OpenWindow(button, block, block.startPos, block.startTime, maxTime));
    }

    private void SelectOnLoopMod()
    {
        SetLoopMod.SetActive(true);
        SelectLoopType(0);
        UpdateLoopTable();
    }
    private void UpdateLoopTable()
    {
        var olds = GameObject.FindGameObjectsWithTag("MoveLoopButton");
        foreach (var c in olds)
        {
            Destroy(c.gameObject);
        }

        if (currentObject == null)
        {
            return;
        }

        LoopBlocksList lb = currentObject.GetComponent<LoopBlocksList>();
        MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();

        if (lb == null)
            return;
        lb.moveLoopBlocks = new List<GameObject>();

        if (ml != null)
        {
            RectTransform rt = LoopLines[0].GetComponent<RectTransform>();
            rt.sizeDelta.Set(ml.ml.loopTime * 50f, rt.rect.y);
            int count = 0;
            foreach (var c in ml.ml.loopList)
            {
                var button = Instantiate(LoopBlocks[0], LoopLines[0].transform);
                //if (count == 0)
                //{
                //    if (ml.ml.loopList.Count > count + 1)
                //    {
                //        MoveLoopBlock nextMl = ml.ml.loopList[count + 1];
                //        button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow
                //                        .OpenWindow(button, c, c.startPos, nextMl.startTime, 0));
                //    }
                //    else
                //    {
                //        button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow
                //                        .OpenWindow(button, c, c.startPos, LoopLines[0].GetComponent<RectTransform>().sizeDelta.x / 50f, 0));
                //    }
                //}
                //else
                //{
                //    MoveLoopBlock pastMl = ml.ml.loopList[count - 1];
                //    if (ml.ml.loopList.Count > count + 1)
                //    {
                //        MoveLoopBlock nextMl = ml.ml.loopList[count + 1];
                //        button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow
                //                        .OpenWindow(button, c, pastMl.endPos, nextMl.startTime, pastMl.startTime + pastMl.playTime));
                //    }
                //    else
                //    {
                //        button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow
                //                        .OpenWindow(button, c, pastMl.endPos, LoopLines[0].GetComponent<RectTransform>().sizeDelta.x / 50f, pastMl.startTime + pastMl.playTime));
                //    }

                //}

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(LoopLines[0].transform.position.x + c.startTime * 50, LoopLines[0].transform.position.y);
                rect.sizeDelta = new Vector2(c.playTime * 50, rect.rect.height);
                lb.moveLoopBlocks.Add(button);
                count++;
            }
        }
    }
}
