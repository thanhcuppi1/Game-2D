using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game2D
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D m_Rigidbody;
        [SerializeField] private Animator m_Animator;

        //Walk
        [SerializeField] private float m_WalkingSpeed;

        //Jump
        [SerializeField] private LayerMask m_GroundLayerMask;
        [SerializeField] private Transform m_GroundCastPoint;
        [SerializeField] private Vector2 m_GroundCastSize;
        [SerializeField] private float m_JumpForce;

        //Climb
        [SerializeField] private LayerMask m_ClimbableLayerMask;
        [SerializeField] private float m_ClimbSpeed;

        private int m_Direction;
        private bool m_OnGround;
        private int m_AttackHash;
        private int m_DyingHash;
        private int m_IdleHash;
        private int m_WalkingHash;
        private PlayerInputActions m_PlayerInput;
        private Vector2 m_MovementInput;
        private int m_JumpCount;
        private bool m_AttackInput;
        private Collider2D m_Collider2D;

        private void OnEnable()
        {
            if (m_PlayerInput == null)
            {
                m_PlayerInput = new PlayerInputActions();
                m_PlayerInput.Player.Movement.started += OnMovement;
                m_PlayerInput.Player.Movement.performed += OnMovement;
                m_PlayerInput.Player.Movement.canceled += OnMovement;
                m_PlayerInput.Player.Jump.started += OnJump;
                m_PlayerInput.Player.Jump.performed += OnJump;
                m_PlayerInput.Player.Jump.canceled += OnJump;
                m_PlayerInput.Player.Attack.started += OnAttack;
                m_PlayerInput.Player.Attack.performed += OnAttack;
                m_PlayerInput.Player.Attack.canceled += OnAttack;
            }
            m_PlayerInput.Enable();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(m_GroundCastPoint.position, m_GroundCastSize);
        }

        private void OnDisable()
        {
            if (m_PlayerInput != null)
                m_PlayerInput.Disable();
        }

        private void Start()
        {
            TryGetComponent(out m_Collider2D);
            m_AttackHash = Animator.StringToHash("Attack");
            m_IdleHash = Animator.StringToHash("Idle");
            m_WalkingHash = Animator.StringToHash("Walking");
            m_DyingHash = Animator.StringToHash("Dying");
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            m_OnGround = Physics2D.BoxCast(m_GroundCastPoint.position, m_GroundCastSize, 0, Vector3.forward, 0, m_GroundLayerMask);
            if (m_OnGround)
                m_JumpCount = 0;

            CheckMovement();
            CheckClimb();
            CheckAnimations();
        }

        private void CheckMovement()
        {
            if (m_AttackInput)
                return;

            m_Rigidbody.velocity = new Vector2(m_MovementInput.x * m_WalkingSpeed, m_Rigidbody.velocity.y);
        }

        private void CheckClimb()
        {
            if (m_Collider2D.IsTouchingLayers(m_ClimbableLayerMask))
            {
                Vector2 velocity = m_Rigidbody.velocity;
                velocity.y = m_ClimbSpeed * m_MovementInput.y;
                m_Rigidbody.velocity = velocity;
                m_Rigidbody.gravityScale = 0;
            }
            else
                m_Rigidbody.gravityScale = 2f;
        }

        private void CheckAnimations()
        {
            m_Animator.SetBool(m_AttackHash, m_AttackInput);
            m_Animator.SetBool(m_IdleHash, m_Rigidbody.velocity.x == 0 && !m_AttackInput);
            m_Animator.SetBool(m_WalkingHash, m_Rigidbody.velocity.x != 0 && !m_AttackInput);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (m_AttackInput)
                return;

            if (context.started || context.performed)
            {
                if (!m_OnGround && m_JumpCount >= 2)
                    return;

                m_JumpCount++;
                m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_JumpForce);
            }
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
            {
                m_MovementInput = context.ReadValue<Vector2>();
                transform.localScale = new Vector3(m_MovementInput.x >= 0 ? 1 : -1, 1, 1);
            }
            else
                m_MovementInput = Vector2.zero;
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started || context.performed)
                m_AttackInput = true;
            else
                m_AttackInput = false;
        }

        private void PlayAttackSFX()
        {
            AudioManager.Instance.PlaySFX_MeleeSplash();
        }

        [ContextMenu("PlayAttackAnim")]
        private void PlayAttackAnim()
        {
            m_Animator.SetBool(m_AttackHash, true);
            m_Animator.SetBool(m_IdleHash, false);
            m_Animator.SetBool(m_WalkingHash, false);
        }

        [ContextMenu("PlayIdleAnim")]
        private void PlayIdleAnim()
        {
            m_Animator.SetBool(m_IdleHash, true);
            m_Animator.SetBool(m_WalkingHash, false);
            m_Animator.SetBool(m_AttackHash, false);
        }

        [ContextMenu("PlayWalkingAnim")]
        private void PlayWalkingAnim()
        {
            m_Animator.SetBool(m_WalkingHash, true);
            m_Animator.SetBool(m_IdleHash, false);
            m_Animator.SetBool(m_AttackHash, false);
        }

        [ContextMenu("PlayDyingAnim")]
        private void PlayDyingAnim()
        {
            m_Animator.SetBool(m_DyingHash, true);
        }

        [ContextMenu("ResetAnim")]
        private void ResetAnim()
        {
            m_Animator.SetBool(m_IdleHash, true);
            m_Animator.SetBool(m_AttackHash, false);
            m_Animator.SetBool(m_WalkingHash, false);
            m_Animator.SetBool(m_DyingHash, false);
        }
    }
}