using UnityEngine;

namespace SIS
{
    /// <summary>
    /// This class fades in and out arrows which indicate to the player which direction they should be facing.
    /// </summary>
    public class VRGUIArrows : MonoBehaviour
    {
        [SerializeField] private float m_FadeDuration = 0.5f;       // How long it takes for the arrows to appear and disappear.
        [SerializeField] private float m_ShowAngle = 60f;           // How far from the desired facing direction the player must be facing for the arrows to appear.
        [SerializeField] private Transform m_DesiredDirection;      // Indicates which direction the player should be facing (uses world space forward if null).
        [SerializeField] private Transform m_Camera;                // Reference to the camera to determine which way the player is facing.
        [SerializeField] private Renderer[] m_ArrowRenderers;       // Reference to the renderers of the arrows used to fade them in and out.

        private float m_CurrentAlpha;                               // The alpha the arrows currently have.
        private float m_TargetAlpha;                                // The alpha the arrows are fading towards.
        private float m_FadeSpeed;                                  // How much the alpha should change per second (calculated from the fade duration).
        private const string k_MaterialPropertyName = "_Alpha";     // The name of the alpha property on the shader being used to fade the arrows.


	    private void Start ()
	    {
            // Speed is distance (zero alpha to one alpha) divided by time (duration).
            m_FadeSpeed = 1f / m_FadeDuration;
	    }


        private void Update()
        {
            // The vector in which the player should be facing is the forward direction of the transform specified or world space.
            Vector3 desiredForward = m_DesiredDirection == null ? Vector3.forward : m_DesiredDirection.forward;

            // The forward vector of the camera as it would be on a flat plane.
            Vector3 flatCamForward = Vector3.ProjectOnPlane(m_Camera.forward, Vector3.up).normalized;

            // The difference angle between the desired facing and the current facing of the player.
            float angleDelta = Vector3.Angle (desiredForward, flatCamForward);

            // If the difference is greater than the angle at which the arrows are shown, their target alpha is one otherwise it is zero.
            m_TargetAlpha = angleDelta > m_ShowAngle ? 1f : 0f;

            // Increment the current alpha value towards the now chosen target alpha and the calculated speed.
            m_CurrentAlpha = Mathf.MoveTowards (m_CurrentAlpha, m_TargetAlpha, m_FadeSpeed * Time.deltaTime);

            // Go through all the arrow renderers and set the given property of their material to the current alpha.
            for (int i = 0; i < m_ArrowRenderers.Length; i++)
            {
                m_ArrowRenderers[i].material.SetFloat(k_MaterialPropertyName, m_CurrentAlpha);
            }
        }


        /// <summary>
        /// Turn off the arrows entirely.
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }


        /// <summary>
        /// Turn the arrows on.
        /// </summary>
        public void Show ()
        {
            gameObject.SetActive(true);
        }
    }
}