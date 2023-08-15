using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnRotation : MonoBehaviour
{
    public void SetRotation(Vector3 target)
    {
        transform.LookAt(target, Vector3.up);
    }
}
