using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class SpikeController : MonoBehaviour
    {
        private Rigidbody rigid = null;
        private MeshCollider meshCollider = null;

        private Vector3 originalLocalPos = Vector3.zero;
        private Vector3 originalLocalAngles = Vector3.zero;


        private void OnEnable()
        {
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();
            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>();
            rigid.isKinematic = true;
            meshCollider.enabled = true;
            originalLocalPos = transform.localPosition;
            originalLocalAngles = transform.localEulerAngles;
        }


        private void OnDisable()
        {
            rigid.isKinematic = true;
            meshCollider.enabled = true;
            transform.localPosition = originalLocalPos;
            transform.localEulerAngles = originalLocalAngles;
        }


        /// <summary>
        /// Explode this spike.
        /// </summary>
        public void OnExplode()
        {
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();
            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>();

            rigid.isKinematic = false;
            meshCollider.enabled = false;
            Vector2 forceDir = (transform.position - transform.root.position).normalized;
            rigid.AddForceAtPosition(forceDir * 10f, transform.root.position, ForceMode.Impulse);
            rigid.AddTorque(forceDir * 5f, ForceMode.Impulse);
        }
    }
}
