using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpriteSheet : MonoBehaviour
{
    public SpriteRenderer playerShadow;
    public Sprite[] spriteSheetSlots; // An array of sprites representing the SpriteSheet slots
    public int index;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteSheetSlots = Resources.LoadAll<Sprite>("FlexSprite/yoga");
        spriteSheetSlots = Resources.LoadAll<Sprite>("FlexSprite/FlexRun_Sprite_Player");
        
        //spriteSheetSlots = new Sprite[];

    }
    public void Start()
    {
        //FlexRunManager.Instance.SetupSprite(spriteSheetSlots.Length);
        SelectSpriteSheetSlot((int)(spriteSheetSlots.Length*0.5f));
    }
    public void SelectSpriteSheetSlot(int index)
    {
        //Debug.Log(index);
        //index -= 1;
        if (index >= 0 && index < spriteSheetSlots.Length)
        {
            spriteRenderer.sprite = spriteSheetSlots[index];
            //if (playerShadow.gameObject.activeSelf)
                playerShadow.sprite = spriteSheetSlots[index];
        }
        else
        {

            Debug.LogError($"{index} Invalid index for SpriteSheet slot!");
        }
    }


}
