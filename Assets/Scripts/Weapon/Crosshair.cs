using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Image CrosshairImage;
    
    void Update()
    {
        
    }

    public void setCrosshair(Sprite crosshairSprite)
    {
        CrosshairImage.sprite = crosshairSprite;
    }
}
