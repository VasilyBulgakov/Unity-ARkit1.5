using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.iOS;

public class DrawLines : MonoBehaviour {

	private Vector3[] pointCloud;
	private bool frameUpdated = false;
	public int maxPointsToShow = 10000;

	Mesh mesh;

	// Use this for initialization
	public void ARFrameUpdated(UnityARCamera camera)
    {
        pointCloud = camera.pointCloudData;
        frameUpdated = true;
    }

	// Use this for initialization
	void Start () {		
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
       
        frameUpdated = false;

		mesh = new Mesh();
 
        GetComponent<MeshFilter>().mesh = mesh;
       
	}
	
	private void FixedUpdate() {
		 if (frameUpdated) {
            if (pointCloud != null && pointCloud.Length > 0 && maxPointsToShow > 0) {
                int numParticles = Mathf.Min (pointCloud.Length, maxPointsToShow);
				CreateMesh(  pointCloud );
            }
            frameUpdated = false;
        }
	}

	void CreateMesh( Vector3[] points) {        
        int[] indecies = new int[points.Length];

        for(int i=0;i<points.Length;++i) {            
            indecies[i] = i;           
        } 
        mesh.vertices = points;
        mesh.SetIndices(indecies, MeshTopology.Lines, 0);
 
    }
}
