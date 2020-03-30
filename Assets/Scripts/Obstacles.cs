using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour {
    int health = 0;
    SpriteRenderer spriteRender;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    GameObject destroyFx;

    void Start() {
        spriteRender = GetComponent<SpriteRenderer>();
        spriteRender.sprite = sprites[health];
    }

    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.relativeVelocity.magnitude > 4.0f && other.relativeVelocity.magnitude < 10.0f) {
            UpdateObstacleSprite();
        } else if (other.relativeVelocity.magnitude >12.0f && other.gameObject.CompareTag("Player")) {
            RenderDestroyFx();
            Destroy(gameObject);
        }
    }

    void UpdateObstacleSprite() {
        if(health < sprites.Length - 1) {
            health++;
            spriteRender.sprite = sprites[health];
        }else {
            RenderDestroyFx();
            Destroy(gameObject);
        }
    }

    void RenderDestroyFx() {
        Instantiate(destroyFx, transform.position, Quaternion.identity);
    }
}
