using Assets.Gamelogic.Core;
using Improbable.Ship;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Gamelogic.Player
{
    // Add this MonoBehaviour on client workers only
    [WorkerType(WorkerPlatform.UnityClient)]
    public class PlayerInputController : MonoBehaviour
    {
        /* 
         * Client will only have write-access for their own designated PlayerShip entity's ShipControls component,
         * so this MonoBehaviour will be enabled on the client's designated PlayerShip GameObject only and not on
         * the GameObject of other players' ships.
         */
        [Require]
        private ShipControls.Writer shipControlsWriter;

        void OnEnable()
        {
        }

        void Update()
        {
			uint warpSpeed = 0;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				warpSpeed += 1;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				warpSpeed += 2;
			}
			
            shipControlsWriter.Send(new ShipControls.Update()
                .SetTargetSpeed(Input.GetAxis("Vertical"))
                .SetTargetSteering(Input.GetAxis("Horizontal"))
				.SetWarpSpeed(warpSpeed));

            if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(999)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(0)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(1)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(2)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(3)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(4)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(5)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(6)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(7)));
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
            {
                shipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(8)));
            }
        }
    }
}
