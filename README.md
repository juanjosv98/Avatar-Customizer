# AI Avatar Creator Prototype

This is a **Unity** prototype built during a hackathon to explore interactive avatar customization using the **Ready Player Me** SDK.  
The project focuses for now on providing a simple UI and a text-based command interface to change avatar features such as body shape and hairstyle.

## Features

- Loads and displays a Ready Player Me avatar in Unity  
- UI controls to modify avatar properties (e.g. body type, hairstyles)  
- Text command panel to trigger avatar changes via typed commands  
- Code structured so that a Speech-to-Text system can later be connected to the same command pipeline  
- Built as a lightweight prototype that can be extended into a full XR / Meta Quest experience

## Technologies Used

- **Unity 6**  
- **C#**  
- **Ready Player Me Core / Avatar Creator Elements**  

## 📂 Project Structure

- `Assets/` – project scripts, scenes, UI, and Ready Player Me sample assets  
- `Packages/` – Unity package manifest and dependencies  
- `ProjectSettings/` – Unity project configuration  

## Status
We integrated the Ready Player Me SDK into the Unity project, built the UI for avatar customization, and implemented the text-based command system that connects user inputs to avatar changes in real time.  
The goal of the hackathon was to demonstrate how users could eventually control and customize an avatar using natural language (via future Speech-to-Text integration).
The project can be further extended with:

- full Speech-to-Text integration  
- XR support for Meta Quest  
- additional avatar customization options and UI polish

