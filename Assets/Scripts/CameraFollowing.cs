using UnityEngine;

public class CameraFollowing : MonoBehaviour {
  [SerializeField]
  Transform player, minLimit;
  Vector3 posCam, stageDimensions;
  float maxX, t = 1;

  void Update() {
    StartFollowing();
  }

  void StartFollowing() {
    stageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    maxX = stageDimensions.x - (stageDimensions.x / 1.3f);
    posCam = transform.position;

    posCam.x = player.position.x;
    //if the given value is no within the min and max, then return the min;
    posCam.x = Mathf.Clamp(posCam.x, minLimit.position.x, maxX);

    transform.position = posCam;


  }
}
