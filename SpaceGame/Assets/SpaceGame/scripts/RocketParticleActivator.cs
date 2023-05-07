using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceGame
{
    public class RocketParticleActivator : MonoBehaviour
    {
        [Required] public ParticleSystem FrontLeftRocketParticles;
        [Required] public ParticleSystem FrontRightRocketParticles;
        [Required] public ParticleSystem BackLeftRocketParticles;
        [Required] public ParticleSystem BackRightRocketParticles;

        [Required] public InputRigidbodyMover InputRigidbodyMover;
        [Required] public InputRigidbodyRotater InputRigidbodyRotater;

        private void FixedUpdate()
        {
            updateParticleState(FrontLeftRocketParticles, InputRigidbodyMover.SpeedSign < 0f || InputRigidbodyRotater.AngularSpeedSign > 0f);
            updateParticleState(FrontRightRocketParticles, InputRigidbodyMover.SpeedSign < 0f || InputRigidbodyRotater.AngularSpeedSign < 0f);
            updateParticleState(BackLeftRocketParticles, InputRigidbodyMover.SpeedSign > 0f || InputRigidbodyRotater.AngularSpeedSign < 0f);
            updateParticleState(BackRightRocketParticles, InputRigidbodyMover.SpeedSign > 0f || InputRigidbodyRotater.AngularSpeedSign > 0f);

            static void updateParticleState(ParticleSystem particles, bool state)
            {
                if (state && !particles.isPlaying)
                    particles.Play();
                else if (!state && particles.isPlaying)
                    particles.Stop();
            }
        }
    }
}
