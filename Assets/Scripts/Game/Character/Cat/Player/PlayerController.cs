using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WFPIR.Game
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput playerInput { get; set; } // for switching action maps
        private Rigidbody2D playerRigidbody;
        private Animator animator;
        private string animationState;
        private bool moving;
        private float movementSpeed = 7f;
        private Vector2 movementInput;
        private bool isGrounded;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            playerInput = GetComponent<PlayerInput>();
            playerRigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!isGrounded) return;

            playerRigidbody.velocity += new Vector2(0, 5);
            print("jump");
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == "Environment")
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.tag == "Environment")
            {
                isGrounded = false;
            }
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            if (!IsMoving())
            {
                IdleAnimationHandler();
                return;
            }

            MovementAnimationHandler();

            Vector3 movementDirection = new Vector3(movementInput.x, 0, 0);
            movementDirection = movementDirection * movementSpeed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movementDirection);

            //transform.position += movementDirection;
        }

        public bool IsMoving()
        {
            if (movementInput == Vector2.left || movementInput == Vector2.right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ProcessMovementInput(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void MovementAnimationHandler()
        {
            if (movementInput == Vector2.left)
            {
                animator.transform.rotation = new Quaternion(0, 180, 0, 0);
                PlayPlayerAnimation("Player_Walk_anim");
            }
            else if (movementInput == Vector2.right)
            {
                animator.transform.rotation = new Quaternion(0, 0, 0, 0);
                PlayPlayerAnimation("Player_Walk_anim");
            }
        }

        private void IdleAnimationHandler()
        {
            if (animationState == "Player_Walk_anim")
            {
                PlayPlayerAnimation("Player_Idle_Variant_0_anim");
            }
        }

        public void PlayPlayerAnimation(string newAnimationState)
        {
            if (animationState == newAnimationState) return;

            animator.Play(newAnimationState);

            animationState = newAnimationState;
        }
    }



}
