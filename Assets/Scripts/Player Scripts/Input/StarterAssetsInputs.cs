using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
//using static UnityEngine.Rendering.DebugUI;

#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool shoot;
		public bool aim;
		public bool pause = false;
		public bool reload;
		public bool interact;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

        public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }

        public void OnPause(InputValue value)
        {
            PauseInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
			InteractInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			if (!pause)
			{
				move = newMoveDirection;
			} 
		}

		public void LookInput(Vector2 newLookDirection)
		{
			if (!pause)
			{
				look = newLookDirection;
			}
			else {
				look = new Vector2(0,0);
			}
		}

		public void JumpInput(bool newJumpState)
		{
			if (!pause)
			{
				jump = newJumpState;
			}
		}

		public void SprintInput(bool newSprintState)
		{
			if (!pause)
			{
				sprint = newSprintState;
			}
		}
        public void ShootInput(bool newShootState)
        {
			if (!pause)
			{
                shoot = newShootState;
            }
        }

        public void AimInput(bool newAimState)
        {
            if (!pause)
            {
                aim = newAimState;
            }
        }

        public void PauseInput(bool pauseInput) {
			if (!pause)
			{
				pause = pauseInput;
			}
			else if(pauseInput){
				pause = false;
			}
			if (pause)
			{
				SetCursorState(!cursorLocked);
			}
			else {
                SetCursorState(cursorLocked);
            }
		}

		public void ReloadInput(bool reloadInput) {
			if (!pause)
			{
				reload = reloadInput;
			}
			else {
				reload = false;
			}
		}

        public void InteractInput(bool interactInput)
        {
            if (!pause)
            {
                interact = interactInput;
            }
            else
            {
                interact = false;
            }
        }

        private void OnApplicationFocus(bool hasFocus)
		{
			if (!pause)
			{
				SetCursorState(cursorLocked);
			}
			else 
			{
                SetCursorState(!cursorLocked);
            }
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}