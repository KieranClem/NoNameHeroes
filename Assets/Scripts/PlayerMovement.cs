using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public StartBattle LoadManager;
    public InformationStorage Info;

    private void Start()
    {
        Info = GameObject.FindGameObjectWithTag("InfoStorage").GetComponent<InformationStorage>();
        if(Info.BattleNumber > 0)
        {
            this.transform.position = Info.Position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * MovementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Info.Position = this.transform.position;
            Info.EnemiesFought.Add(collision.name);
            LoadManager.ChangeToBattleScene();
        }
    }
}
