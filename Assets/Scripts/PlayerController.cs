using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    LayerMask playerLayer;
    Touch touch;
    bool clicked;
    RaycastHit2D ray;
    Vector2 screenWp;
    Vector3 playerNewPos;

    Ray fromFrontCatapult;
    CircleCollider2D playerCircleCollider;
    Vector2 catpultToPlayer;
    Vector2 playerWidth;

    [SerializeField]
    LineRenderer lineBack, lineFront;
    [SerializeField]
    SpringJoint2D spring;
    Vector2 prevVel;
    Rigidbody2D playerRb;

    [SerializeField]
    GameObject playerDeathFx;

    //spring limits
    Transform backCatapult;
    Ray rayToTouchPos;
    [SerializeField]
    float dragLimit = 3.0f;

    void Start() {
        playerCircleCollider = GetComponent<CircleCollider2D>();

        spring = GetComponent<SpringJoint2D>();
        playerRb = GetComponent<Rigidbody2D>();

        fromFrontCatapult = new Ray(lineFront.transform.position, Vector3.zero);
        backCatapult = spring.connectedBody.transform;
        rayToTouchPos = new Ray(backCatapult.position, Vector3.zero);
        LineSetup();
    }

    // Update is called once per frame
    void Update() {
        LineUpdate();
        SpringEffect();
        prevVel = playerRb.velocity;

        #if UNITY_ANDROID
        PlayerMovementAndroid();
        #endif

        #if UNITY_EDITOR
         PlayerMovementEditor();
        #endif


        if (!clicked && !playerRb.isKinematic)
            DestroyPlayer();
    }

    void PlayerMovementEditor() {
        if (clicked) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LimitPlayerMovement(mousePos);
        }
    }

    void PlayerMovementAndroid() {
        if (Input.touchCount > 0) {
            touch = Input.GetTouch(0);
            screenWp = Camera.main.ScreenToWorldPoint(touch.position);
            ray = Physics2D.Raycast(screenWp, Vector2.zero, Mathf.Infinity, playerLayer.value);

            if (ray.collider) {
                clicked = true;
            }

            if (clicked) {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
                    playerNewPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                    LimitPlayerMovement(playerNewPos);
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                playerRb.isKinematic = false;
                clicked = false;
            }

        }
    }

    void LimitPlayerMovement(Vector3 inputPos) {
        catpultToPlayer = inputPos - backCatapult.position;


        inputPos.z = transform.position.z;

        if (catpultToPlayer.magnitude > dragLimit) {
            rayToTouchPos.direction = catpultToPlayer;
            inputPos = rayToTouchPos.GetPoint(dragLimit);
        }

        transform.position = inputPos;
    }

    void LineSetup() {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);
    }

    void LineUpdate() {
        catpultToPlayer = transform.position - lineFront.transform.position;
        fromFrontCatapult.direction = catpultToPlayer;

        playerWidth = fromFrontCatapult.GetPoint(catpultToPlayer.magnitude + playerCircleCollider.radius);

        lineFront.SetPosition(1, playerWidth);
        lineBack.SetPosition(1, playerWidth);
    }

    void SpringEffect() {
        if(spring != null) {
            if (!playerRb.isKinematic) {
                if (prevVel.sqrMagnitude > playerRb.velocity.sqrMagnitude) {
                    lineFront.enabled = false;
                    lineBack.enabled = false;

                    Destroy(spring);
                    playerRb.velocity = prevVel;
                }
            }
            
        }
    }

    void DestroyPlayer() {
        if(playerRb.velocity.magnitude <= 0.0f && !playerRb.IsSleeping()) {
            StartCoroutine(PlayerDeath(2.0f));
        }
    }

    IEnumerator PlayerDeath(float seconds) {
        yield return new WaitForSeconds(seconds);
        Instantiate(playerDeathFx, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
    }

    void OnMouseDown() {
        clicked = true;
    }

    void OnMouseUp() {
        playerRb.isKinematic = false;
        clicked = false;
    }

}
