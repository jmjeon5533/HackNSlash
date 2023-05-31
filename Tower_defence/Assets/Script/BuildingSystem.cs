using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;

    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    [SerializeField] private PlayerController player;
    [SerializeField] RectTransform ConfigTab;

    public GameObject prefab1, prefab2;

    private PlaceableObject objectToPlace;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    private void Start()
    {
        ConfigTab.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            InitializeWithObject(prefab2);
        }
        if (!objectToPlace)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndSetting();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitSetting();
        }
    }
    public void EndSetting()
    {
        if (CanBePlaced(objectToPlace))
        {
            objectToPlace.GetComponent<BoxCollider>().isTrigger = false;
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            foreach (var m in objectToPlace.ChildMaterial)
            {
                m.material = objectToPlace.MyMaterial;
            }
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
        ConfigTab.gameObject.SetActive(false);
        ConfigTab.GetComponent<ConfigScript>().Target = null;
    }
    public void ExitSetting()
    {
        Destroy(objectToPlace.gameObject);
        ConfigTab.gameObject.SetActive(false);
        ConfigTab.GetComponent<ConfigScript>().Target = null;
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit Hit, int.MaxValue, LayerMask.GetMask("Ground")))
        {
            return Hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private static TileBase[] GetTileBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    #endregion

    #region Building Placement
    public void InitializeWithObject(GameObject prefab)
    {
        var playerPos = new Vector3(player.gameObject.transform.position.x, 0, player.gameObject.transform.position.z);
        Vector3 position = SnapCoordinateToGrid(playerPos);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
        obj.GetComponent<BoxCollider>().isTrigger = true;
        foreach (var m in obj.GetComponent<PlaceableObject>().ChildMaterial)
        {
            m.GetComponent<MeshRenderer>().material = obj.GetComponent<PlaceableObject>().GridMaterial;
        }
        ConfigTab.GetComponent<ConfigScript>().Target = obj.transform;
        ConfigTab.gameObject.SetActive(true);
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;

        TileBase[] baseArray = GetTileBlock(area, MainTilemap);

        foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }
    public void TakeArea(Vector3Int start, Vector3Int size)
    {
        MainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }

    #endregion
}