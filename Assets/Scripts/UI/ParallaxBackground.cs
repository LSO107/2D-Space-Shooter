using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class ParallaxBackground : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private float speedX = 0.003f;
        [SerializeField]
        private float speedY = 0.003f;

        [SerializeField]
        private RawImage[] backgroundImages;

        private float m_MoveX, m_MoveY;
        private float m_Width, m_Height;

        // Set the width and height for the parallax,
        // multiplied by two so the image isn't squashed
        // into a small space
        //
        private void Start()
        {
            var sprite = background.sprite;
            m_Width = (float)Screen.width / sprite.texture.width * 2;
            m_Height = (float)Screen.height / sprite.texture.height * 2;
        }

        private void Update()
        {
            foreach (var image in backgroundImages)
            {
                ParallaxScroll(image);
            }
        }

        /// <summary>
        /// Parallax scroll based on Time.deltaTme using the
        /// sprite width and height 
        /// </summary>
        private void ParallaxScroll(RawImage rawImage)
        {
            m_MoveX += speedX * Time.deltaTime;
            m_MoveY += speedY * Time.deltaTime;
            rawImage.uvRect = new Rect(m_MoveX, m_MoveY, m_Width, m_Height);
        }
    }
}