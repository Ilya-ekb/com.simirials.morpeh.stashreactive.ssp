# Morpeh.StashReactive.SSP

Reactive **Added/Removed** events for `Stash<T>` powered by **Morpeh.SystemStateProcessor**.  
Unity **2021.3+** (compatible with Unity 6) • License: **MIT** • Requires **Morpeh** + **SystemStateProcessor**.  [oai_citation:0‡GitHub](https://github.com/scellecs/morpeh?utm_source=chatgpt.com)

<p align="left">
  <a href="https://docs.unity3d.com/Manual/upm-ui-giturl.html"><img alt="UPM" src="https://img.shields.io/badge/UPM-Git%20URL-informational?logo=unity"></a>
  <a href="https://img.shields.io"><img alt="Unity" src="https://img.shields.io/badge/Unity-2021.3%2B-black?logo=unity"></a>
  <a href="https://opensource.org/licenses/MIT"><img alt="License: MIT" src="https://img.shields.io/badge/License-MIT-blue.svg"></a>
</p>

---

## ✨ Features
- Fire **Added** when an entity first matches `Filter.With<T>()`; fire **Removed** when it leaves the filter or is destroyed.
- Zero boilerplate: one system + two events.
- Plays nicely with other Morpeh plugins.

> Built on `SystemStateProcessor`: entities entering/leaving a filter trigger `Create/Remove` callbacks that you process each frame.  [oai_citation:1‡GitHub](https://github.com/codewriter-packages/Morpeh.SystemStateProcessor?utm_source=chatgpt.com)

---

## 📦 Requirements
- **Morpeh** ECS.  [oai_citation:2‡GitHub](https://github.com/scellecs/morpeh?utm_source=chatgpt.com)  
- **Morpeh.SystemStateProcessor** (reactivity helper).  [oai_citation:3‡GitHub](https://github.com/codewriter-packages/Morpeh.SystemStateProcessor?utm_source=chatgpt.com)

---

## 🛠 Installation

### 1) Install dependencies (Unity **Window → Package Manager** → **Add** → *Install package from git URL*)
- Morpeh:  
  ```
  https://github.com/scellecs/morpeh.git?path=Scellecs.Morpeh
  ```
   [oai_citation:4‡GitHub](https://github.com/scellecs/morpeh?utm_source=chatgpt.com)
- SystemStateProcessor:  
  ```
  https://github.com/codewriter-packages/Morpeh.SystemStateProcessor.git
  ```
   [oai_citation:5‡GitHub](https://github.com/codewriter-packages/Morpeh.SystemStateProcessor?utm_source=chatgpt.com)

> How to install from a Git URL via Package Manager (official docs).  [oai_citation:6‡Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/upm-ui-giturl.html?utm_source=chatgpt.com)

### 2) Install this package
- **Add package from git URL** → `https://github.com/Ilya-ekb/com.simirials.morpeh.stashreactive.ssp.git#v1.0.0`  
  *(or use **Add package from disk** and select `package.json` if you keep it locally).*

### 3) Import the sample
In Package Manager, open the package page and import **Samples → Basic Usage**. (Packages with a `Samples~` folder expose importable samples in the UI.)  [oai_citation:7‡Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/cus-samples.html?utm_source=chatgpt.com)

---

## 🚀 Quick Start

```csharp
using Morpeh.ReactiveSSP;
using Scellecs.Morpeh;

public struct MyComponent : IComponent { public int value; }

void Bootstrap() {
    var world = World.Default;

    var sys = world.AddReactiveStashSSP<MyComponent>(order: 50, emitExistingOnAwake: true);

    sys.Added   += e => UnityEngine.Debug.Log($"[MyComponent] ADDED   -> {e.ID}");
    sys.Removed += e => UnityEngine.Debug.Log($"[MyComponent] REMOVED -> {e.ID}");

    var stash = world.GetStash<MyComponent>();
    var ent = world.CreateEntity();
    stash.Add(ent);
    world.Commit(); // events processed on system's next Process()
}
```

Sample scene/script lives in **Samples → Basic Usage**.

---

## 🧠 How it works (under the hood)
- We build a filter `World.Filter.With<T>()` and pass **Create/Remove** handlers to `ToSystemStateProcessor`.
- Calling `processor.Process()` every frame invokes:
  - **Create(Entity e)** when the entity first matches the filter ⇒ emits **Added**.
  - **Remove(ref State s)** when the entity leaves the filter or is destroyed ⇒ emits **Removed**.
- The small `ISystemStateComponent` stores `Entity` for safe cleanup semantics.  [oai_citation:8‡GitHub](https://github.com/codewriter-packages/Morpeh.SystemStateProcessor?utm_source=chatgpt.com)

---

## 📚 Links
- Morpeh ECS (core) — repo & README.  [oai_citation:9‡GitHub](https://github.com/scellecs/morpeh?utm_source=chatgpt.com)  
- Morpeh.SystemStateProcessor — repo & README.  [oai_citation:10‡GitHub](https://github.com/codewriter-packages/Morpeh.SystemStateProcessor?utm_source=chatgpt.com)  
- Unity docs: *Install package from Git URL* (UPM).  [oai_citation:11‡Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/upm-ui-giturl.html?utm_source=chatgpt.com)  
- Unity docs: *Create package samples* (`Samples~`).  [oai_citation:12‡Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/cus-samples.html?utm_source=chatgpt.com)

---

## 📄 License
MIT — see `LICENSE.md`.

---

## 🗺 Roadmap
- [ ] Optional batching API for multiple `T` in one processor  
- [ ] Editor gizmos/logging switches  
- [ ] Extra helpers to bridge Morpeh.Events

---

> If you publish this as a GitHub repo, tag releases (`v1.0.0`) so projects can pin versions via UPM Git URLs. (Unity supports branches/commits/tags in Git URLs for Package Manager installs.)  [oai_citation:13‡Unity Documentation](https://docs.unity3d.com/2020.1/Documentation/Manual/upm-ui-giturl.html?utm_source=chatgpt.com)

---
