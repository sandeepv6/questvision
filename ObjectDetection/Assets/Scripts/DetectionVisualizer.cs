using UnityEngine;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;
using PimDeWitte.UnityMainThreadDispatcher;

public class DetectionVisualizer : MonoBehaviour
{
    public Camera displayCamera; // Should be CenterEyeAnchor
    public RectTransform canvasRect; // Assign your UI canvas here
    public GameObject boundingBoxPrefab; // Prefab with Image + TextMeshProUGUI

    private ClientWebSocket ws;
    private List<GameObject> currentBoxes = new List<GameObject>();

    async void Start()
    {
        ws = new ClientWebSocket();
        await ws.ConnectAsync(new Uri("ws://192.168.2.173:8765"), CancellationToken.None);
        _ = ListenForDetections(); // Fire-and-forget
    }

    async Task ListenForDetections()
    {
        var buffer = new byte[8192];

        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string json = Encoding.UTF8.GetString(buffer, 0, result.Count);

            // Parse detections
            JArray detections = JArray.Parse(json);
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                DrawBoxes(detections);
            });
        }
    }

    void DrawBoxes(JArray detections)
    {
        // Clear previous boxes
        foreach (var box in currentBoxes)
            Destroy(box);
        currentBoxes.Clear();

        foreach (var detection in detections)
        {
            var bbox = detection["bbox"];
            float x1 = bbox[0].Value<float>();
            float y1 = bbox[1].Value<float>();
            float x2 = bbox[2].Value<float>();
            float y2 = bbox[3].Value<float>();
            string label = detection["label"].Value<string>();

            float width = x2 - x1;
            float height = y2 - y1;

            // Normalize (assumes 640x640 capture size)
            float normX = x1 / 640f;
            float normY = y1 / 640f;
            float normW = width / 640f;
            float normH = height / 640f;

            // Convert to canvas space
            Vector2 screenPos = new Vector2(normX, 1 - normY); // Flip Y for UI

            // Create UI Box
            GameObject boxGO = Instantiate(boundingBoxPrefab, canvasRect);
            boxGO.GetComponent<RectTransform>().anchorMin = screenPos;
            boxGO.GetComponent<RectTransform>().anchorMax = new Vector2(screenPos.x + normW, screenPos.y - normH);
            boxGO.GetComponentInChildren<TextMeshProUGUI>().text = label;

            currentBoxes.Add(boxGO);
        }
    }
}
