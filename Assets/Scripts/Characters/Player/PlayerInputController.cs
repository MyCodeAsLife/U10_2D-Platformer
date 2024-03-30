using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerInputController : MonoBehaviour
    {
        public readonly DoubleReactiveProperty<bool, SkillEnum> IsSkillUsed = new();

        public PlayerInputActions UserInputActions;

        private PlayerMovementController _movementController;

        private void Awake()
        {
            UserInputActions = new PlayerInputActions();
            _movementController = GetComponent<PlayerMovementController>();
        }

        private void OnEnable()
        {
            UserInputActions.Enable();
            UserInputActions.Movement.Jump.performed += _movementController.OnEnableJump;
            UserInputActions.Movement.Jump.canceled += _movementController.OnDisableJump;
            UserInputActions.Movement.MoveHorizontal.performed += _movementController.OnEnableHorizontalMovement;
            UserInputActions.Movement.MoveVertical.performed += _movementController.OnEnableVerticalMovement;
            UserInputActions.Movement.MoveHorizontal.canceled += _movementController.OnDisableHorizontalMovement;
            UserInputActions.Movement.MoveVertical.canceled += _movementController.OnDisableVerticalMovement;
            UserInputActions.Movement.Attack.performed += OnAttackEnable;
            UserInputActions.Movement.Attack.canceled += OnAttackDisable;
            UserInputActions.Movement.Vampirism.performed += OnVampirismEnable;
            UserInputActions.Movement.Vampirism.canceled += OnVampirismDisable;
        }

        private void OnDisable()
        {
            UserInputActions.Movement.Jump.performed -= _movementController.OnEnableJump;
            UserInputActions.Movement.Jump.canceled -= _movementController.OnDisableJump;
            UserInputActions.Movement.MoveHorizontal.performed -= _movementController.OnEnableHorizontalMovement;
            UserInputActions.Movement.MoveVertical.performed -= _movementController.OnEnableVerticalMovement;
            UserInputActions.Movement.MoveHorizontal.canceled -= _movementController.OnDisableHorizontalMovement;
            UserInputActions.Movement.MoveVertical.canceled -= _movementController.OnDisableVerticalMovement;
            UserInputActions.Movement.Attack.performed -= OnAttackEnable;
            UserInputActions.Movement.Attack.canceled -= OnAttackDisable;
            UserInputActions.Movement.Vampirism.performed -= OnVampirismEnable;
            UserInputActions.Movement.Vampirism.canceled -= OnVampirismDisable;
            UserInputActions.Disable();
        }

        private void OnAttackEnable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(true, SkillEnum.Slash);
        }

        private void OnAttackDisable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(false, SkillEnum.Slash);
        }

        private void OnVampirismEnable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(true, SkillEnum.Vampirism);
        }

        private void OnVampirismDisable(InputAction.CallbackContext obj)
        {
            IsSkillUsed.SetValues(false, SkillEnum.Vampirism);
        }
    }
}