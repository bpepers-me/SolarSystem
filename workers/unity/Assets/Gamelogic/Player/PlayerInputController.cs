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
        private ShipControls.Writer ShipControlsWriter;

        //private CannonFirer cannonFirer;

        void OnEnable()
        {
            //cannonFirer = GetComponent<CannonFirer>();
        }

        void Update()
        {
            ShipControlsWriter.Send(new ShipControls.Update()
                .SetTargetSpeed(Input.GetAxis("Vertical"))
                .SetTargetSteering(Input.GetAxis("Horizontal")));

            if (Input.GetKeyDown(KeyCode.Q))
            {
                //ShipControlsWriter.Send(new ShipControls.Update().AddFireLeft(new FireLeft()));
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //ShipControlsWriter.Send(new ShipControls.Update().AddFireRight(new FireRight()));
            }

            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                ShipControlsWriter.Send(new ShipControls.Update().AddWarp(new Warp(2)));
            }
        }
    }
}
