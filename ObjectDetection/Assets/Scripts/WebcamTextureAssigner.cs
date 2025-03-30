using PassthroughCameraSamples;
using System.Collections;
using UnityEngine;

/// <summary>
/// Assigns the <see cref="WebCamTexture"/> of the Quest camera
/// to the <see cref="Renderer"/> component of the current game object.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class WebcamTextureAssigner : MonoBehaviour
{
    /// <summary>
    /// Start coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        //references that will be filled by the coroutine
        WebCamTextureManager webCamTextureManager = null;
        WebCamTexture webCamTexture = null;

        //wait until the WebCamTextureManager is found and is ready to provide a texture
        do
        {
            yield return null;

            //if the WebCamTextureManager is not found yet, find it
            if (webCamTextureManager == null)
            {
                webCamTextureManager = FindFirstObjectByType<WebCamTextureManager>();
            }
            //else, if we have it, try to get the texture of the camera of the headset
            else
            {
                webCamTexture = webCamTextureManager.WebCamTexture;
            }
        } while (webCamTexture == null);

        //here we have the texture. Assign it to the main texture of the main material of the renderer
        GetComponent<Renderer>().material.mainTexture = webCamTexture;
    }
}