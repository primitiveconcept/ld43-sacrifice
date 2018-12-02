namespace LetsStartAKittyCult
{
    using UnityEngine;


    public interface INestedSprite
    {
        SpriteRenderer SpriteRenderer { get; set; }
    }


    public static class NestedSpriteExtensions
    {
        public static void NestSprite<T>(this T gameObject)
            where T: MonoBehaviour, INestedSprite
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                return;

            GameObject nestedSpriteObject = new GameObject("SpriteRenderer");
            nestedSpriteObject.transform.SetParent(gameObject.transform);
            nestedSpriteObject.transform.localPosition = Vector3.zero;

            SpriteRenderer nestedSpriteRenderer = nestedSpriteObject.AddComponent<SpriteRenderer>();
            nestedSpriteRenderer.sprite = spriteRenderer.sprite;
            nestedSpriteRenderer.color = spriteRenderer.color;
            nestedSpriteRenderer.flipX = spriteRenderer.flipX;
            nestedSpriteRenderer.flipY = spriteRenderer.flipY;
            nestedSpriteRenderer.material = spriteRenderer.material;
            nestedSpriteRenderer.drawMode = spriteRenderer.drawMode;
            nestedSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
            nestedSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder;
            nestedSpriteRenderer.maskInteraction = spriteRenderer.maskInteraction;
            GameObject.Destroy(spriteRenderer);

            gameObject.SpriteRenderer = nestedSpriteRenderer;
        }
    }
}
