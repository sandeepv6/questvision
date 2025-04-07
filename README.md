# 🧠 AI-Powered Object Detection & 3D Bounding Boxes in Mixed Reality (Quest 3)

This project brings real-time object detection to **mixed reality** using the **Meta Quest 3**. It combines Unity's passthrough APIs, **Meta’s PassthroughCamera API**, and **Unity Sentis** for AI inference with **YOLO**, anchoring object detections into the physical world.

---

## 🚀 Features

- 🔍 **Object Detection** using YOLO model via Unity Sentis
- 📷 **Passthrough Camera** integration for full-color MR vision
- 📦 **2D & 3D Bounding Boxes** rendered in screen space and real-world coordinates
- 🎯 **Scene Understanding Raycasting** to anchor detections in 3D
- 📋 **Live Detection List Panel** in AR HUD
- ⚡ Optimized for Meta Quest 3 (OpenXR, Android build)

---

## 📸 Preview

> _(Include screenshots or GIFs of bounding boxes overlaying real-world objects here)_

---

## 🛠️ Tech Stack

| Technology | Role |
|------------|------|
| Unity 2022+ | Core development environment |
| Unity Sentis | AI inference (YOLO) |
| Meta PassthroughCamera API | Access to passthrough feed |
| Meta Scene Understanding | World raycasting for 3D anchors |
| OpenXR + Meta Quest Support | XR integration |
| WebCam fallback (Editor) | Editor-based testing without Quest builds |

---

## 📦 Setup Instructions

### ✅ Prerequisites
- Meta Quest 3 with Developer Mode enabled
- Unity 2022+ with OpenXR
- [Meta XR SDK + PassthroughCamera Samples](https://developer.oculus.com/downloads/package/meta-xr-all-in-one-sdk/)
- Unity Sentis package (via Package Manager)
- A YOLO model (.onnx) configured for Sentis

---

### 🧱 Unity Setup (Simplified)
1. Create a new Unity project with **3D (URP)** or **Built-in Pipeline**
2. Import:
   - Meta XR SDK / PassthroughCamera Samples
   - Unity Sentis
3. Enable XR:
   - Install **OpenXR** and enable **Meta Quest Support**
   - Under Android → check:
     - ☑ Passthrough
     - ☑ Scene
     - ☑ Scene Anchors
4. Add permissions:
   - In PassthroughCameraPermissions script or manifest:
     ```
     com.oculus.permission.USE_SCENE
     com.oculus.permission.PASSTHROUGH_CAMERA
     ```
5. Build settings:
   - IL2CPP, ARM64
   - Min API Level 31 (Android 12)

---

## 🧠 How It Works

1. The **passthrough feed** is captured via Meta’s API
2. Every frame is sent to **Unity Sentis**, which runs YOLO object detection
3. Bounding boxes are drawn in 2D using Unity UI
4. A 3D **raycast** is performed for each detection to anchor it in world space
5. Detected objects are shown in a **floating detection panel** in AR

---

## 💡 Example Use Cases

- AR productivity apps that track objects on desks
- Real-world object recognition training
- Accessible tech: real-time feedback for visually impaired users
- MR games with object-based interaction

---

## 🧪 Known Issues

- Raycast may log `EnvironmentRaycastManager is not supported` if Scene Understanding is not properly enabled or Quest permissions aren’t granted.
- YOLO confidence score placeholder (`1.0`) — customize with your model output if needed.
- Must build to Quest to test raycast functionality (won’t work in Editor).

---

## 📌 TODO / Roadmap

- [ ] Add audio feedback for detections
- [ ] Switch YOLO model dynamically (small/medium/large)
- [ ] Add tap-to-select or hand tracking interaction
- [ ] Export detected object logs for analysis

---

## 📜 License

MIT License (or your choice)

---

## 🙌 Credits

- Meta XR SDK + PassthroughCamera Samples
- Unity Sentis team
- YOLO open-source model community

---

## 👋 Author

Built by Sandeep Virk as part of a university CV/AR project using Unity + Meta XR + AI.  
Feel free to fork, contribute, or reach out!


