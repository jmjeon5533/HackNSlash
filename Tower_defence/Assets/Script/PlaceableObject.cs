using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] Vertices;
    public Material GridMaterial;
    public List<MeshRenderer> ChildMaterial = new List<MeshRenderer>();
    public Material MyMaterial;

    private Animator anim;
    public float AttackCoolTime;
    private float CurAttackTime;
    public float Radius;
    public GameObject BulletPrefab;
    [SerializeField] int Damage;
    [SerializeField] Transform TurretArm;
    [SerializeField] Transform[] FirePoint;

    private void GetColliderVertexPositionsLocal()
    {
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        Vertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        Vertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        Vertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }
    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[Vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(Vertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }
        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x), Mathf.Abs((vertices[0] - vertices[3]).y), 1);
    }
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }
    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
    private void Update()
    {
        if (Placed)
        {

            Collider[] hit = Physics.OverlapSphere(transform.position, Radius, LayerMask.GetMask("Enemy"));
            if (hit.Length > 0)
            {
                CurAttackTime += Time.deltaTime;
                var pos = (hit[0].transform.position - transform.position).normalized;
                var y = (Mathf.Atan2(pos.x, pos.z) * Mathf.Rad2Deg) / 2;
                if (CurAttackTime >= AttackCoolTime)
                {
                    CurAttackTime -= AttackCoolTime;
                    
                    foreach(var p in FirePoint)
                    {
                        var bullet = Instantiate(BulletPrefab, p.position, Quaternion.Euler(0, y, 0));
                        bullet.GetComponent<BulletBase>().Damage = Damage;
                    }
                }
                TurretArm.transform.rotation = Quaternion.Euler(0,y * 2,0);
            }
        }
    }
}
