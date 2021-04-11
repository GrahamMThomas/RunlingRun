namespace RunlingRun.Player.Controllers
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(MouseController))]
    public class KeyboardController : MonoBehaviour
    {
        // private MouseController _mouseController;

        // Ability 1
        private const KeyCode _Ability1Key = KeyCode.E;
        public event Func<IEnumerator> UseAbility1;

        // Ability 2
        private const KeyCode _Ability2Key = KeyCode.Q;
        public event Func<IEnumerator> UseAbility2;

        // private void Awake()
        // {
        //     _mouseController = GetComponent<MouseController>();
        // }

        private void Update()
        {
            if (Input.GetKeyDown(_Ability1Key))
            {
                StartCoroutine(UseAbility1());
            }
            if (Input.GetKeyDown(_Ability2Key))
            {
                StartCoroutine(UseAbility2());
            }
        }
    }
}