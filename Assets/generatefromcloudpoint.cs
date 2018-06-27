using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.iOS;
public class generatefromcloudpoint : MonoBehaviour {

	private Vector3[] m_PointCloudData;
	private bool frameUpdated = false;

	public int maxPointsToShow = 10000;

	private MeshGenerator generator;
	public void ARFrameUpdated(UnityARCamera camera)
    {
        m_PointCloudData = camera.pointCloudData;
        frameUpdated = true;
    }

	// Use this for initialization
	void Start () {		
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
       generator = new MeshGenerator();
	   generator.meshFilter = GetComponent<MeshFilter>();
        frameUpdated = false;
	}
	
	// Update is called once per frame
	private void FixedUpdate() {
		if (frameUpdated) {
            if (m_PointCloudData != null && m_PointCloudData.Length > 0 && maxPointsToShow > 0) {
                int numParticles = Mathf.Min (m_PointCloudData.Length, maxPointsToShow);
				generator.generateHullMesh(MeshGenerator.Vec3ToVertex3(m_PointCloudData));
            }
            frameUpdated = false;
        }
	}
}
