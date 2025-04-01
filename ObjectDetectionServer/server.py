import asyncio
import websockets
import base64
import numpy as np
import cv2
from ultralytics import YOLO
import json
from websockets.exceptions import ConnectionClosedOK, ConnectionClosedError

# --- CONFIGURATION ---
YOLO_MODEL_PATH = "yolov8m.pt"  # You can use yolov8n.pt, yolov8s.pt, etc.
CONFIDENCE_THRESHOLD = 0.25     # Lower this if no detections show up
SAVE_DEBUG_FRAME = True        # Set to True to save frames as images
DEBUG_FRAME_PATH = "test.jpg"
PORT = 8765

# --- Load YOLO Model ---
model = YOLO(YOLO_MODEL_PATH)
print(f"✅ YOLOv8 model loaded: {YOLO_MODEL_PATH}")

# --- Handle Incoming Connections ---
async def handle_connection(websocket):
    print("🔌 Client connected")
    try:
        while True:
            try:
                # Timeout if Unity stops sending
                message = await asyncio.wait_for(websocket.recv(), timeout=10)
            except asyncio.TimeoutError:
                print("⚠️ No data received in 10s. Closing connection.")
                break

            # Decode base64 image
            jpg_data = base64.b64decode(message)
            np_arr = np.frombuffer(jpg_data, np.uint8)
            frame = cv2.imdecode(np_arr, cv2.IMREAD_COLOR)

            if SAVE_DEBUG_FRAME:
                cv2.imwrite(DEBUG_FRAME_PATH, frame)

            # Run YOLO detection
            results = model(frame, verbose=False)[0]
            print(f"📦 Detected {len(results.boxes)} object(s)")

            # Prepare results
            detections = []
            for box in results.boxes:
                conf = float(box.conf[0])
                if conf < CONFIDENCE_THRESHOLD:
                    continue

                x1, y1, x2, y2 = map(float, box.xyxy[0])
                cls = int(box.cls[0])
                label = model.names[cls]

                detections.append({
                    "label": label,
                    "confidence": conf,
                    "bbox": [x1, y1, x2, y2]
                })

            # Send JSON back to Unity
            await websocket.send(json.dumps(detections))

    except (ConnectionClosedOK, ConnectionClosedError):
        print("❎ Client disconnected.")
    except Exception as e:
        print(f"❌ Server error: {e}")

# --- Main Entry Point ---
async def main():
    print(f"🚀 Starting server on ws://localhost:{PORT}")
    async with websockets.serve(handle_connection, "localhost", PORT, close_timeout=1):
        await asyncio.Future()  # Run forever

if __name__ == "__main__":
    asyncio.run(main())
    print("🚨 Server stopped.")