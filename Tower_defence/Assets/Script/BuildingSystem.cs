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
    public PlayerController player;

    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private TileBase whiteTile;
    [SerializeField] RectTransform ConfigTab;

    public List<GameObject> Prefabs = new List<GameObject>();

    private PlaceableObject objectToPlace;

    private bool isSelecting = false;

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
    public void EndSetting()
    {
        if (CanBePlaced(objectToPlace))
        {
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
        isSelecting = false;
    }
    public void ExitSetting()
    {
        Destroy(objectToPlace.gameObject);
        ConfigTab.gameObject.SetActive(false);
        ConfigTab.GetComponent<ConfigScript>().Target = null;
        isSelecting = false;
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
    public void InitializeWithObject(int index)
    {
        if (!isSelecting)
        {
            isSelecting = true;
            var playerPos = new Vector3(player.gameObject.transform.position.x, 0, player.gameObject.transform.position.z);
            Vector3 position = SnapCoordinateToGrid(playerPos + new Vector3(0,0,3f));

            GameObject obj = Instantiate(Prefabs[index], position, Quaternion.identity);
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