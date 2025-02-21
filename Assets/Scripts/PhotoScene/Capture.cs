using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture renderTexture;
    public Image background;

    private void Start()
    {
        cam = Camera.main;
    }

    public void Create()
    {
        StartCoroutine(CaptureImage());
    }

    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, true);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);

        yield return null;

        var data = texture.EncodeToPNG();
        string name = "Thumbnail";
        string extension = ".png";
        string path = Application.persistentDataPath + "/Thumbnails/";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + extension, data);

        yield return null;
    }
}
