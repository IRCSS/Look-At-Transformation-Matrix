using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Manager : MonoBehaviour {


    public Transform LookAt;

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

        Vector3 lookAtVector = (LookAt.transform.position - this.transform.position).normalized;
        float   angle        = 1f * Mathf.Deg2Rad * Vector3.Angle(lookAtVector, this.transform.forward);
        Vector3 coordRight   = this.transform.forward;
        if (Vector3.Dot(coordRight, lookAtVector) == 1) coordRight = this.transform.right;
        Vector3.OrthoNormalize(ref lookAtVector, ref coordRight);
        // Vector3 coordUp      = Vector3.Cross(lookAtVector, coordRight).normalized;+
        Vector3 coordUp = this.transform.up;
        Vector3.OrthoNormalize(ref lookAtVector, ref coordUp);
        Vector3.OrthoNormalize(ref coordRight, ref coordUp);


        Matrix4x4 wToRotation = new Matrix4x4( new Vector4(                 1.0f,                  0.0f,                  0.0f,   0.0f)* 1f,
                                               new Vector4(                 0.0f,                  1.0f,                  0.0f,   0.0f)* 1f,
                                               new Vector4(                 0.0f,                  0.0f,                  1.0f,   0.0f)* 1f, 
                                               new Vector4(-transform.position.x, -transform.position.y, -transform.position.z,   1.0f)* 1f);

                  wToRotation = new Matrix4x4( new Vector4(  coordRight.x,         coordRight.y,         coordRight.z,       0.0f)*-1f,
                                               new Vector4(     coordUp.x,            coordUp.y,            coordUp.z,       0.0f)* 1f,
                                               new Vector4(lookAtVector.x,       lookAtVector.y,       lookAtVector.z,       0.0f)* 1f, 
                                               new Vector4(          0.0f,                 0.0f,                 0.0f,       1.0f)) * wToRotation;
        

        Matrix4x4 lookatRotation = new Matrix4x4( new Vector4( Mathf.Cos(angle),       0.0f,   Mathf.Sin(angle),   0.0f),
                                                  new Vector4(             0.0f,       1.0f,               0.0f,   0.0f),
                                                  new Vector4(-Mathf.Sin(angle),       0.0f,   Mathf.Cos(angle),   0.0f), 
                                                  new Vector4(             0.0f,       0.0f,               0.0f,   1.0f)).transpose;

        // MVP matrix aka UnityObjectToClip matrix. 
        Matrix4x4 objectToClip = GL.GetGPUProjectionMatrix(cam.projectionMatrix, true)
            * cam.worldToCameraMatrix
            * wToRotation.inverse
            * lookatRotation
            * wToRotation
            * transform.GetComponent<Renderer>().localToWorldMatrix;
        
        Shader.SetGlobalMatrix("_oToC", objectToClip);
	}
}
