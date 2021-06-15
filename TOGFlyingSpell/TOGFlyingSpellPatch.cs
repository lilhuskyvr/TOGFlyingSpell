using System;
using System.Collections;
using Animancer;
using MLSpace;
using TOGModFramework;
using TOGModFramework.Extensions;
using UnityEngine;

namespace TOGFlyingSpell
{
    public class TOGFlyingSpellPatch : MonoBehaviour
    {
        private bool _buttonJumpPressed;

        public void Inject()
        {
            EventManager.OnPlayerLoaded += EventManagerOnOnPlayerLoaded;
        }

        private void EventManagerOnOnPlayerLoaded()
        {
            ConfigManager.local.StartCoroutine(PlayerControlInputListenerCoroutine());
        }

        private IEnumerator PlayerControlInputListenerCoroutine()
        {
            while (Player.local != null)
            {
                var currentButtonJumpPressed = Player.local.controlInput.OrderController.ButtonBPressed;

                if (currentButtonJumpPressed)
                {
                    Player.local.rb.velocity = 15 * Player.local.Head.transform.up;
                }

                //stop pressing trigger
                if (!currentButtonJumpPressed && _buttonJumpPressed)
                {
                    Player.local.rb.velocity = Vector3.zero;
                    Player.local.isJumping = false;
                }

                //start pressing trigger
                if (currentButtonJumpPressed && !_buttonJumpPressed)
                {
                    Player.local.rb.velocity = Vector3.zero;
                    Player.local.isJumping = true;
                }

                if (Player.local.IsFlying())
                {
                    Player.local.rb.useGravity = false;
                }
                else
                {
                    Player.local.rb.useGravity = true;
                }

                _buttonJumpPressed = currentButtonJumpPressed;

                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }
    }
}