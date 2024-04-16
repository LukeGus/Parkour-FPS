using UnityEngine;
namespace cowsins
{
    public class PlayerDefaultState : PlayerBaseState
    {
        public PlayerDefaultState(PlayerStates currentContext, PlayerStateFactory playerStateFactory)
            : base(currentContext, playerStateFactory) { }

        private PlayerMovement player;

        public override void EnterState()
        {
            player = _ctx.GetComponent<PlayerMovement>();
        }

        public override void UpdateState()
        {
            HandleMovement();
            CheckSwitchState();
            CheckUnCrouch();
        }

        public override void FixedUpdateState() { player.Movement(true); }

        public override void ExitState() { }

        public override void CheckSwitchState()
        {
            // Check climbing
            if (player.DetectLadders()) SwitchState(_factory.Climb());

            // Check Jump
            if (player.ReadyToJump && InputManager.jumping && (player.CanJump && (player.grounded || player.canCoyote) || player.wallRunning || player.jumpCount > 0 && player.maxJumps > 1 && player.CanJump))
                SwitchState(_factory.Jump());

            // Check Crouch
            if (InputManager.crouchingDown && !player.wallRunning && player.allowCrouch)
            {
                if (player.grounded)
                    SwitchState(_factory.Crouch());
                else
                {
                    if (player.allowCrouchWhileJumping) SwitchState(_factory.Crouch());
                }

            }

            // Check Grapple
            if (player.allowGrapple)
            {
                player.HandleGrapple();
                player.UpdateGrappleRenderer();
            }
        }

        public override void InitializeSubState() { }

        void HandleMovement()
        {
            if (InputManager.x != 0 || InputManager.y != 0) player.events.OnMove.Invoke();
            player.Look();
            player.FootSteps();
            player.HandleVelocities();
            player.HandleCoyoteJump();
        }

        private bool canUnCrouch = false;

        private void CheckUnCrouch()
        {

            RaycastHit hitt;
            if (!InputManager.crouching) // Prevent from uncrouching when there�s a roof and we can get hit with it
            {
                if (Physics.Raycast(_ctx.transform.position, _ctx.transform.up, out hitt, 5.5f))
                {
                    canUnCrouch = false;
                }
                else
                    canUnCrouch = true;
            }
            if (canUnCrouch)
            {
                player.events.OnStopCrouch.Invoke(); // Invoke your own method on the moment you are standing up NOT WHILE YOU ARE NOT CROUCHING
                player.StopCrouch();
            }
        }
    }
}