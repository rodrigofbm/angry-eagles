using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFX : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 2.0f);
    }
}
