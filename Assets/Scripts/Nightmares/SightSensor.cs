using General;
using UnityEngine;

[ExecuteInEditMode]
public class SightSensor : MonoBehaviour
{
    [field: SerializeField] public float Distance { get; set; }
    [field: SerializeField] public float Angle { get; set; }
    [field: SerializeField] public float Height { get; set; }
    [field: SerializeField] public Color Colour { get; set; }
    [field: SerializeField] public LayerMask OcclusionLayers { get; set; }

    public float TimeWithoutSeeingPlayer { get; set; }

    private Mesh _mesh;
    private Collider[] _colliders;
    private int _count;

    private void Awake()
    {
        _colliders = new Collider[50];
    }

    private void Update()
    {
        TimeWithoutSeeingPlayer += Time.deltaTime;
    }

    public bool IsInSight(GameObject obj)
    {
        if (obj == GameManager.Instance.Player.gameObject) TimeWithoutSeeingPlayer = 0f;
        
        var origin = transform.position;
        var destination = obj.transform.position;
        var direction = destination - origin;

        if (direction.y < 0 || direction.y > Height) return false;

        direction.y = 0f;
        var deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > Angle) return false;

        origin.y += Height / 2f;
        destination.y = origin.y;

        return !Physics.Linecast(origin, destination, OcclusionLayers);
    }

    private Mesh CreateMesh()
    {
        var mesh = new Mesh();

        const int segments = 10;
        const int numTriangles = (segments * 4) + 4;
        const int numVertices = numTriangles * 3;

        var vertices = new Vector3[numVertices];
        var triangles = new int[numVertices];

        var bottomCentre = Vector3.zero;
        var bottomLeft = Quaternion.Euler(0, -Angle, 0) * Vector3.forward * Distance;
        var bottomRight = Quaternion.Euler(0, Angle, 0) * Vector3.forward * Distance;

        var topCentre = bottomCentre + Vector3.up * Height;
        var topLeft = bottomLeft + Vector3.up * Height;
        var topRight = bottomRight + Vector3.up * Height;

        var v = 0;

        // Left.
        
        vertices[v++] = bottomCentre;
        vertices[v++] = bottomLeft;
        vertices[v++] = topLeft;
        
        vertices[v++] = topLeft;
        vertices[v++] = topCentre;
        vertices[v++] = bottomCentre;
        
        // Right.
        
        vertices[v++] = bottomCentre;
        vertices[v++] = topCentre;
        vertices[v++] = topRight;
        
        vertices[v++] = topRight;
        vertices[v++] = bottomRight;
        vertices[v++] = bottomCentre;

        var currentAngle = -Angle;
        var deltaAngle = Angle * 2 / segments;

        for (var i = 0; i < segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * Distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * Distance;

            topLeft = bottomLeft + Vector3.up * Height;
            topRight = bottomRight + Vector3.up * Height;
            
            // Far.
        
            vertices[v++] = bottomLeft;
            vertices[v++] = bottomRight;
            vertices[v++] = topRight;
        
            vertices[v++] = topRight;
            vertices[v++] = topLeft;
            vertices[v++] = bottomLeft;
        
            // Top.
        
            vertices[v++] = topCentre;
            vertices[v++] = topLeft;
            vertices[v++] = topRight;
        
            // Bottom
        
            vertices[v++] = bottomCentre;
            vertices[v++] = bottomLeft;
            vertices[v++]   = bottomRight;
            
            currentAngle += deltaAngle;
        }

        for (var i = 0; i < numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        _mesh = CreateMesh();
    }

    private void OnDrawGizmos()
    {
        if (!_mesh) return;
        
        Gizmos.color = Colour;
        
        var t = transform;
        Gizmos.DrawMesh(_mesh, t.position, t.rotation);
        
        Gizmos.DrawWireSphere(transform.position, Distance);
        for (var i = 0; i < _count; ++i) Gizmos.DrawSphere(_colliders[i].transform.position, .2f);
    }
}
