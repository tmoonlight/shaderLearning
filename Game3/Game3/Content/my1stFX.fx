#include "PPVertexShader.fxh"

#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//Texture2D SpriteTexture;
sampler s0 : register(s0);
float4 haha1;
float4 haha;
float param1;
float2 ttt;
//float4 example = float4(2, 3, 4, 5);

//sampler2D SpriteTextureSampler = sampler_state
//{
//	Texture = (SpriteTexture);
//};

//struct VertexShaderOutput
//{
//	float4 Position : SV_POSITION;
//	float4 Color : COLOR0;
//	//float2 TextureCoordinates : TEXCOORD0;
//};

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

const

float4 MainPS2(float2 coco : DEPTH0, float2 coords: TEXCOORD) : COLOR0
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
	if (color.a < param1)
	{
		color = float4(.9f,.4f,.4f,1);
		//color = haha1;
	}
	return color;
}

float4 MainPS(/*float2 coco : DEPTH0, */float2 coords: TEXCOORD) : COLOR0
{
    if (coords.x > .5f && coords.y > .5f)
        return 0;
	//return tex2D(SpriteTextureSampler,input.TextureCoordinates) * input.Color;
	//float4 color = tex2D(s0, input.TextureCoordinates);
        float4 color = tex2D(s0, coords);
	///*float4 coloru = tex2D(s0, float2(coords.x, coords.y - 1));
	//float4 colord = tex2D(s0, float2(coords.x, coords.y + 1));
	//float4 colorl = tex2D(s0, float2(coords.x - 1, coords.y));
	//float4 colorr = tex2D(s0, float2(coords.x + 1, coords.y));*/

	if (!any(color)) return color;//没颜色则返回
	//if (any(coloru) && any(colord) && any(colorl) && any(colorr))  //有颜色
	////{
	////	return color;
	////}
	if (color.a < param1)
	{
		color = float4(.9f,.4f,.4f,1);
		//color = haha1;
	}
    return color;
}

float4 NoSampPS( /*float2 coco : DEPTH0, */float2 coords : TEXCOORD) : COLOR0
{
    float3 color = float3(1.0, 1.0, 1.0);
    color = distance(coords, 0.3) * color;
    return float4(color,1.0);
}



technique SpriteDrawing
{
	pass P0
	{
        //VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		//PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
        PixelShader = compile PS_SHADERMODEL NoSampPS();
    }
};