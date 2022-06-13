using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXProcessor : MonoBehaviour
{
    [SerializeField]
    private GameObject hitPrefab;

    private Quaternion rotation;
    private Vector3 position;

    public void DisplayVFX(ContactPoint contact)
    {
        rotation = Quaternion.Euler(0, 0, -Vector3.forward.z);
        position = contact.point;
        Instantiate(hitPrefab, position, rotation);
    }
}
