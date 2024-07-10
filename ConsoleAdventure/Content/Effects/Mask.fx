#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D Colors; 
SamplerState Sampler;

float2 MaskSize;
float2 TextureSize;
float2 cof;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float2 maskCoord = floor(input.TextureCoordinates * TextureSize / MaskSize) * MaskSize / TextureSize;
    float4 maskColor = Colors.Sample(Sampler, (maskCoord) * cof); // * cof
	
    //float randomValue = frac(sin(dot(rectCoord, float2(12.9898, 78.233))) * 43758.5453);
    //float4 maskColor = float4(randomValue, frac(randomValue * 2.0), frac(randomValue * 3.0), 1.0);

    float4 original = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float3 color = original;
    
    if (original.r > 0 && original.g > 0 && original.b > 0)
    {
        color = maskColor;
    }

    return float4(color, input.Color.a);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
  