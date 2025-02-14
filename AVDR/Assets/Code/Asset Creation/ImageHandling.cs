using System.IO;
using UnityEngine;

public static class ImageHandling
{
    public static void PerformSerialCapture(Camera camera, int size, string path) {
        RenderTexture temporary = RenderTexture.GetTemporary(size, size, 16, RenderTextureFormat.ARGB32);
        camera.targetTexture = temporary;

        camera.Render();

        Texture2D texture2D = new Texture2D(size, size, TextureFormat.RGB24, false);

        RenderTexture original = RenderTexture.active;
        RenderTexture.active = temporary;
        
        texture2D.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        RenderTexture.ReleaseTemporary(temporary);
        camera.targetTexture = null;
        RenderTexture.active = original;
        texture2D.Apply();
        
        SaveToDisk(texture2D, path);

        if(Application.isPlaying) {
            GameObject.Destroy(texture2D);
        }
        else {
            GameObject.DestroyImmediate(texture2D);
        }
    }

    public static void ScreenCaptureCaptureScreen(Camera camera, int size, string path) {
        ScreenCapture.CaptureScreenshot(path);
    }

    public static void SaveToDisk(Texture2D texture2D, string localPath) {
        if(texture2D == null) {
            Debug.LogError("No texture to save.");
            return;
        }
        if(string.IsNullOrEmpty(localPath)) {
            Debug.LogError("path was empty");
            return;
        }
        byte[] bytes = texture2D.EncodeToPNG();
        string path = Application.dataPath + "/" + localPath;
        File.WriteAllBytes(path, bytes);
        Debug.Log(path);
    }
}