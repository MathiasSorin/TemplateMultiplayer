using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : Singleton<PaintManager>{

    public Shader texturePaint;
    public Shader extendIslands;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");

    Material paintMaterial;
    Material extendMaterial;

    CommandBuffer command;

    public override void Awake(){
        base.Awake();
        
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void initTextures(Paintable paintable){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        if(paintable.customMaskTexture != null && mask != null){
            Graphics.Blit(paintable.customMaskTexture, mask);
            command.Blit(mask, support);
            command.Blit(mask, extend, extendMaterial);
            Graphics.ExecuteCommandBuffer(command);
            command.Clear();
        }
        else{
            command.SetRenderTarget(mask);
            command.SetRenderTarget(extend);
            command.SetRenderTarget(support);
        }

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }


    public void paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support);
        paintMaterial.SetColor(colorID, color ?? Color.red);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        command.SetRenderTarget(mask);
        command.DrawRenderer(rend, paintMaterial, 0);

        command.SetRenderTarget(support);
        command.Blit(mask, support);

        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
        GetPaintedTexturePercentage(paintable);
    }

    public void clear(Paintable paintable){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();

        command.SetRenderTarget(mask);
        command.ClearRenderTarget(false, true, Color.clear);

        command.SetRenderTarget(extend);
        command.ClearRenderTarget(false, true, Color.clear);

        command.SetRenderTarget(support);
        command.ClearRenderTarget(false, true, Color.clear);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    public float GetPaintedTexturePercentage(Paintable paintable)
    {
        RenderTexture mask = paintable.getMask();
        RenderTexture downscaledMask = RenderTexture.GetTemporary(128, 128, 0, RenderTextureFormat.ARGB32);

        Graphics.Blit(mask, downscaledMask);

        Texture2D temp = new Texture2D(downscaledMask.width, downscaledMask.height, TextureFormat.RGBA32, false);

        RenderTexture.active = downscaledMask;
        temp.ReadPixels(new Rect(0, 0, downscaledMask.width, downscaledMask.height), 0, 0);
        temp.Apply();
        RenderTexture.active = null;

        Color32[] pixels = temp.GetPixels32();
        int paintedPixels = 0;
        for(int i = 0; i < pixels.Length; i++)
        {
            if(pixels[i].a > 0)
            {
                paintedPixels++;
            }
        }
        Debug.Log($"Painted Pixels: {paintedPixels} / {pixels.Length} ({(float)paintedPixels / pixels.Length * 100f}%)");
        return (float)paintedPixels / pixels.Length;
    }

}
