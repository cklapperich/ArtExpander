using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using HarmonyLib;
using ArtExpander.Patches;
namespace ArtExpander.Core
{
    public class GhostCardAnimatedRenderer : MonoBehaviour
    {
        private Image mainImage;
        private Image maskImage;
        private Image glowImage;
        private Sprite[] frames;
        private Coroutine animationCoroutine;
        private float frameDelay = 0.1f;
        private CardUI parentCardUI;
        private bool wasAnimating = false;

        public void Initialize(Image mainImage, Image maskImage, Image glowImage, Sprite[] frames)
        {
            this.mainImage = mainImage;
            this.maskImage = maskImage;
            this.glowImage = glowImage;  // Add glow mask
            this.frames = frames;
                    
            parentCardUI = GetComponentInParent<CardUI>();
            
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
            var (borderType, isDestiny) = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
        }

        private IEnumerator AnimateSprites()
        {
            int currentFrame = 0;  // Add frame counter
            
            while (true) {
                mainImage.sprite = frames[currentFrame];
                
                // Update all mask layers with the same frame
                maskImage.sprite = frames[currentFrame];
                if (glowImage != null) {
                    glowImage.sprite = frames[currentFrame];
                }
                
                currentFrame = (currentFrame + 1) % frames.Length;
                yield return new WaitForSeconds(frameDelay);
            }
        }

    private void OnDisable()
    {
        var (borderType, isDestiny) = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();
        
        wasAnimating = (animationCoroutine != null);
        
        // Plugin.Logger.LogWarning($"Animation OnDisable. State:" +
        //     $"\nBorder Type: {borderType}" +
        //     $"\nIs Destiny: {isDestiny}" +
        //     $"\nIs Far Culled: {IsFarCulled()}" +
        //     $"\nParent Active: {(parentCardUI != null ? parentCardUI.gameObject.activeInHierarchy : false)}" +
        //     $"\nWas Animating: {wasAnimating}");
        
        StopAnimation();
    }

    private void OnEnable()
    {
        var (borderType, isDestiny) = CardUISetCardPatch.CardDataTracker.GetCurrentCardInfo();
        // Plugin.Logger.LogWarning($"Animation OnEnable. State:" +
        //     $"\nBorder Type: {borderType}" +
        //     $"\nIs Destiny: {isDestiny}" +
        //     $"\nIs Far Culled: {IsFarCulled()}" +
        //     $"\nParent Active: {(parentCardUI != null ? parentCardUI.gameObject.activeInHierarchy : false)}" +
        //     $"\nWas Animating: {wasAnimating}");

        if (wasAnimating)
        {
            StartAnimation();
        }
    }

        private void OnDestroy()
        {
            StopAnimation();
        }

        private bool IsFarCulled()
        {
            if (parentCardUI == null) return false;
            
            var isCulledField = AccessTools.Field(typeof(CardUI), "m_IsFarDistanceCulled");
            if (isCulledField != null)
            {
                return (bool)isCulledField.GetValue(parentCardUI);
            }
            return false;
        }

        // Add this if you need it from the patch
        public void ResetAnimation()
        {
            StopAnimation();
            StartAnimation();
        }
    }
}