namespace cowsins
{
    public class PlayerStateFactory
    {
        PlayerStates _context;

        public PlayerStateFactory(PlayerStates currentContext) { _context = currentContext; }

        public PlayerBaseState Default() { return new PlayerDefaultState(_context, this); }

        public PlayerBaseState Jump() { return new PlayerJumpState(_context, this); }

        public PlayerBaseState Crouch() { return new PlayerCrouchState(_context, this); }

        public PlayerBaseState Climb() { return new PlayerClimbState(_context, this); }

    }
}