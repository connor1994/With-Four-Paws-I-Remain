using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFPIR.Game
{
    public class PlayerController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            animator = GetComponent<Animator>();
        }
    }
}
