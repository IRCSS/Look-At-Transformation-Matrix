using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour {


    public  Transform     LookAt;
    public  Transform     modelFoward;
    public  MeshRenderer  renderOfTheMesh;

    // When true, the model is rendered normaly with its own MVP matrix, this is so that you can correctly set the pivot
    public  bool          DebugNormalRenderView;

    private Material      meshMaterial;

    private void Start()
    {
        renderOfTheMesh.sharedMaterial.SetFloat("_debugOrignalPosition", 0.0f); // incase the original material has changed
                                                                                // this resets the rendering so that in editor the material is rendered normal
                                                                                // I did have execute in edit mode at the begining but it complicates resource 
                                                                                // management


        meshMaterial = new Material( renderOfTheMesh.sharedMaterial);
        renderOfTheMesh.sharedMaterial = meshMaterial;  
    }

    void OnEnable()
    {
        Camera.onPreRender += UpdateMVP;
    }

    void OnDisable()
    {
        Camera.onPreRender -= UpdateMVP;
        Destroy(meshMaterial);
    }

    private void OnDestroy()
    {
        Destroy(meshMaterial);
    }

    // Update is called once per frame
    void UpdateMVP(Camera cam)
    {

       

        Vector3 lookAtVector = modelFoward.worldToLocalMatrix*(LookAt.transform.position - modelFoward.transform.position).normalized ;
        Vector3 coordRight   = Vector3.Cross(Vector3.up, lookAtVector).normalized;
        Vector3 coordUp      = Vector3.Cross(lookAtVector, coordRight).normalized;
        Vector3 scale        = modelFoward.transform.localScale;

        
        Matrix4x4 scaleMatrix = new Matrix4x4( new Vector4(scale.x,       0.0f,     0.0f,       0.0f),
                                               new Vector4(   0.0f,    scale.y,     0.0f,       0.0f),
                                               new Vector4(   0.0f,       0.0f,  scale.z,       0.0f), 
                                               new Vector4(   0.0f,       0.0f,     0.0f,       1.0f)) ;


        Matrix4x4 lookAtMatrix = new Matrix4x4( new Vector4(        coordRight.x,         coordRight.y,           coordRight.z,       0.0f),
                                               new Vector4(           coordUp.x,            coordUp.y,              coordUp.z,       0.0f),
                                               new Vector4(      lookAtVector.x,       lookAtVector.y,         lookAtVector.z,       0.0f), 
                                               new Vector4(                0.0f,                  0.0f,                  0.0f,       1.0f)) * scaleMatrix;
        
        // MVP matrix aka UnityObjectToClip matrix. 
        Matrix4x4 objectToClip = GL.GetGPUProjectionMatrix(cam.projectionMatrix, true)
            * cam.worldToCameraMatrix * modelFoward.localToWorldMatrix
            * lookAtMatrix * modelFoward.worldToLocalMatrix
            * renderOfTheMesh.localToWorldMatrix;


        meshMaterial.SetFloat("_debugOrignalPosition", DebugNormalRenderView ? 0.0f : 1.0f);

        meshMaterial.SetMatrix("_oToC", objectToClip);
	}
}
