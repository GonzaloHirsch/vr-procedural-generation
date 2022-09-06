using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : Framework.MonoBehaviorSingleton<MeshGenerator>
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;
    private Vector2 center;

    [Header("Size")]
    public int xSize = 20;
    public int zSize = 20;
    private int offsetLimits = 999999;
    [Header("Noise")]
    public float zoomFactor = 0.3f;
    public float noiseScale = 2f;
    public float maxZoomFactor = 0.1f;
    public float maxNoiseScale = 8f;
    private float currentZoomFactor;
    private float currentNoiseFactor;
    private float maxDistance;
    [Header("Visual")]
    public Material material;
    public Vector3 initialColor, finalColor;

    public void GenerateTerrain()
    {
        // Create and set mesh
        this.mesh = new Mesh();
        this.GetComponent<MeshFilter>().mesh = this.mesh;
        // Edit material tiling
        this.UpdateMeshColor(0f);
        this.material.mainTextureScale = new Vector2(this.xSize, this.zSize);
        this.GetComponent<MeshRenderer>().material = this.material;
        // Move the mesh to the center and compute the center point
        this.transform.position = new Vector3(-this.xSize / 2, 0f, -this.zSize / 2);
        this.center = new Vector2(this.xSize / 2, this.zSize / 2);
        this.maxDistance = Vector2.Distance(this.center, new Vector2(this.xSize, this.zSize));
        // Creating the mesh points
        this.CreateMesh();
        this.UpdateMesh();
        // Add the mesh collider
        this.gameObject.AddComponent<MeshCollider>();
        MeshCollider mc = this.gameObject.GetComponent<MeshCollider>();
        mc.sharedMesh = this.mesh;
    }

    void CreateMesh()
    {
        // Allocate space for the mesh
        this.vertices = new Vector3[(this.xSize + 1) * (this.zSize + 1)];
        this.uv = new Vector2[this.vertices.Length];
        // Create all the vertices
        float vertexY, distance, factor;
        // Sample points for the noise, to generate randomness
        int xSample = Random.Range(-this.offsetLimits, this.offsetLimits);
        int zSample = Random.Range(-this.offsetLimits, this.offsetLimits);
        for (int i = 0, z = 0; z <= this.zSize; z++)
        {
            for (int x = 0; x <= this.xSize; x++)
            {
                distance = Vector2.Distance(this.center, new Vector2(x,z));
                factor = Mathf.InverseLerp(0.5f, this.maxDistance, distance);
                // Compute noise variables
                this.currentZoomFactor = this.zoomFactor + ((this.maxZoomFactor - this.zoomFactor) * factor);
                this.currentNoiseFactor = this.noiseScale + ((this.maxNoiseScale - this.noiseScale) * factor);
                // Get Y based on the noise
                vertexY = Mathf.PerlinNoise(xSample + x * this.currentZoomFactor, zSample + z * this.currentZoomFactor) * this.currentNoiseFactor;
                // Create the vertex
                this.vertices[i] = new Vector3(x, vertexY, z);
                // Setting the UV points
                this.uv[i] = new Vector2((float) x / this.xSize, (float) z / this.zSize);
                i++;
            }
        }
        // Create all the triangle tuples
        this.triangles = new int[6 * this.xSize * this.zSize];
        for (int vert = 0, tris = 0, z = 0; z < this.zSize; z++)
        {
            for (int x = 0; x < this.xSize; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + this.xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + this.xSize + 1;
                triangles[tris + 5] = vert + this.xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        this.mesh.Clear();
        this.mesh.vertices = this.vertices;
        this.mesh.triangles = this.triangles;
        this.mesh.uv = this.uv;
        this.mesh.RecalculateNormals();
        this.mesh.UploadMeshData(false);
    }

    public void UpdateMeshColor(float completionPercentage)
    {
        // Simple linear interpolation
        Vector3 color = ((this.finalColor - this.initialColor) * Mathf.Clamp(completionPercentage, 0f, 1f) + this.initialColor)/255f;
        // Color customColor = n
        material.SetColor("_Color", new Color(color.x, color.y, color.z));
    }

    /* 
    // Debug gizmos for mesh points
    void OnDrawGizmos()
    {
        if (this.vertices != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 v in this.vertices)
            {
                Gizmos.DrawSphere(v, .1f);
            }
        }
    } */
}
