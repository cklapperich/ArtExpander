using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ArtExpander.Core
{
    public class GhostCardAnimatedRenderer : MonoBehaviour
    {
        private Image mainImage;
        private Image maskImage;
        private Sprite[] frames;
        private Coroutine animationCoroutine;
        private float frameDelay = 0.1f; // 100ms default, can be adjusted

        public void Initialize(Image mainImage, Image maskImage, Sprite[] frames)
        {
            this.mainImage = mainImage;
            this.maskImage = maskImage;
            this.frames = frames;
            
            StopAnimation();
            StartAnimation();
        }

        public void StartAnimation()
        {
            if (frames == null || frames.Length == 0) return;
            if (animationCoroutine != null) return;

            animationCoroutine = StartCoroutine(AnimateSprites());
        }

        public void StopAnimation()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        private IEnumerator AnimateSprites()
        {
            int currentFrame = 0;
            
            while (true)
            {
                if (mainImage != null && maskImage != null && frames != null && frames.Length > 0)
                {
                    mainImage.sprite = frames[currentFrame];
                    maskImage.sprite = frames[currentFrame];
                    currentFrame = (currentFrame + 1) % frames.Length;
                }
                
                yield return new WaitForSeconds(frameDelay);
            }
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void OnDestroy()
        {
            StopAnimation();
        }
    }
}