using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RootCapsule.Control.SceneControl
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Puppet : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void GoToPosition(Vector3 position)
        {
            agent.SetDestination(position);
        }
    }
}
