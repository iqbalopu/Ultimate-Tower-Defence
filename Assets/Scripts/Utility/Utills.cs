using System.Collections;
using System.Collections.Generic;
using System;

public static class Utills
{
    public static int ConvertRange(int originalStart, int originalEnd, int newStart, int newEnd, int value)
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (int)(newStart + ((value - originalStart) * scale));
    }
    
    public static float ConvertRange(float originalStart, float originalEnd, float newStart, float newEnd, float value)
    {
        float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }
}
