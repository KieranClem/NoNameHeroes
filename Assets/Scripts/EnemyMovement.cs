using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public InformationStorage info;
    public float MovementSpeed = 5f;
    public float walkdistance = 5f;
    private Vector2 FirstPosition;
    private Vector2 SecondPosition;
    private Vector2 movement;
    public Animator animator;
    public Rigidbody2D rb;
    private bool LeftRight = false;
    
    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.FindGameObjectWithTag("InfoStorage").GetComponent<InformationStorage>();
        if (info.EnemiesFought.Contains(this.name))
            Destroy(this.gameObject);
        movement = new Vector2(this.transform.position.x, 0);
        FirstPosition = new Vector2(this.transform.position.x + walkdistance, movement.y);
        SecondPosition = new Vector2(this.transform.position.x - walkdistance, movement.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (!LeftRight)
        {
            movement.x =+ 1;
            if (this.transform.position.x >= FirstPosition.x)
                LeftRight = true;
        }
        else
        {
            movement.x = -1f;
            if (this.transform.position.x <= SecondPosition.x)
                LeftRight = false;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * MovementSpeed * Time.fixedDeltaTime);
    }
}
