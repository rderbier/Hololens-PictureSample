using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.WebCam;


public class PhotoCamera : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
  
    public GameObject PhotoPrefab;
    
    Resolution cameraResolution;
    
    float ratio = 1.0f;
    AudioSource shutterSound;

    // Use this for initialization
    void Start()
    {
        shutterSound = GetComponent<AudioSource>() as AudioSource;
        Debug.Log("File path " + Application.persistentDataPath);
        cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        
        ratio = (float)cameraResolution.height / (float)cameraResolution.width;

        // Create a PhotoCapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
            photoCaptureObject = captureObject;
            Debug.Log("camera ready to take picture");
        });
        
    }
    public void StopCamera()
    {
        // Deactivate our camera
        
        photoCaptureObject?.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
    public void TakePicture()
    {
        CameraParameters cameraParameters = new CameraParameters();
        cameraParameters.hologramOpacity = 0.0f;
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;
        cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

        // Activate the camera
        if (photoCaptureObject != null)
        {
            if (shutterSound != null)
            {
                shutterSound.Play();
            }
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        }
    }

    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        // Copy the raw image data into our target texture
        var targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        
        GameObject newElement = Instantiate<GameObject>(PhotoPrefab);
        GameObject quad = newElement.transform.Find("Quad").gameObject;
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material.mainTexture = targetTexture;
        // new Material(Shader.Find("Unlit/Texture"));

        // Set position and rotation 
        // Bug in Hololens v2 and Unity 2019 about PhotoCaptureFrame not having the location data - March 2020
        // 
        // Matrix4x4 cameraToWorldMatrix;
        // photoCaptureFrame.TryGetCameraToWorldMatrix(out cameraToWorldMatrix);
        //  Vector3 position = cameraToWorldMatrix.MultiplyPoint(Vector3.zero);
        //  Quaternion rotation = Quaternion.LookRotation(-cameraToWorldMatrix.GetColumn(2), cameraToWorldMatrix.GetColumn(1));
        // Vector3 cameraForward = cameraToWorldMatrix * Vector3.forward;






        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.Normalize();
        newElement.transform.position  = Camera.main.transform.position + (cameraForward * 0.6f);
        
        newElement.transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);
        Vector3 scale = newElement.transform.localScale;
        scale.y = scale.y* ratio; // scale the entire photo on height
        newElement.transform.localScale = scale;




    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
}