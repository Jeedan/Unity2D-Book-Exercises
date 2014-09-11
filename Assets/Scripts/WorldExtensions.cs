using UnityEngine;
using System.Collections;

public static class WorldExtensions
{
    public static Vector3 ToVector3_2D(this Vector3 coordinate)
    {
        return new Vector3(coordinate.x, coordinate.y, 0);
    }

    public static Vector2 GetScreenPositionIn2D(this Vector3 screenCoordinate)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(screenCoordinate);
        return new Vector2(wp.x, wp.y);
    }

    public static Vector3 GetScreenPositionFor2D(this Vector3 screenCoordinate)
    {
        Vector3 wp = Camera.main.ScreenToWorldPoint(screenCoordinate);
        return wp.ToVector3_2D();
    }
}
