using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLobby : MonoBehaviour
{
    public static CanvasLobby self;
    public static Canvas canvas;

    private void Awake()
    {
        self = this;
    }
}
