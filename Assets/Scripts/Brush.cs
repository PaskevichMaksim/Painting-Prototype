using System;
using UnityEngine;

[Serializable]
public class Brush
{
    public Color Color;
    public int Size;

    public Brush (Color color, int size)
    {
        Color = color;
        Size = size;
    }
}
