using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ObjectType
{
    PlayerMark,
    Wall,
    EnemyDefault,
}

public class Palate : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D DrawZone;
    private TileBase tileToDraw;
    public GameObject PlayerMark;
    public GameObject Wall;
    public GameObject EnemyDefault;
    private GameObject SelectedObject;
    private GameObject playerSpawnPoint;




    private void OnEnable()
    {
        SelectedObject = null;
        playerSpawnPoint = null;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && SelectedObject != null && MouseAvailable())
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPos);

            //tilemap.SetTile(cellPosition, tileToDraw);
            GameObject madeObject = Instantiate(SelectedObject, tilemap.CellToWorld(cellPosition) + tilemap.cellSize / 2f, Quaternion.identity);
            madeObject.transform.parent = transform;
            if (SelectedObject == PlayerMark)
            {
                if (playerSpawnPoint != null)
                {
                    Destroy(playerSpawnPoint);
                }
                playerSpawnPoint = madeObject;
            }
        }
    }
    private bool MouseAvailable()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return DrawZone.OverlapPoint(mousePosition);
    }

    public void SelectPlayer()
    {
        SelectedObject = PlayerMark;
    }

    public void SelectWall()
    {
        SelectedObject = Wall;
    }

    public void SelectDefaultEnemy()
    {
        SelectedObject = EnemyDefault;
    }
}
