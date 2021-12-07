using UnityEngine;
[RequireComponent(typeof(PlaneMeshGeneratorComponent))]
public class PlaneMeshDeformComponent : MonoBehaviour
{
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool animate;
    private Vector3[] baseHeightMesh;
    private PlaneMeshGeneratorComponent gen;
    private Mesh mesh;

    private void Start()
    {
        gen = GetComponent<PlaneMeshGeneratorComponent>();
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        gen.CreateMesh();
        if(animate)
            CreateWave();
    }
    
    private void CreateWave()
    {
        baseHeightMesh = mesh.vertices;
        Vector3[] waveVertices = new Vector3[baseHeightMesh.Length];
        Vector3 vertice = new Vector3();
        float verticeWave;
        int verticeCols = gen.cols + 1;
        for (int x = 0; x <= gen.cols; x++)
        {
            verticeWave = Mathf.Sin(Time.time * speed + ( x * Mathf.PI * 2/gen.cols)) * amplitude; 
            for (int y = 0;  y <= gen.rows ; y++)
            {
                vertice = baseHeightMesh[verticeCols * y + x];
                vertice.y += verticeWave; 
                waveVertices[verticeCols * y + x] = vertice;
            }
        }
        mesh.vertices = waveVertices;
        mesh.RecalculateNormals();
    }
}
