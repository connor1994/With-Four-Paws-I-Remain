using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using WFPIR.Utility;
using Random = UnityEngine.Random;

namespace WFPIR.Game
{
    public class PlayerController : MonoBehaviour
    {
        private UtilityHelper utilityHelper;
        private Rigidbody2D playerRigidbody;
        private Animator animator;
        private string animationState;
        private float movementSpeed = 7f;
        private Vector2 movementInput;
        private bool isGrounded;
        private bool jumping;
        private bool holdingJump;
        private bool running;
        private bool isIdle;
        private float jumpForce = 8f;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            utilityHelper = GetComponent<UtilityHelper>();
            playerRigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public void BeginJump(InputAction.CallbackContext context)
        {
            if (!isGrounded || jumping) return;

            CancelIdling();

            if (context.canceled)
            {
                if (!holdingJump)
                {
                    jumping = true;
                    ResetAnimationState();
                    PlayPlayerAnimation("Player_Jump_anim");
                }
                else
                {
                    StopCoroutine(ChargeJump());
                }
            }
            else if (context.performed)
            {
                print("performed");

                jumpForce = 12f;

                StartCoroutine(ChargeJump());
            }
        }

        IEnumerator ChargeJump()
        {
            holdingJump = true;

            ResetAnimationState();
            PlayPlayerAnimation("Player_Jump_anim", 0.1f, true);

            yield return new WaitForSeconds(0.25f);

            print("first charge");

            ResetAnimationState();
            PlayPlayerAnimation("Player_Jump_anim", 0.2f, true);

            yield return new WaitForSeconds(0.25f);

            print("second charge");
            jumping = true;
            ResetAnimationState();
            PlayPlayerAnimation("Player_Jump_anim", 0.3f);
        }

        public void Jump()
        {
            if (transform.eulerAngles.y == 180)
            {
                playerRigidbody.AddForce((Vector2.up + Vector2.left) * jumpForce, ForceMode2D.Impulse);
            }
            else if (transform.eulerAngles.y == 0)
            {
                playerRigidbody.AddForce((Vector2.up + Vector2.right) * jumpForce, ForceMode2D.Impulse);
            }

            ResetJumpForce();
        }

        private void ResetJumpForce()
        {
            jumpForce = 8f;
        }

        public void Run(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                StartRun(true);
            }
            else if (context.canceled)
            {
                StartRun(false);
            }
        }

        private void StartRun(bool startRun)
        {
            if (startRun)
            {
                running = true;
                movementSpeed = 10f;
                jumpForce = 12f;
            }
            else
            {
                running = false;
                movementSpeed = 7f;
                jumpForce = 10f;
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

                    if (holdingJump)
                    {
                        holdingJump = false;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            if (!isGrounded || jumping || holdingJump) return;

            if (IsInputIdle())
            {
                StartRun(false);
                IdleAnimationHandler();
                return;
            }

            isIdle = false;

            ProcessInputRotation();

            Vector3 movementDirection = new Vector3(movementInput.x, 0, 0);
            movementDirection = movementDirection * movementSpeed * Time.deltaTime;

            transform.position += movementDirection;

            AnimationMovementHandler();
        }

        private bool IsInputIdle()
        {
            if (movementInput == Vector2.zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ProcessInputRotation()
        {
            if (movementInput == Vector2.right)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else if (movementInput == Vector2.left)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }

        private void AnimationMovementHandler()
        {
            if (!running)
            {
                PlayPlayerAnimation("Player_Walk_anim");
            }
            else
            {
                PlayPlayerAnimation("Player_Run_anim");
            }
        }

        public void ProcessMovementInput(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        private void IdleAnimationHandler()
        {
            if (!isIdle)
            {
                PlayPlayerAnimation("Player_Idle_Variant_0_anim");
            }
        }

        private void CancelIdling()
        {
            if (isIdle)
            {
                isIdle = false;
            }
        }

        public void PlayPlayerAnimation(string animationToPlay, float atTime = 0, bool shouldPause = false)
        {
            if (animationState == animationToPlay) return;

            animator.Play(animationToPlay, 0, atTime);

            if (shouldPause)
            {
                animator.speed = 0;
            }
            else
            {
                if (animator.speed == 0)
                {
                    animator.speed = 1;
                }
            }

            animationState = animationToPlay;
        }

        private void ResetAnimationState()
        {
            animationState = null;
        }
    }
}
