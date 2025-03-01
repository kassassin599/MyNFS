using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
  [SerializeField]
  float scrollSpeed = 5f;

  [SerializeField]
  private Renderer rend;

  private void Update()
  {
    Vector2 currentTextureOffset = rend.material.GetTextureOffset("_MainTex");
    float distanceToScroll = Time.deltaTime * scrollSpeed;
    float newXOffset = currentTextureOffset.x + distanceToScroll;

    Vector2 newOffset = new Vector2(newXOffset, currentTextureOffset.y);
    rend.material.SetTextureOffset("_MainTex", newOffset);
  }
}