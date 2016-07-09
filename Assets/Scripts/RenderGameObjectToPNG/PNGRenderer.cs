using UnityEngine;
using System.Collections;

public class PNGRenderer : MonoBehaviour {
    public Camera renderCamera;
    public SpriteRenderer spriteRenderer;

	void Start () {
        RenderTexture renderTexture = new RenderTexture(512, 512, 24);
	    renderTexture.antiAliasing = 4;
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();
        RenderTexture.active = renderTexture;

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
	    Rect rect = new Rect(0, 0, texture.width, texture.height);
        texture.ReadPixels(rect, 0, 0 );
        texture.Apply();

	    //RenderTexture.active = null;
	    //renderCamera.targetTexture = null;

        //Find non transparent rect
	    int left = 0;
	    int top = 0;
	    int width = 0;
	    int height = 0;

        //Left
	    for (int i = 0; i < texture.width; i++) {
	        for (int k = 0; k < texture.height; k++) {
	            if (texture.GetPixel(i, k).a > 0.001f) {
	                left = i;
                    goto endloop1;
	            }
	        }
	    }
        endloop1:

        //Width
        for( int i = texture.width-1; i >= 0; i-- ) {
            for( int k = 0; k < texture.height; k++ ) {
                if( texture.GetPixel( i, k ).a > 0.001f ) {
                    width = i - left;
                    goto endloop2;
                }
            }
        }
        endloop2:

        //Top
        for( int k = 0; k < texture.height; k++ ) {
            for( int i = left; i < left + width; i++ ) {
                if( texture.GetPixel( i, k ).a > 0.001f ) {
                    top = k;
                    goto endloop3;
                }
            }
        }
        endloop3:

        //Height
        for( int k = texture.height - 1; k >= 0; k-- ) {
            for( int i = left; i < left + width; i++ ) {
                if( texture.GetPixel( i, k ).a > 0.001f ) {
                    height = k - top;
                    goto endloop4;
                }
            }
        }
        endloop4:

        Rect r = new Rect(left, top, width, height);
        Debug.Log(r);

        Texture2D trimmedTexture = new Texture2D( width, height);
        trimmedTexture.SetPixels(0,0, width, height, texture.GetPixels(left, top, width, height));
        trimmedTexture.Apply();

        spriteRenderer.sprite = Sprite.Create( trimmedTexture, new Rect(0,0, trimmedTexture.width, trimmedTexture.height ), new Vector2(0.5f, 0.5f));

        byte[] bytes = trimmedTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes( Application.dataPath + "/p.png", bytes );
    }

}
