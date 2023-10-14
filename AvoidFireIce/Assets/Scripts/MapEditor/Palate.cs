using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject MainLoopInfo;
    public List<TMP_InputField> MainLoopInfoInputs;
    public Toggle MainLoopInfoBind;
    public List<GameObject> FireLoopUi;

    private InfoMoveLoopWindow infoMoveLoopWindow;
    private InfoRotateLoopWindow infoRotateLoopWindow;
    private InfoFireLoopWindow infoFireLoopWindow;

    private GameObject SelectedObject;

    public LayerMask usedLayer;
    private GameObject currentObject;

    private EditMode editMode;
    private LoopType currentLoopEdit;

    //MultipleSelect
    public Toggle MultiSelectToggle;
    private bool isMultiSelect = false;
    public List<GameObject> SelectedObjects;

    private void Awake()
    {
        MainLoopInfoInputs[0].onEndEdit.AddListener(MoveLoopLengthChanged);
        MainLoopInfoInputs[1].onEndEdit.AddListener(RotateLoopLengthChanged);
        MainLoopInfoInputs[2].onEndEdit.AddListener(FireLoopLengthChanged);
        SelectedObjects = new List<GameObject>();
        isMultiSelect = false;
    }
    private void OnEnable()
    {
        SelectedObject = null;
        editMode = EditMode.Place;
        infoMoveLoopWindow = GetComponent<InfoMoveLoopWindow>();
        infoRotateLoopWindow = GetComponent<InfoRotateLoopWindow>();
        infoFireLoopWindow = GetComponent<InfoFireLoopWindow>();
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
        if (!isMultiSelect)
        {
            ReleaseSelection();
        }
        else
        {
            SelectedObjects.Add(currentObject);
        }
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
            OpenMainLoopInfo(false);
            infoMoveLoopWindow.CloseWindow();
        }

        foreach (var multi in SelectedObjects)
        {
            if (multi != null)
            {
                if (Defines.instance.isHaveElement(multi.GetComponent<MarkerInfo>().ObjectType))
                {
                    multi.GetComponent<DangerObject>().SetColor();
                }
                else
                {
                    multi.GetComponent<SpriteRenderer>().color = Color.white;
                }
                multi.layer = 6;
            }
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
        GameObject madeObject = Instantiate(currentObject, currentObject.transform.position + new Vector3(0.5f, 0f, 0f), currentObject.transform.rotation);
        MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();
        if (ml != null)
        {
            madeObject.GetComponent<MoveLoopData>().ml = new MoveLoop();
            madeObject.GetComponent<MoveLoopData>().ml.loopTime = ml.ml.loopTime;
            madeObject.GetComponent<MoveLoopData>().ml.loopList = ml.ml.loopList.ToList();
        }
        RotateLoopData rl = currentObject.GetComponent<RotateLoopData>();
        if (rl != null)
        {
            madeObject.GetComponent<RotateLoopData>().rl = new RotateLoop();
            madeObject.GetComponent<RotateLoopData>().rl.loopTime = rl.rl.loopTime;
            madeObject.GetComponent<RotateLoopData>().rl.loopList = rl.rl.loopList.ToList();
        }
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
            ChekNeedFireLoop();
        }
        InfoButton.SetActive(false);
    }

    public void SelectLoopType(int type)
    {
        infoMoveLoopWindow.Release();
        infoRotateLoopWindow.Release();
        for (int i = 0; i < 3; i++)
        {
            if (i == type)
            {
                var bar = LoopTableBars.transform.GetChild(i);
                bar.GetComponent<LayoutElement>().preferredHeight = 50;
                var rect = LoopLines[i].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 13f);
                var buttons = LoopLines[i].GetComponentsInChildren<RectTransform>();
                if (buttons != null && buttons.Length > 0)
                {
                    foreach (var button in buttons)
                    {
                        if (rect == button) continue;
                        button.sizeDelta = new Vector2(button.sizeDelta.x, 30f);
                    }
                }
            }
            else
            {
                var bar = LoopTableBars.transform.GetChild(i);
                bar.GetComponent<LayoutElement>().preferredHeight = 25;
                var rect = LoopLines[i].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 3f);
                var buttons = LoopLines[i].GetComponentsInChildren<RectTransform>();
                if (buttons != null && buttons.Length > 0)
                {
                    foreach (var button in buttons)
                    {
                        if (rect == button) continue;
                        button.sizeDelta = new Vector2(button.sizeDelta.x, 10f);
                    }
                }
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
        LoopBlocksList lbl = LoopBlocksListMaker();
        switch (currentLoopEdit)
        {
            case LoopType.Move:
                {
                    MoveLoopData ml = MoveLoopMaker();
                    var button = Instantiate(LoopBlocks[0], LoopLines[0].transform);
                    MoveLoopBlock block = new MoveLoopBlock();
                    List<MoveLoopBlock> blocks = ml.ml.loopList;
                    if (blocks.Count == 0)
                    {
                        block.startTime = 0;
                    }
                    else
                    {
                        block.startTime = blocks[blocks.Count - 1].startTime + blocks[blocks.Count - 1].playTime;
                    }
                    block.moveVector = new Vector2(1f, 0f);
                    block.playTime = 1f;
                    if (ml.ml.loopTime < block.startTime + block.playTime)
                    {
                        block.playTime = ml.ml.loopTime - block.startTime;
                        if (block.playTime <= 0)
                        {
                            Destroy(button);
                            Debug.Log("TooMuchButtons!");
                            return;
                        }
                    }
                    ml.ml.loopList.Add(block);

                    RectTransform rect = button.GetComponent<RectTransform>();
                    rect.position = new Vector2(LoopLines[0].transform.position.x + block.startTime * 50, LoopLines[0].transform.position.y);
                    rect.sizeDelta = new Vector2(block.playTime * 50, rect.rect.height);
                    lbl.moveLoopBlocks.Add(button);

                    button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow.OpenWindow(button, currentObject, ml.ml.loopList.IndexOf(block)));
                }
                break;
            case LoopType.Rotate:
                {
                    RotateLoopData rl = RotateLoopMaker();
                    var button = Instantiate(LoopBlocks[1], LoopLines[1].transform);
                    RotateLoopBlock block = new RotateLoopBlock();
                    List<RotateLoopBlock> blocks = rl.rl.loopList;
                    if (blocks.Count == 0)
                    {
                        block.startTime = 0;
                    }
                    else
                    {
                        block.startTime = blocks[blocks.Count - 1].startTime + blocks[blocks.Count - 1].playTime;
                    }
                    block.rot = 90;
                    block.playTime = 1f;
                    if (rl.rl.loopTime < block.startTime + block.playTime)
                    {
                        block.playTime = rl.rl.loopTime - block.startTime;
                        if (block.playTime <= 0)
                        {
                            Destroy(button);
                            Debug.Log("TooMuchButtons!");
                            return;
                        }
                    }
                    rl.rl.loopList.Add(block);

                    RectTransform rect = button.GetComponent<RectTransform>();
                    rect.position = new Vector2(LoopLines[1].transform.position.x + block.startTime * 50, LoopLines[1].transform.position.y);
                    rect.sizeDelta = new Vector2(block.playTime * 50, rect.rect.height);
                    lbl.rotateLoopBlocks.Add(button);

                    button.GetComponent<Button>().onClick.AddListener(() => infoRotateLoopWindow.OpenWindow(button, currentObject, rl.rl.loopList.IndexOf(block)));
                }
                break;
            case LoopType.Fire:
                {
                    FireLoopData fl = FireLoopMaker();
                    var button = Instantiate(LoopBlocks[2], LoopLines[2].transform);
                    FireLoopBlock block = new FireLoopBlock();
                    List<FireLoopBlock> blocks = fl.fl.loopList;
                    if (blocks.Count == 0)
                    {
                        block.startTime = 0;
                    }
                    else
                    {
                        block.startTime = blocks[blocks.Count - 1].startTime + blocks[blocks.Count - 1].playTime;
                    }
                    block.rate = 1;
                    block.speed = 10;
                    block.playTime = 1f;
                    if (fl.fl.loopTime < block.startTime + block.playTime)
                    {
                        block.playTime = fl.fl.loopTime - block.startTime;
                        if (block.playTime <= 0)
                        {
                            Destroy(button);
                            Debug.Log("TooMuchButtons!");
                            return;
                        }
                    }
                    fl.fl.loopList.Add(block);

                    RectTransform rect = button.GetComponent<RectTransform>();
                    rect.position = new Vector2(LoopLines[2].transform.position.x + block.startTime * 50, LoopLines[2].transform.position.y);
                    rect.sizeDelta = new Vector2(block.playTime * 50, rect.rect.height);
                    lbl.fireLoopBlocks.Add(button);

                    button.GetComponent<Button>().onClick.AddListener(() => infoFireLoopWindow.OpenWindow(button, currentObject, fl.fl.loopList.IndexOf(block)));
                }
                break;
        }
    }

    private void SelectOnLoopMod()
    {
        SetLoopMod.SetActive(true);
        SelectLoopType(0);
        UpdateLoopTable();
        OpenMainLoopInfo(false);
        infoMoveLoopWindow.index = -1;
        infoRotateLoopWindow.index = -1;
        infoFireLoopWindow.index = -1;
    }
    public void UpdateLoopTable()
    {
        var olds1 = GameObject.FindGameObjectsWithTag("MoveLoopButton");
        var olds2 = GameObject.FindGameObjectsWithTag("RotateLoopButton");
        var olds3 = GameObject.FindGameObjectsWithTag("FireLoopButton");
        foreach (var c in olds1)
        {
            Destroy(c.gameObject);
        }
        foreach (var c in olds2)
        {
            Destroy(c.gameObject);
        }
        foreach (var c in olds3)
        {
            Destroy(c.gameObject);
        }

        if (currentObject == null)
        {
            return;
        }

        ChekNeedFireLoop();

        LoopBlocksList lb = currentObject.GetComponent<LoopBlocksList>();
        MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();
        RotateLoopData rl = currentObject.GetComponent<RotateLoopData>();
        FireLoopData fl = currentObject.GetComponent<FireLoopData>();

        if (lb == null)
        {
            LoopLines[0].GetComponent<RectTransform>().sizeDelta = new Vector2(500, LoopLines[0].GetComponent<RectTransform>().sizeDelta.y);
            return;
        }

        lb.moveLoopBlocks = new List<GameObject>();
        lb.rotateLoopBlocks = new List<GameObject>();

        if (ml != null)
        {
            LoopLines[0].GetComponent<RectTransform>().sizeDelta = new Vector2(ml.ml.loopTime * 50f, LoopLines[0].GetComponent<RectTransform>().sizeDelta.y);
            int count = 0;
            foreach (var c in ml.ml.loopList)
            {
                var button = Instantiate(LoopBlocks[0], LoopLines[0].transform);
                button.GetComponent<Button>().onClick.AddListener(() => infoMoveLoopWindow.OpenWindow(button, currentObject, ml.ml.loopList.IndexOf(c)));

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(LoopLines[0].transform.position.x + c.startTime * 50f, LoopLines[0].transform.position.y);
                rect.sizeDelta = new Vector2(c.playTime * 50, rect.rect.height);
                lb.moveLoopBlocks.Add(button);
                count++;
            }

        }
        else
        {
            LoopLines[0].GetComponent<RectTransform>().sizeDelta = new Vector2(500, LoopLines[0].GetComponent<RectTransform>().sizeDelta.y);
        }

        if (rl != null)
        {
            LoopLines[0].GetComponent<RectTransform>().sizeDelta = new Vector2(rl.rl.loopTime * 50f, LoopLines[1].GetComponent<RectTransform>().sizeDelta.y);
            int count = 0;
            foreach (var c in rl.rl.loopList)
            {
                var button = Instantiate(LoopBlocks[1], LoopLines[1].transform);
                button.GetComponent<Button>().onClick.AddListener(() => infoRotateLoopWindow.OpenWindow(button, currentObject, rl.rl.loopList.IndexOf(c)));

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(LoopLines[1].transform.position.x + c.startTime * 50f, LoopLines[1].transform.position.y);
                rect.sizeDelta = new Vector2(c.playTime * 50, rect.rect.height);
                lb.rotateLoopBlocks.Add(button);
                count++;
            }

        }
        else
        {
            LoopLines[1].GetComponent<RectTransform>().sizeDelta = new Vector2(500, LoopLines[0].GetComponent<RectTransform>().sizeDelta.y);
        }

        if (fl != null)
        {
            LoopLines[2].GetComponent<RectTransform>().sizeDelta = new Vector2(fl.fl.loopTime * 50f, LoopLines[2].GetComponent<RectTransform>().sizeDelta.y);
            int count = 0;
            foreach (var c in fl.fl.loopList)
            {
                var button = Instantiate(LoopBlocks[2], LoopLines[2].transform);
                button.GetComponent<Button>().onClick.AddListener(() => infoFireLoopWindow.OpenWindow(button, currentObject, fl.fl.loopList.IndexOf(c)));

                RectTransform rect = button.GetComponent<RectTransform>();
                rect.position = new Vector2(LoopLines[2].transform.position.x + c.startTime * 50f, LoopLines[2].transform.position.y);
                rect.sizeDelta = new Vector2(c.playTime * 50, rect.rect.height);
                lb.fireLoopBlocks.Add(button);
                count++;
            }

        }
        else
        {
            LoopLines[2].GetComponent<RectTransform>().sizeDelta = new Vector2(500, LoopLines[2].GetComponent<RectTransform>().sizeDelta.y);
        }

        DrawLoopTableLine();
    }

    public void OpenMainLoopInfo(bool on)
    {
        MainLoopInfo.SetActive(on);
        if (currentObject == null) { return; }
        if (on)
        {
            LoopBlocksList lbl = LoopBlocksListMaker();
            MainLoopInfoBind.isOn = lbl.isTmeBind;

            MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();
            if (ml != null)
            {
                MainLoopInfoInputs[0].text = ml.ml.loopTime.ToString();
                RectTransform rt = LoopLines[0].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(ml.ml.loopTime * 50f, rt.sizeDelta.y);
            }
            else
            {
                MainLoopInfoInputs[0].text = 10f.ToString();
                RectTransform rt = LoopLines[0].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(500f, rt.sizeDelta.y);
            }

            RotateLoopData rl = currentObject.GetComponent<RotateLoopData>();
            if (rl != null)
            {
                MainLoopInfoInputs[1].text = rl.rl.loopTime.ToString();
                RectTransform rt = LoopLines[1].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rl.rl.loopTime * 50f, rt.sizeDelta.y);
            }
            else
            {
                MainLoopInfoInputs[1].text = 10f.ToString();
                RectTransform rt = LoopLines[1].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(500f, rt.sizeDelta.y);
            }

            if (isTowerChecker(currentObject))
            {
                MainLoopInfoInputs[2].gameObject.SetActive(true);
                FireLoopData fl = currentObject.GetComponent<FireLoopData>();
                if (fl != null)
                {
                    MainLoopInfoInputs[2].text = fl.fl.loopTime.ToString();
                    RectTransform rt = LoopLines[2].GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(fl.fl.loopTime * 50f, rt.sizeDelta.y);
                }
                else
                {
                    MainLoopInfoInputs[2].text = 10f.ToString();
                    RectTransform rt = LoopLines[2].GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(500f, rt.sizeDelta.y);
                }
            }
            else
            {
                MainLoopInfoInputs[2].gameObject.SetActive(false);
            }

        }
    }

    public void TimeBindSet()
    {
        LoopBlocksList lbl = LoopBlocksListMaker();
        MainLoopInfoBind.isOn = lbl.isTmeBind;
    }

    public void MoveLoopLengthChanged(string newValue)
    {
        LoopBlocksList lbl = LoopBlocksListMaker();
        MoveLoopData ml = MoveLoopMaker();

        float value;
        if (float.TryParse(newValue, out value))
        {
            float minLength = ml.ml.loopList.Count > 0 ? ml.ml.loopList[ml.ml.loopList.Count - 1].startTime + ml.ml.loopList[ml.ml.loopList.Count - 1].playTime : 0f;
            if (value < minLength) { value = minLength; }
            ml.ml.loopTime = value;
            RectTransform rt = LoopLines[0].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(ml.ml.loopTime * 50f, rt.sizeDelta.y);
            MainLoopInfoInputs[0].text = value.ToString();
        }
        DrawLoopTableLine();
    }

    public void RotateLoopLengthChanged(string newValue)
    {
        LoopBlocksList lbl = LoopBlocksListMaker();
        RotateLoopData rl = RotateLoopMaker();

        float value;
        if (float.TryParse(newValue, out value))
        {
            float minLength = rl.rl.loopList.Count > 0 ? rl.rl.loopList[rl.rl.loopList.Count - 1].startTime + rl.rl.loopList[rl.rl.loopList.Count - 1].playTime : 0f;
            if (value < minLength) { value = minLength; }
            rl.rl.loopTime = value;
            RectTransform rt = LoopLines[1].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rl.rl.loopTime * 50f, rt.sizeDelta.y);
            MainLoopInfoInputs[1].text = value.ToString();
        }
        DrawLoopTableLine();
    }

    public void FireLoopLengthChanged(string newValue)
    {
        LoopBlocksList lbl = LoopBlocksListMaker();
        FireLoopData fl = FireLoopMaker();

        float value;
        if (float.TryParse(newValue, out value))
        {
            float minLength = fl.fl.loopList.Count > 0 ? fl.fl.loopList[fl.fl.loopList.Count - 1].startTime + fl.fl.loopList[fl.fl.loopList.Count - 1].playTime : 0f;
            if (value < minLength) { value = minLength; }
            fl.fl.loopTime = value;
            RectTransform rt = LoopLines[2].GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(fl.fl.loopTime * 50f, rt.sizeDelta.y);
            MainLoopInfoInputs[2].text = value.ToString();
        }
        DrawLoopTableLine();
    }

    private MoveLoopData MoveLoopMaker()
    {
        MoveLoopData ml = currentObject.GetComponent<MoveLoopData>();
        if (ml == null)
        {
            ml = currentObject.AddComponent<MoveLoopData>();
            ml.ml = new MoveLoop();
            ml.ml.loopList = new List<MoveLoopBlock>();
            ml.ml.loopTime = 10f;
        }

        return ml;
    }

    private LoopBlocksList LoopBlocksListMaker()
    {
        LoopBlocksList lbl = currentObject.GetComponent<LoopBlocksList>();

        if (lbl == null)
        {
            lbl = currentObject.AddComponent<LoopBlocksList>();
            lbl.moveLoopBlocks = new List<GameObject>();
            lbl.rotateLoopBlocks = new List<GameObject>();
            lbl.fireLoopBlocks = new List<GameObject>();
            lbl.isTmeBind = false;
        }

        return lbl;
    }

    private RotateLoopData RotateLoopMaker()
    {
        RotateLoopData rl = currentObject.GetComponent<RotateLoopData>();
        if (rl == null)
        {
            rl = currentObject.AddComponent<RotateLoopData>();
            rl.rl = new RotateLoop();
            rl.rl.loopList = new List<RotateLoopBlock>();
            rl.rl.loopTime = 10f;
        }

        return rl;
    }

    private FireLoopData FireLoopMaker()
    {
        FireLoopData fl = currentObject.GetComponent<FireLoopData>();
        if (fl == null)
        {
            fl = currentObject.AddComponent<FireLoopData>();
            fl.fl = new FireLoop();
            fl.fl.loopList = new List<FireLoopBlock>();
            fl.fl.loopTime = 10f;
        }

        return fl;
    }
    public void DeleteBlock()
    {
        if (currentObject == null) return;
        int index = -1;
        switch (currentLoopEdit)
        {
            case LoopType.Move:
                index = infoMoveLoopWindow.index;
                break;
            case LoopType.Rotate:
                index = infoRotateLoopWindow.index;
                break;
            case LoopType.Fire:
                index = infoFireLoopWindow.index;
                break;
        }
        if (index < 0) return;
        var lbl = currentObject.GetComponent<LoopBlocksList>();
        if (lbl == null) return;
        switch (currentLoopEdit)
        {
            case LoopType.Move:
                {
                    var button = lbl.moveLoopBlocks[infoMoveLoopWindow.index];
                    currentObject.GetComponent<MoveLoopData>().ml.loopList.RemoveAt(index);
                    lbl.moveLoopBlocks.Remove(button);
                    Destroy(button);
                    infoMoveLoopWindow.CloseWindow();
                    infoMoveLoopWindow.index = -1;
                }
                break;
            case LoopType.Rotate:
                {
                    var button = lbl.rotateLoopBlocks[infoRotateLoopWindow.index];
                    currentObject.GetComponent<RotateLoopData>().rl.loopList.RemoveAt(index);
                    lbl.rotateLoopBlocks.Remove(button);
                    Destroy(button);
                    infoRotateLoopWindow.CloseWindow();
                    infoRotateLoopWindow.index = -1;
                }
                break;
            case LoopType.Fire:
                {
                    var button = lbl.fireLoopBlocks[infoFireLoopWindow.index];
                    currentObject.GetComponent<FireLoopData>().fl.loopList.RemoveAt(index);
                    lbl.fireLoopBlocks.Remove(button);
                    Destroy(button);
                    infoFireLoopWindow.CloseWindow();
                    infoFireLoopWindow.index = -1;
                }
                break;
        }
        OpenMainLoopInfo(false);
    }

    public bool isTowerChecker(GameObject obj)
    {
        if (obj == null) return false;
        MarkerInfo target = obj.GetComponent<MarkerInfo>();
        if (target == null)
        {
            return false;
        }
        return ((ObjectType)target.ObjectType == ObjectType.BulletTower)
            || ((ObjectType)target.ObjectType == ObjectType.RayTower);
    }

    public void ChekNeedFireLoop()
    {
        if (isTowerChecker(currentObject))
        {
            foreach (var item in FireLoopUi)
            {
                item.SetActive(true);
            }
        }
        else
        {
            foreach (var item in FireLoopUi)
            {
                item.SetActive(false);
            }
        }
    }


    //MultipleSelect
    public void SwitchMultifulSelect()
    {
        isMultiSelect = MultiSelectToggle.isOn;
    }
}
