using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatusCanvasCamera
{
    Camera,
    RenderMode,
}

public class DontChangeCanvasCamera : MonoBehaviour
{
    public StatusCanvasCamera[] status;
}
