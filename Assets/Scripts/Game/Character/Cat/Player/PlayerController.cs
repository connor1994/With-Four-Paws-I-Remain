using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WFPIR.Game
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D playerRigidbody;
        private Animator animator;
        private string animationState;
        private float movementSpeed = 7f;
        private Vector2 movementInput;
        private bool isGrounded;
        private bool jumping;
        private bool running;
        private float jumpForce = 10f;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public void BeginJump(InputAction.CallbackContext context)
        {
            if (!isGrounded || IsMoving()) return;

            if (context.performed)
            {
                jumping = true;

                PlayPlayerAnimation("Player_Jump_anim");
            }
        }

        public void Jump()
        {
            if (transform.eulerAngles.y == 180)
            {
                playerRigidbody.AddForce((Vector2.up + Vector2.left) * jumpForce, ForceMode2D.Impulse);
                print("jump left");
            }
            else if (transform.eulerAngles.y == 0)
            {
                playerRigidbody.AddForce((Vector2.up + Vector2.right) * jumpForce, ForceMode2D.Impulse);
            }
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                running = true;
                movementSpeed = 10f;
                print("running");
                PlayPlayerAnimation("Player_Run_anim");
            }
            else if (context.canceled)
            {
                running = false;
                print("Stopped running");
                movementSpeed = 7f;
            }
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

                if (jumping)
                {
                    jumping = false;
                }
            }
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            if (!isGrounded || jumping) return;

            if (!IsMoving())
            {
                IdleAnimationHandler();
                return;
            }

            MovementRotationHandler();

            Vector3 movementDirection = new Vector3(movementInput.x, 0, 0);
            movementDirection = movementDirection * movementSpeed * Time.deltaTime;

            playerRigidbody.transform.position += movementDirection;
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

        private void MovementRotationHandler()
        {
            if (movementInput == Vector2.left)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (movementInput == Vector2.right)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            if (!running)
            {
                PlayPlayerAnimation("Player_Walk_anim");
            }
        }

        private void IdleAnimationHandler()
        {
            PlayPlayerAnimation("Player_Idle_Variant_0_anim");
        }

        public void PlayPlayerAnimation(string newAnimationState)
        {
            if (animationState == newAnimationState) return;

            animator.Play(newAnimationState);

            animationState = newAnimationState;
        }
    }



}
