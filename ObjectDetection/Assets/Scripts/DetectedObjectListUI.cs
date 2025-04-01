/* using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DetectedObjectListUI : MonoBehaviour
{
    public TextMeshProUGUI detectedText;

    public void UpdateDetectedList(List<DetectionEntry> detections)
    {
        if (detections.Count == 0)
        {
            detectedText.text = "Detected:\n(none)";
            return;
        }

        string output = "Detected:\n";
        foreach (var det in detections)
        {
            output += $"- {det.label} ({det.confidence * 100f:F0}%)\n";
        }

        detectedText.text = output;
    }
}
*/