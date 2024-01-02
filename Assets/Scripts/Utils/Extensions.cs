using Structs;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions
{
    public static bool CheckLayer(this LayerMask layerMask, LayerMask layer)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    public static float GetAnglesFromDir(Vector3 position, Vector3 dir)
    {
        var forwardLimitPos = position + dir;
        var srcAngles = Mathf.Rad2Deg * Mathf.Atan2(forwardLimitPos.y - position.y, forwardLimitPos.x - position.x);

        return srcAngles;
    }

    public static float NormalizeAngle(float angle)
    {
        return angle < 0 ? 360 + angle : angle;
    }

    public static void DrawWireArc(Vector3 position, Vector3 dir, float anglesRange, float radius, float maxSteps = 20)
    {
        float srcAngles = GetAnglesFromDir(position, dir);
        Vector3 initialPos = position;
        Vector3 posA = initialPos;
        float stepAngles = anglesRange / maxSteps;
        float angle = srcAngles - anglesRange / 2;
        for (int i = 0; i <= maxSteps; i++)
        {
            float rad = Mathf.Deg2Rad * angle;
            Vector3 posB = initialPos;
            posB += new Vector3(radius * Mathf.Cos(rad), radius * Mathf.Sin(rad), 0);

            Gizmos.DrawLine(posA, posB);

            angle += stepAngles;
            posA = posB;
        }
        Gizmos.DrawLine(posA, initialPos);
    }

    public static string GetNextLevelName()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int levelIdx = Int32.Parse(currentSceneName.Split("_")[1]);
        int sublevelIdx = Int32.Parse(currentSceneName.Split("_")[2]);
        string sceneToLoad = String.Format("Level_{0}_{1}", levelIdx, sublevelIdx + 1);
        Debug.Log(sceneToLoad);

        if (SceneUtility.GetBuildIndexByScenePath(sceneToLoad) > -1)
            return sceneToLoad;
        else
            return "Hub";
    }

    public static void GizmosSetColor(float r, float g, float b, float a = 1f)
    {
        Gizmos.color = new Color(r, g, b, a);
    }

    public static void GizmosSetColor(Color color, float a = 1f)
    {
        Gizmos.color = new Color(color.r, color.g, color.b, a);
    }
}
