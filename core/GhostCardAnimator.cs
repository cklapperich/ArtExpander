using UnityEngine;
using UnityEngine.UI;
namespace ArtExpander.Core
{
    public class GhostCardAnimatedRenderer : MonoBehaviour
    {
        private Image mainImage;
        private Sprite[] frames;
        private float frameDelay = 0.1f;
        private CardUI parentCardUI;

        // Animation state for Update() approach
        private float frameTimer = 0f;
        private int currentFrame = 0;
        private bool isAnimating = false;

        public void Initialize(Image mainImage, Sprite[] frames, int fps)
        {
            // Clean up other animators first
            var others = GetComponents<GhostCardAnimatedRenderer>();
            if (others.Length > 1)
            {
                //Plugin.Logger.LogWarning($"[Animation {componentId}] Found {others.Length} animators on object, cleaning up extras");
                foreach(var other in others)
                {
                    if (other != this)
                    {
                        other.StopAnimation();
                        Destroy(other);
                    }
                }
            }

            this.mainImage = mainImage;
            this.frames = frames;
            this.frameDelay = 1f / fps;
            this.frameTimer = 0f;
            this.currentFrame = 0;

            parentCardUI = GetComponentInParent<CardUI>();

            // Just enable animation - Update() will handle the rest
            isAnimating = true;
        }

        public void StopAnimation()
        {
            isAnimating = false;
        }

        private void Update()
        {
            // Only animate if we have frames and animation is enabled
            if (!isAnimating || frames == null || frames.Length == 0 || mainImage == null)
                return;

            frameTimer += Time.deltaTime;

            if (frameTimer >= frameDelay)
            {
                // Keep the remainder for precise timing
                frameTimer -= frameDelay;

                // Update sprite
                mainImage.sprite = frames[currentFrame];

                // Advance to next frame
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        private void OnDisable()
        {
            // Animation automatically stops when disabled (Update won't run)
            // We keep isAnimating true so it resumes on enable without resetting
        }

        private void OnEnable()
        {
            // Animation automatically resumes when enabled (Update will run again)
            // Continues from wherever it left off - no reset needed
        }

        private void OnDestroy()
        {
            isAnimating = false;
        }
    }
}