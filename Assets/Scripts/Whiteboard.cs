using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon;
public class Whiteboard : Photon.PunBehaviour {

	private int textureSize = 2048;
	private int penSize1 = 10;
	private int penSize2 = 10;
	private Texture2D texture;
	private Color[] color;
	private bool touching, touchingLast;
	private float posX, posY;
	private float lastX, lastY;
	// Use this for initialization
	void Start () {
		// Set whiteboard texture
		Color c;
		c = Color.white;
		Renderer renderer = GetComponent<Renderer>();
		this.color = Enumerable.Repeat<Color>(c, penSize1 * penSize2).ToArray<Color>();
		this.texture = new Texture2D(textureSize, textureSize);
		renderer.material.mainTexture = (Texture) texture;
		for(int i = 0; i <= 2047; i++)
		{
			for(int j = 0; j <= 2047; j++)
			{
				texture.SetPixels(i, j, penSize1, penSize2, color);
				
			}
		}
		texture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
		// Transform textureCoords into "pixel" values
		int x = (int) (posX * textureSize - (penSize1 / 2));
		int y = (int) (posY * textureSize - (penSize2 / 2));

		// Only set the pixels if we were touching last frame
		if (touchingLast) {
			// Set base touch pixels
			texture.SetPixels(x, y, penSize1, penSize2, color);

			// Interpolate pixels from previous touch
			for (float t = 0.01f; t < 1.00f; t += 0.01f) {
				int lerpX = (int) Mathf.Lerp (lastX, (float) x, t);
				int lerpY = (int) Mathf.Lerp (lastY, (float) y, t);
				texture.SetPixels (lerpX, lerpY, penSize1, penSize2, color);
			}
		}

		// If currently touching, apply the texture
		if (touching) {
			texture.Apply ();
		}
			
		this.lastX = (float) x;
		this.lastY = (float) y;

		this.touchingLast = this.touching;
	}

	[PunRPC]
	public void ToggleTouch(bool touching) {
		this.touching = touching;
	}

	[PunRPC]
	public void SetTouchPosition(float x, float y) {
		this.posX = x;
		this.posY = y;
	}

	[PunRPC]
	public void SetColor(string colour) {
		Color col;
		if (colour == "blue")
		{
			col = Color.blue;
			this.penSize1 = 10;
		}
		else if (colour == "red")
		{
			col = Color.red;
			this.penSize1 = 10;
		}
		else if (colour == "black")
		{
			col = Color.black;
			this.penSize1 = 10;
		}
		else
		{
			col = Color.white;
			this.penSize1 = 40;
		}
		this.color = Enumerable.Repeat<Color>(col, penSize1 * penSize2).ToArray<Color>();
	}
}
