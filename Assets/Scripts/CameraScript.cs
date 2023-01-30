using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    
    static WebCamTexture backCam;
    void Start()
    {
        
        if (backCam == null)
            backCam = new WebCamTexture();

        GetComponent<Renderer>().material.mainTexture = backCam;

        if (!backCam.isPlaying)
            backCam.Play();
        
       
        var pixelData = backCam.GetPixels();

        //print("Total pixels" + pixelData.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
