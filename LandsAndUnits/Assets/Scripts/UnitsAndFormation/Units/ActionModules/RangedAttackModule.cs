using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitsAndFormation
{
    public class RangedAttackModule : BehaviourModule
    {
        public float _projectileSpeed;
        public Projectile _projectileDetails;
        public Transform _projectileSpawnPoint;
        uint solutionIndex;

        public override void ModuleSpecificInteraction()
        {
            if (_unitBrain._targetInformation == null) return;
            Vector3 targetPos = _unitBrain._targetInformation.Position() + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.25f, 1.5f), Random.Range(-.5f, .5f));
            Vector3 diff = targetPos - _projectileSpawnPoint.position;
            Vector3 diffGround = new Vector3(diff.x, 0f, diff.z);

            Vector3[] solutions = new Vector3[2];
            int numSolutions;

            numSolutions = fts.solve_ballistic_arc(_projectileSpawnPoint.position, _projectileSpeed, targetPos, Vector3.zero, 9.81f, out solutions[0], out solutions[1]);

            if (numSolutions > 0)
            {
                transform.forward = diffGround;
                var proj = GameObject.Instantiate<GameObject>(_projectileDetails._projectilePrefab);
                ProjectileDetailsReferencer x = proj.AddComponent<ProjectileDetailsReferencer>();
                x.details = _projectileDetails;
                var motion = proj.GetComponent<BallisticMotion>();
                motion.Initialize(_projectileSpawnPoint.position, 9.81f);

                var index = solutionIndex % numSolutions;
                var impulse = solutions[0];
                ++solutionIndex;

                motion.AddImpulse(impulse);
            }
        }

        public override IEnumerator ModuleSpecificInteractionIEnumerator(float reloadTime, float animationTime)
        {
            float elapsedTime = 0;
            _animationController.TriggerRangedAttack();
            while (elapsedTime < animationTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            ModuleSpecificInteraction();
            while (elapsedTime < reloadTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }


            if (_unitBrain._targetInformation != null)
                StartCoroutine(ModuleSpecificInteractionIEnumerator(reloadTime, animationTime));
        }
    }
}

