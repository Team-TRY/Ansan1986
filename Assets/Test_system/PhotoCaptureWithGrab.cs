using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhotoCaptureWithGrab : XRGrabInteractable
{
    private Camera cameraToUse;
    private bool hasShot = false; // ���ڿ�����������Ĳ���ֵ

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!hasShot)
        {
            TakePhoto();
            hasShot = true; // ���պ�����Ϊtrue����ֹ�ٴ�����
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        hasShot = false; // �ͷ�ʱ��������״̬�������ٴ�����
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
