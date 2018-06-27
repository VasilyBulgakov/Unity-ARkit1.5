using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;

public class DelaunayGen : MonoBehaviour {
	 public int NumberOfVertices = 1000;

    public float size = 10;

    public int seed = 0;

    private DelaunayTriangulation2 delaunay;

    private Material lineMaterial;

	private Mesh mesh;
	// Use this for initialization

	Vector3[] points;
	int[] indecies;

	void Start () {
		lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));

		List<Vertex2> vertices = new List<Vertex2>();

		Random.InitState(seed);
		for (int i = 0; i < NumberOfVertices; i++)
		{
			float x = size * Random.Range(-1.0f, 1.0f);
			float y = size * Random.Range(-1.0f, 1.0f);

			vertices.Add(new Vertex2(x, y));
		}

		delaunay = new DelaunayTriangulation2();
		delaunay.Generate(vertices);

		generateMesh();
	}
	private void OnPostRender()
	{
		if (delaunay == null || delaunay.Cells.Count == 0 || delaunay.Vertices.Count == 0) return;

	}

	private void generateMesh()
	{
		mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;

		points = new Vector3[delaunay.Cells.Count*3];
        indecies = new int[delaunay.Cells.Count*3];

		int index = 0;  
		Debug.Log("CELLS COUNT:" + delaunay.Cells.Count);
		foreach (DelaunayCell<Vertex2> cell in delaunay.Cells)
		{
			DrawSimplex(cell.Simplex, index);	
			index+=3;			
		}  
        mesh.vertices = points;		
		mesh.triangles = indecies;
	}


	private void DrawSimplex(Simplex<Vertex2> f, int index)
	{
		points[index] = new Vector3(f.Vertices[0].X, 0.0f, f.Vertices[0].Y);
		points[index+1] = new Vector3(f.Vertices[1].X, 0.0f, f.Vertices[1].Y);
		points[index+2] = new Vector3(f.Vertices[2].X, 0.0f, f.Vertices[2].Y);

		indecies[index] = index;
		indecies[index+1] = index+1;
		indecies[index+2] = index+2;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
