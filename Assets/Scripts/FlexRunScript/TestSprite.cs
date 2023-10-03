using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    void Start()
    {
        //// Get the texture from the SpriteRenderer
        //Texture2D texture = (Texture2D)spriteRenderer;

        //// Create a new Sprite[] to store the slices
        //sprites = new Sprite[texture.width * texture.height];

        //// Iterate over the pixels in the texture and create a Sprite for each one
        //for (int x = 0; x < texture.width; x++)
        //{
        //    for (int y = 0; y < texture.height; y++)
        //    {
        //        // Get the pixel color
        //        Color color = texture.GetPixel(x, y);

        //        // Create a new Sprite from the pixel color
        //        Sprite sprite = Sprite.Create(texture, new Rect(x, y, 1, 1),new Vector2(0.5f,0.5f),100);

        //        // Add the sprite to the Sprite[]
        //        sprites[x + y * texture.width] = sprite;
        //    }
        //}
    }
}
