using UnityEngine;

public class PlayerDeathFx : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 3.0f);
    }

}
