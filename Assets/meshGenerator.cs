using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HullDelaunayVoronoi.Hull;
using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Voronoi;
using HullDelaunayVoronoi.Primitives;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
	private Mesh mesh;
    public MeshFilter meshFilter;
    private ConvexHull3 hull;	
    private DelaunayTriangulation2 delaunay;

	Vector3[] points;
	int[] indecies;
 
    // Use this for initialization
    void Start () {
       
    }
 
    void CreateMesh() {
        
    }

    public static Vertex3[] Vec3ToVertex3(Vector3[] v)
    {
        int size = v.Length;
        Vertex3[] vertices = new Vertex3[size];
        for (int i = 0; i < size; i++)
        {
            var p = v[i];
            vertices[i] = new Vertex3(p.x, p.y, p.z);
        }
        return vertices;
    }

    public void generateDelaunayMesh(Vertex3[] vertices)
	{
        Vertex2[] vertices2 = new Vertex2[vertices.Length];
        for(int i =0; i < vertices.Length; i++)
        {
            vertices2[i] = new Vertex2(vertices[i].X, vertices[i].Z);
        }
        delaunay = new DelaunayTriangulation2();
		delaunay.Generate(vertices2);
         if (delaunay == null || delaunay.Cells.Count == 0 || delaunay.Vertices.Count == 0) return;

		mesh = new Mesh(); 
           

		points = new Vector3[delaunay.Cells.Count*3];
        indecies = new int[delaunay.Cells.Count*3];

		int index = 0;  
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




    public void generateHullMesh(Vertex3[] vertices)
	{
        hull = new ConvexHull3();
		hull.Generate(vertices);
         if (hull == null || hull.Simplexs.Count == 0 || hull.Vertices.Count == 0) return;

		mesh = new Mesh(); 
       meshFilter.mesh = mesh;

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
