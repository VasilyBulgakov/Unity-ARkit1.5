using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HullDelaunayVoronoi.Hull;
using HullDelaunayVoronoi.Primitives;

	using UnityEngine.XR.iOS;
public class HullGen : MonoBehaviour {

	public int NumberOfVertices = 1000;

    public float size = 5;

    public int seed = 0;

	private Mesh mesh;

	private float theta;

    private ConvexHull3 hull;	
	Vector3[] points;
	int[] indecies;

    private Vector3[] m_PointCloudData;

public MeshFilter filt;
	MeshGenerator mg;
private bool frameUpdated = false;

public int maxPointsToShow = 1000;
	public void ARFrameUpdated(UnityARCamera camera)
    {
        m_PointCloudData = camera.pointCloudData;
        frameUpdated = true;
    }

	// Use this for initialization
	void Start () {		
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
       
        frameUpdated = false;

		// Vertex3[] vertices = new Vertex3[NumberOfVertices];

		// Random.InitState(seed);
		// for (int i = 0; i < NumberOfVertices; i++)
		// {
		// 	float x = size * Random.Range(-1.0f, 1.0f);
		// 	float y = size * Random.Range(-0.1f, 0.1f);
		// 	float z = size * Random.Range(-1.0f, 1.0f);

		// 	vertices[i] = new Vertex3(x, y, z);
		// }

		hull = new ConvexHull3();
		// hull.Generate(vertices);	
		// generateMesh();
	}
	private void FixedUpdate()
	{
		if (frameUpdated) {
            if (m_PointCloudData != null && m_PointCloudData.Length > 0 && maxPointsToShow > 0) {
                int numParticles = Mathf.Min (m_PointCloudData.Length, maxPointsToShow);

				Vertex3[] vertices = new Vertex3[numParticles];
				for (int i = 0; i < numParticles; i++)
				{
					var p = m_PointCloudData[i];
					vertices[i] = new Vertex3(p.x, p.y, p.z);
				}
				hull.Generate(vertices);
                generateMesh();
				// mg.meshFilter = filt;	
				// mg.generateHullMesh(vertices);
            }
            frameUpdated = false;
        }
	}

	private void generateMesh()
	{
		mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;

		points = new Vector3[hull.Simplexs.Count*3];
        indecies = new int[hull.Simplexs.Count*3];

		int index = 0; 
		foreach (Simplex<Vertex3> f in hull.Simplexs)
		{
			DrawSimplex(f, index);	
			index+=3;			
		}  
        mesh.vertices = points;		
		mesh.triangles = indecies;
	}
	private void DrawSimplex(Simplex<Vertex3> f, int index)
	{
		points[index] = new Vector3(f.Vertices[0].X, f.Vertices[0].Y, f.Vertices[0].Z);
		points[index+1] = new Vector3(f.Vertices[1].X, f.Vertices[1].Y, f.Vertices[1].Z);
		points[index+2] = new Vector3(f.Vertices[2].X, f.Vertices[2].Y, f.Vertices[2].Z);

		indecies[index] = index;
		indecies[index+1] = index+1;
		indecies[index+2] = index+2;
	}
	
}
