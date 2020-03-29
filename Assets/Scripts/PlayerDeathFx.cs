using UnityEngine;

public class PlayerDeathFx : MonoBehaviour
{
    void Start() {
        Destroy(gameObject, 5.0f);
    }

}
