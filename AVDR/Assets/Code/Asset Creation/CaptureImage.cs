using System;
using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CaptureImage : MonoBehaviour
{
    // public RenderTexture renderTexture;
    public int size = 1024;
    new public Camera camera;

    private RenderTexture temporaryRenderTexture;
    private Texture2D texture2D;

    void Start() {
        // CaptureScreen();
    }

    public void CaptureScreen() {
        PerformCaptureTest();
        // ScreenCapture.CaptureScreenshot(path, 1);
    }

    public void CaptureAllAssets() {
        StartCoroutine(CaptureAllCoroutine());
    }

    private IEnumerator CaptureAllCoroutine() {
        /* get ready */
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        Debug.LogWarning("Capture All Assets Started.");
        // temporaryRenderTexture = RenderTexture.GetTemporary(size, size, 16, RenderTextureFormat.ARGB32);
        // camera.targetTexture = temporaryRenderTexture;
        // texture2D = new Texture2D(size, size, TextureFormat.RGB24, false);
        // RenderTexture original = RenderTexture.active;
        // RenderTexture.active = temporaryRenderTexture;
        
        /* get the assets we need to capture */
        AssetManager assetManager = FindFirstObjectByType<AssetManager>();
        AKVPDice[] dicePairs = assetManager.All.diceSets;

        /* capture each asset */
        yield return CaptureDiceSets(dicePairs, AssetType.DiceSet);
        
        /* finish up */
        // if(Application.isPlaying) {
        //     Destroy(texture2D);
        // }
        // else {
        //     DestroyImmediate(texture2D);
        // }
        // RenderTexture.ReleaseTemporary(temporaryRenderTexture);
        // camera.targetTexture = null;
        // RenderTexture.active = original;
        stopwatch.Stop();
        Debug.Log("Capture All Assets completed in " + stopwatch.Elapsed + " s");
    }

    /// <summary>
    /// Iterates over the given assets and creates a screenshot and saves it.
    /// </summary>
    /// <param name="dicePairs"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator CaptureDiceSets(AKVPDice[] dicePairs, AssetType type) {
        yield return new WaitForSeconds(1);
        foreach(AKVPDice pair in dicePairs) {
            Debug.Log(pair.key);
            string path = "Resources/Screenshots/Dice/" + pair.key + ".png";
            PerformSerialCapture(path);
        }
    }

    private void PerformSerialCapture(string path) {
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
            Destroy(texture2D);
        }
        else {
            DestroyImmediate(texture2D);
        }
    }
    
    private void PerformCaptureTest() {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        
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
        
        string path = "Resources/Screenshots/Test/" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss SC") + ".png";
        SaveToDisk(texture2D, path);

        if(Application.isPlaying) {
            Destroy(texture2D);
        }
        else {
            DestroyImmediate(texture2D);
        }

        Debug.Log($"rendered in {stopwatch.Elapsed.Milliseconds} ms");
    }

    private void SaveToDisk(Texture2D texture2D, string localPath) {
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
