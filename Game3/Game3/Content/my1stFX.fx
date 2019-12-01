#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler s0;
float4 example = float4(2, 3, 4, 5);

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = (SpriteTexture);
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 PixelShaderFunction(float2 coords: TEXCOORD) : COLOR0
{
	float4 color = tex2D(s0, coords);
	if (!any(color)) return color;
	float step = 1.0 / 7;
	if (coords.x < (step * 1)) color = float4(1, 0, 0, 1);
	else if (coords.x < (step * 2)) color = float4(1, .5, 0, 1);
	else if (coords.x < (step * 3)) color = float4(1, 1, 0, 1);
	else if (coords.x < (step * 4)) color = float4(0, 1, 0, 1);
	else if (coords.x < (step * 5)) color = float4(0, 0, 1, 1);
	else if (coords.x < (step * 6)) color = float4(.3, 0, .8, 1);
	else                            color = float4(1, .8, 1, 1);

	return color;
}

float4 MainPS(float2 coords: TEXCOORD) : COLOR0
{
	//return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	//float4 color = tex2D(s0, input.TextureCoordinates);
	float4 color = tex2D(s0,coords);
	/*float4 coloru = tex2D(s0, float2(coords.x, coords.y - 1));
	float4 colord = tex2D(s0, float2(coords.x, coords.y + 1));
	float4 colorl = tex2D(s0, float2(coords.x - 1, coords.y));
	float4 colorr = tex2D(s0, float2(coords.x + 1, coords.y));*/

	if (!any(color)) return color;//没颜色则返回
	//if (any(coloru) && any(colord) && any(colorl) && any(colorr))  //有颜色
	//{
	//	return color;
	//}
	if (color.a < .9f)
	{
		color = float4(1, 0, 0, 0.8);
	}
	return color;
}

technique SpriteDrawing
{
	pass P0
	{
		//PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};