using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour {

    public float xAmplitude, yAmplitude, zAmplitude;
    public float xFrequency, yFrequency, zFrequency;


    private Vector3 StartPosition;

    // Use this for initialization
    void Start () {
        StartPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float xD = Mathf.Sin((Time.time + 00.00f) * 1.0f /  xFrequency        ) * xAmplitude 
                 + Mathf.Sin((Time.time + 05.23f) * 1.0f / (xFrequency * 2.0f)) * xAmplitude*0.5f
                 + Mathf.Sin((Time.time + 51.07f) * 1.0f / (xFrequency * 4.0f)) * xAmplitude*0.25f;

        float yD = Mathf.Sin((Time.time + 00.00f) * 1.0f /  yFrequency        ) * yAmplitude 
                 + Mathf.Sin((Time.time + 05.23f) * 1.0f / (yFrequency * 2.0f)) * yAmplitude*0.5f
                 + Mathf.Sin((Time.time + 51.07f) * 1.0f / (yFrequency * 4.0f)) * yAmplitude*0.25f;

        float zD = Mathf.Sin((Time.time + 00.00f) * 1.0f /  zFrequency        ) * zAmplitude 
                 + Mathf.Sin((Time.time + 05.23f) * 1.0f / (zFrequency * 2.0f)) * zAmplitude*0.5f
                 + Mathf.Sin((Time.time + 51.07f) * 1.0f / (zFrequency * 4.0f)) * zAmplitude*0.25f;

        this.transform.position = StartPosition + new Vector3(xD, yD, zD);
		
	}
}
