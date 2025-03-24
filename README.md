# 🖼️ Unity Web Image Downloader (Singleton Library)

This is a **Unity 2D/3D Web Image Downloader** that efficiently fetches and caches images from the internet. It **limits downloads to 3 concurrent requests** and **supports disk/memory caching**.

## 🚀 Features
✔️ **Singleton-Based** – Easy to use from any script.  
✔️ **Concurrent Download Limit** – Prevents performance drops by allowing only 3 active downloads.  
✔️ **Image Caching** – Avoids re-downloading images by storing them on disk.  
✔️ **Cache Expiry** – Automatically invalidates images older than **7 days**.  
✔️ **Timeout Handling** – Cancels downloads waiting more than **10 seconds**.  
✔️ **Supports Alpha Images** – Works with PNGs that have transparency.  

---

## 🛠️ Setup & Installation

### 1️⃣ Clone or Download
```sh
git clone <your-repo-url>
