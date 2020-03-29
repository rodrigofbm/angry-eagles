﻿using System;
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
    [SerializeField]
    Vector2 prevVel;
    Rigidbody2D playerRb;

    void Start() {
        playerCircleCollider = GetComponent<CircleCollider2D>();

        spring = GetComponent<SpringJoint2D>();
        playerRb = GetComponent<Rigidbody2D>();

        fromFrontCatapult = new Ray(lineFront.transform.position, Vector3.zero);
        LineSetup();
    }

    // Update is called once per frame
    void Update() {
        LineUpdate();
        SpringEffect();
        prevVel = playerRb.velocity;

        PlayerMovement();
    }

    void PlayerMovement() {
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

                    transform.position = playerNewPos;

                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                playerRb.isKinematic = false;
                clicked = false;
            }

        }
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

}
