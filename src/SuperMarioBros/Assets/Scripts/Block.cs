using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Block : MonoBehaviour {

    protected scr_GameController gC { get { return scr_GameController.instance; } }


    protected GameObject obj;
    protected int qnt = 1;

    protected Rigidbody2D rb;
    protected Animator anim;

    protected Vector2 initPos;

    protected void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        initPos = transform.position;
    }
}
