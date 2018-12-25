using UnityEngine;

public static class GameObjectExt
{
    public static void SetSelfActive(this GameObject obj, bool active)
    {
        if(obj.activeSelf == active)
            return;
        obj.SetActive(active);
    }
}