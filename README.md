# Hololens-Picture Sample


This project shows how to use windows [media capture](https://msdn.microsoft.com/library/windows/apps/windows.media.capture.mediacapture.aspx) in Hololens 2 to access the Camera, take picture and use the resulting image as a texture in Unity to create an augmented reality photograph.

Refer to [Locatable camera info from Microsoft](https://docs.microsoft.com/en-us/windows/mixed-reality/locatable-camera) for details on the Device Camera.

[Locatable camera in Unity](https://docs.microsoft.com/en-us/windows/mixed-reality/locatable-camera-in-unity) provides the key API and sample scripts.
### Dependencies
- MRTK Foundation 2.3.0

### Install
Open the project in Unity 2019.2.17f1

Import MRTK 2.3.0 Foundation.

Unity should ask to install Text Mesh Pro.

Verify that
- 'App MixedRealitySpeechCommandsProfile' in CustomProfile folder contains the key word "Take Picture".
- your build settings is correctly set for Hololens 2
- The build contains the scene TakePictureScene

Build, deploy to Hololens 2 (through Visual Studio).

The App should request access to Camera and Microphone, accept !

Enjoy taking Picture by just saying "Take Picture"

### Next Steps
- Add a visor
- Add annotation capability
