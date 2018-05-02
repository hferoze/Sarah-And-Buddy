using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPermissionScript : MonoBehaviour {

    public static bool CheckPermissions(string permission)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            return true;
        }

        return AndroidPermissionsManager.IsPermissionGranted(permission);
    }
}
