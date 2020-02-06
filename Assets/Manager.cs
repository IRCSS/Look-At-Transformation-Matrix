using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Manager : MonoBehaviour {


    public Transform LookAt;
    public Transform mdelTransform;
    public Transform modelFoward;
    void OnEnable()
    {
        Camera.onPreRender += UpdateMVP;
    }

    void OnDisable()
    {
        Camera.onPreRender -= UpdateMVP;
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


        Matrix4x4 modelMatrix = new Matrix4x4( new Vector4(        coordRight.x,         coordRight.y,           coordRight.z,       0.0f),
                                               new Vector4(           coordUp.x,            coordUp.y,              coordUp.z,       0.0f),
                                               new Vector4(      lookAtVector.x,       lookAtVector.y,         lookAtVector.z,       0.0f), 
                                               new Vector4(                0.0f,                  0.0f,                  0.0f,       1.0f)) * scaleMatrix;
        
        // MVP matrix aka UnityObjectToClip matrix. 
        Matrix4x4 objectToClip = GL.GetGPUProjectionMatrix(cam.projectionMatrix, true)
            * cam.worldToCameraMatrix * modelFoward.localToWorldMatrix
            * modelMatrix /** mdelTransform.GetComponent<Renderer>().localToWorldMatrix*/ * modelFoward.worldToLocalMatrix* mdelTransform.localToWorldMatrix;
        
        Shader.SetGlobalMatrix("_oToC", objectToClip);
	}
}
