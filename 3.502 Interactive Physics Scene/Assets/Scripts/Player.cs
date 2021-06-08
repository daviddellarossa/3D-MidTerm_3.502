using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D MousePlayer;

    void OnMouseEnter()
    {
        var hotSpot = new Vector2(MousePlayer.width / (float)2, MousePlayer.height / (float)2);
        Cursor.SetCursor(MousePlayer, hotSpot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
