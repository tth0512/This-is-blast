using UnityEngine;

public static class UIToWorldHelpers
{
    public static Vector3 RectTransformToWorldPosition(RectTransform rt, Canvas canvas, Camera worldCamera, float targetWorldZ)
    {
        if (rt == null) return Vector3.zero;
        if (canvas == null) canvas = rt.GetComponentInParent<Canvas>();
        if (worldCamera == null) worldCamera = Camera.main;

        Vector2 screenPoint;

        Camera camForUI = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? (canvas.worldCamera ?? worldCamera) : null;
        screenPoint = RectTransformUtility.WorldToScreenPoint(camForUI, rt.position);

        float zDistance = targetWorldZ - worldCamera.transform.position.z;
        Vector3 screenPointWithZ = new Vector3(screenPoint.x, screenPoint.y, Mathf.Abs(zDistance));

        Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPointWithZ);

        worldPos.z = targetWorldZ;
        return worldPos;
    }
}
