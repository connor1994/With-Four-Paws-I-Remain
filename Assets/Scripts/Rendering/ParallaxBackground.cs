using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WFPIR.Rendering
{
    public class ParallaxBackground : MonoBehaviour
    {
        private float length, startpos;
        [SerializeField] GameObject cam;
        public float parallaxEffect;

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Start()
        {
            startpos = transform.position.x;
        }

        private void FixedUpdate()
        {
            float temp = (cam.transform.position.x * (1 - parallaxEffect));
            float distance = (cam.transform.position.x * parallaxEffect);

            transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }

    }
}