using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoCaptureWithGrab : XRGrabInteractable
{
    private Camera cameraToUse;
    private bool hasShot = false; // 用于控制拍摄次数的布尔值

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!hasShot)
        {
            TakePhoto();
            hasShot = true; // 拍照后设置为true，防止再次拍照
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        hasShot = false; // 释放时重置拍照状态，允许再次拍照
    }

    void Start()
    {
        cameraToUse = GetComponentInChildren<Camera>();
        if (cameraToUse == null)
        {
            Debug.LogError("No Camera component found in children!");
        }
    }
    public void TakePhoto()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToUse.targetTexture = renderTexture;
        Texture2D photo = new Texture2D(cameraToUse.targetTexture.width, cameraToUse.targetTexture.height, TextureFormat.RGB24, false);
        cameraToUse.Render();
        RenderTexture.active = cameraToUse.targetTexture;
        photo.ReadPixels(new Rect(0, 0, cameraToUse.targetTexture.width, cameraToUse.targetTexture.height), 0, 0);
        photo.Apply();
        cameraToUse.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        byte[] bytes = photo.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/SavedPhoto.png", bytes);

        Debug.Log(Application.persistentDataPath + "/SavedPhoto.png" );
    }
}
