using UnityEngine;
using UnityEngine.Events;

using DG.Tweening;

namespace JyotisSugata.Exploration.Interaction
{
    public class PuzzleSuccessAnimator : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("The part of the object to animate (e.g., Chest Lid or Door Hinge)")]
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private ParticleSystem _completionVFX;

        [Header("Rotation Settings")]
        [SerializeField] private bool _animateRotation = true;
        [Tooltip("The rotation applied when opening. For Chest (X: -110). For Door (Y: 90 or -90)")]
        [SerializeField] private Vector3 _targetRotation = new Vector3(0f, 90f, 0f);

        [Header("Position Settings")]
        [SerializeField] private bool _animatePosition = false;
        [Tooltip("Used if the object slides open instead of rotating")]
        [SerializeField] private Vector3 _targetPosition = Vector3.zero;

        [Header("Animation Settings")]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _easeType = Ease.OutBounce;

        private bool _isOpened = false;

        public void PlaySuccessAnimation()
        {
            if (_isOpened) return;
            _isOpened = true;

            if (_targetTransform != null)
            {
                if (_animateRotation)
                {
                    _targetTransform.DOLocalRotate(_targetRotation, _duration).SetEase(_easeType);
                }

                if (_animatePosition)
                {
                    _targetTransform.DOLocalMove(_targetPosition, _duration).SetEase(_easeType);
                }
            }

            // Play VFX
            if (_completionVFX != null)
            {
                _completionVFX.Play();
            }

            // Disable interaction so they can't open it again
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }

            InteractableObject interactable = GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.enabled = false;
                interactable.SetPlayerInRange(false); // Clean up the interaction prompt/idle VFX
            }
        }
    }
}
