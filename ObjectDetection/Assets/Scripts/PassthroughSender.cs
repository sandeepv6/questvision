using System.Collections;
using UnityEngine;
using System.Net.WebSockets;
using System.Threading;
using System.Text;
using System;
using System.IO;
using System.Threading.Tasks;

public class PassthroughSender : MonoBehaviour
{
    public Camera captureCamera;
    public int width = 256;
    public int height = 256;

    private ClientWebSocket ws;
    private RenderTexture renderTex;
    private Texture2D tex2D;

    async void Start()
    {
        ws = new ClientWebSocket();
        await ws.ConnectAsync(new Uri("ws://192.168.2.173:8765"), CancellationToken.None);

        renderTex = new RenderTexture(width, height, 24);
        tex2D = new Texture2D(width, height, TextureFormat.RGB24, false);

        StartCoroutine(SendFrames());
    }

    IEnumerator SendFrames()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            // Capture camera output
            captureCamera.targetTexture = renderTex;
            captureCamera.Render();

            RenderTexture.active = renderTex;
            tex2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex2D.Apply();
            RenderTexture.active = null;
            captureCamera.targetTexture = null;

            // Encode to JPG
            byte[] jpgBytes = tex2D.EncodeToJPG();
            string base64 = Convert.ToBase64String(jpgBytes);
            byte[] bytesToSend = Encoding.UTF8.GetBytes(base64);

            // Call async send method
            _ = SendToServerAsync(bytesToSend);
        }
    }

    async Task SendToServerAsync(byte[] bytesToSend)
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            await ws.SendAsync(new ArraySegment<byte>(bytesToSend), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    async void OnApplicationQuit()
    {
        if (ws != null && ws.State == WebSocketState.Open)
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "App closed", CancellationToken.None);
        }
    }
}
