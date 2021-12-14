using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlaneMeshGeneratorComponent : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] public int rows;
    [SerializeField] public int cols;
    
    private bool started;
    private Mesh mesh;
    
    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        CreateMesh();
    }

    private void OnValidate()
    {
        if (!started)
            return;
        CreateMesh();
    }

    private void CreatePoints()
    {
        int compteur = 0; int nbPoints = (cols + 1) * (rows + 1);
        Vector3[] verticesArray = new Vector3[nbPoints];
        float z = transform.position.z; float x = transform.position.x;
        float quadWidth = width / cols;
        float quadHeight = height / rows;
        for (int i = 0; i <= rows ; ++i)
        {
            for (int j = 0; j <= cols; ++j)
            {
                if (compteur == 0 || j == 0)
                    verticesArray[compteur] = new Vector3(x,0,z);
                else
                    verticesArray[compteur] = new Vector3(verticesArray[compteur - 1].x + quadWidth, 0,z );
                compteur++;
            }
            z = z - quadHeight;
            x = transform.position.x;
        }
        mesh.vertices = verticesArray;
    }

    private void CreateTriangles()
    {
        int nbPoints = (cols * rows) * 6;
        int[] trianglesArray = new int[nbPoints];
        for (int cpt = 0, i = 0, y = 0; y < rows; y++, i++) {
            for (int x = 0; x < cols; x++, cpt += 6, i++) {
                trianglesArray[cpt + 5] = i;
                trianglesArray[cpt + 4] = i + cols + 1;
                trianglesArray[cpt + 3] = i + 1;
                trianglesArray[cpt + 2] = trianglesArray[cpt + 3];
                trianglesArray[cpt + 1] = trianglesArray[cpt + 4] ;
                trianglesArray[cpt] = i + cols + 2;
            }
        }
        mesh.triangles = trianglesArray;
    }

    private void ApplyUVs()
    {
        Vector2[] uvArray = new Vector2[(cols + 1) * (rows + 1)];
        for (int i = 0, y = 0; y <= rows; y++) {
            for (int x = 0; x <= cols; x++, i++) {
                uvArray[i] =  new Vector2( (float)x / cols, (float)y / rows);
            }
        }
        mesh.uv = uvArray;
    }

    public void CreateMesh()
    {
        CreatePoints();
        CreateTriangles();
        ApplyUVs();
        mesh.RecalculateNormals();
    }
}