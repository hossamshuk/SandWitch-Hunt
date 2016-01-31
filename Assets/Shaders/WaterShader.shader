Shader "Custom/WaterShaderZ" 
{
	Properties
	{
		_MainTex("Diffuse (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0,1)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 1.0
		_AlphaFactor("Alpha factor", Range(0, 1.0)) = 0.5
		_WaveLength("Wave length", Float) = 0.5
		_WaveHeight("Wave height", Float) = 0.5
		_WaveSpeed("Wave speed", Float) = 1.0
		_RandomHeight("Random height", Float) = 0.5
		_RandomSpeed("Random Speed", Float) = 0.5
		_CollisionWaveLength("Collision wave length", Float) = 2.0

	}

	SubShader
	{
		Tags { "Queue" = "Geometry" "RenderType" = "Opaque" } //"AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
				#pragma target 5.0
				#pragma vertex vert
				#pragma geometry geom
				#pragma fragment frag 
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_fwdbase
				#include "UnityCG.cginc"			
				#include "AutoLight.cginc"

				float rand(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
				}

				float rand2(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
				}

				float _WaveLength;
				float _WaveHeight;
				float _WaveSpeed;
				float _RandomHeight;
				float _RandomSpeed;
				float _CollisionWaveLength;
				vector _CollisionVectors[20];

				uniform float4 _LightColor0;

				uniform float4 _Color;
				uniform float4 _SpecColor;
				uniform float _Shininess;
				uniform float _AlphaFactor;

				sampler2D _CameraDepthTexture; 

				struct v2g
				{
					float4  pos : SV_POSITION;
					float3	norm : NORMAL;
					LIGHTING_COORDS(0, 1)
				};

				struct g2f
				{
					float4 pos : SV_POSITION;
					float3 norm : NORMAL;
					float3 diffuseColor : TEXCOORD2;
					float3 specularColor : TEXCOORD3;
				};

				v2g vert(appdata_full v)
				{
					float3 v0 = mul((float3x3)_Object2World, v.vertex).xyz;

					float collPhase = 0.0;

					for (int i = 0; i < 20; i++)
					{
						float distanceToCenter = length(v0.xz - _CollisionVectors[i].xy);
						float waveHeight = _CollisionVectors[i].z;
						float waveState = _CollisionVectors[i].w;

						if (distanceToCenter < _CollisionWaveLength * 10 * waveState)
						{
							collPhase += (waveHeight * ((1.0 - waveState) * distanceToCenter) / (_CollisionWaveLength * 10 * waveState)) * sin(distanceToCenter - (_CollisionWaveLength * 10 * waveState));
						}
					}

					float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
					float phase0_1 = (_RandomHeight)* sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
					float phase0_2 = (_WaveHeight / 5.0) * sin((_Time[1] * _WaveSpeed * 3.0) + (v0.x * -_WaveLength * 4.0) + (v0.z * _WaveLength * 4.0) + rand2(v0.xzz));

					v0.y += collPhase + phase0 + phase0_1 + phase0_2;

					v.vertex.xyz = mul((float3x3)_World2Object, v0);

					v2g o;
					o.pos = v.vertex;
					o.norm = v.normal;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					return o;
				}

				[maxvertexcount(3)]
				void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream)
				{
					float3 v0 = IN[0].pos.xyz;
					float3 v1 = IN[1].pos.xyz;
					float3 v2 = IN[2].pos.xyz;

					float3 centerPos = (v0 + v1 + v2) / 3.0;

					float3 vn = normalize(cross(v1 - v0, v2 - v0));

					float4x4 modelMatrix = _Object2World;
					float4x4 modelMatrixInverse = _World2Object;

					float3 normalDirection = normalize(
						mul(float4(vn, 0.0), modelMatrixInverse).xyz);
					float3 viewDirection = normalize(_WorldSpaceCameraPos
						- mul(modelMatrix, float4(centerPos, 0.0)).xyz);
					float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
					float attenuation = (LIGHT_ATTENUATION(IN[0]) + LIGHT_ATTENUATION(IN[1]) + LIGHT_ATTENUATION(IN[2])) / 3;

					float3 ambientLighting =
						UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb;

					float3 diffuseReflection =
						attenuation * _LightColor0.rgb * _Color.rgb
						* max(0.0, dot(normalDirection, lightDirection));

					float3 specularReflection;
					if (dot(normalDirection, lightDirection) < 0.0)
					{
						specularReflection = float3(0.0, 0.0, 0.0);
					}
					else
					{
						specularReflection = attenuation * _LightColor0.rgb
							* _SpecColor.rgb * pow(max(0.0, dot(
								reflect(-lightDirection, normalDirection),
								viewDirection)), _Shininess);
					}

					g2f o;
					o.pos = mul(UNITY_MATRIX_MVP, IN[0].pos);
					o.norm = vn;
					o.diffuseColor = ambientLighting + diffuseReflection;
					o.specularColor = specularReflection;
					triStream.Append(o);

					o.pos = mul(UNITY_MATRIX_MVP, IN[1].pos);
					o.norm = vn;
					o.diffuseColor = ambientLighting + diffuseReflection;
					o.specularColor = specularReflection;
					triStream.Append(o);

					o.pos = mul(UNITY_MATRIX_MVP, IN[2].pos);
					o.norm = vn;
					o.diffuseColor = ambientLighting + diffuseReflection;
					o.specularColor = specularReflection;
					triStream.Append(o);

				}

				half4 frag(g2f IN) : COLOR
				{
					float z1 = tex2Dproj(_CameraDepthTexture, IN.pos); 
					z1 = LinearEyeDepth(z1);
					float z2 = (IN.pos.z);
					z1 = saturate(0.125 * (abs(z2 - z1)));
					fixed atten = LIGHT_ATTENUATION(IN);

					return float4(IN.specularColor +
					IN.diffuseColor * atten, saturate(z1*0.1) + _AlphaFactor);
				}
			ENDCG
		}

		Pass
		{
            Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			Fog{ Mode Off }
			ZWrite On ZTest Less Cull Off
            Offset [_ShadowBias],[_ShadowBiasSlope]

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#pragma fragmentoption ARB_precision_hint_fastest
				#include "UnityCG.cginc"

				float rand(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
				}

				float rand2(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
				}

				float _WaveLength;
				float _WaveHeight;
				float _WaveSpeed;
				float _RandomHeight;
				float _RandomSpeed;
				float _CollisionWaveLength;
				vector _CollisionVectors[20];

				struct v2f
				{
					V2F_SHADOW_CASTER;
				};
				
				v2f vert(appdata_base v)
				{
					float3 v0 = mul((float3x3)_Object2World, v.vertex).xyz;

					float collPhase = 0.0;

					for (int i = 0; i < 20; i++)
					{
						float distanceToCenter = length(v0.xz - _CollisionVectors[i].xy);
						float waveHeight = _CollisionVectors[i].z;
						float waveState = _CollisionVectors[i].w;

						if (distanceToCenter < _CollisionWaveLength * 10 * waveState)
						{
							collPhase += (waveHeight * ((1.0 - waveState) * distanceToCenter) / (_CollisionWaveLength * 10 * waveState)) * sin(distanceToCenter - (_CollisionWaveLength * 10 * waveState));
						}
					}

					float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
					float phase0_1 = (_RandomHeight)* sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
					float phase0_2 = (_WaveHeight / 5.0) * sin((_Time[1] * _WaveSpeed * 3.0) + (v0.x * -_WaveLength * 4.0) + (v0.z * _WaveLength * 4.0) + rand2(v0.xzz));

					v0.y += collPhase + phase0 + phase0_1 + phase0_2;

					v.vertex.xyz = mul((float3x3)_World2Object, v0);

					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					return o;
				}
				
				float4 frag(v2f i) : COLOR
				{
					SHADOW_CASTER_FRAGMENT(i)
				}
			ENDCG
		}

		Pass
		{
			Name "ShadowCollector"
			Tags{ "LightMode" = "ShadowCollector" }
		
			Fog{ Mode Off }
			ZWrite On ZTest LEqual
		
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_shadowcollector
				#define SHADOW_COLLECTOR_PASS
				#include "UnityCG.cginc"

				float rand(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
				}

				float rand2(float3 co)
				{
					return frac(sin(dot(co.xyz ,float3(19.9128,75.2,34.5122))) * 12765.5213);
				}

				float _WaveLength;
				float _WaveHeight;
				float _WaveSpeed;
				float _RandomHeight;
				float _RandomSpeed;
				float _CollisionWaveLength;
				vector _CollisionVectors[20];

				struct appdata 
				{
					float4 vertex : POSITION;
				};
			
				struct v2f 
				{
					V2F_SHADOW_COLLECTOR;
				};
			
				v2f vert(appdata v) 
				{
					float3 v0 = mul((float3x3)_Object2World, v.vertex).xyz;

					float collPhase = 0.0;

					for (int i = 0; i < 20; i++)
					{
						float distanceToCenter = length(v0.xz - _CollisionVectors[i].xy);
						float waveHeight = _CollisionVectors[i].z;
						float waveState = _CollisionVectors[i].w;

						if (distanceToCenter < _CollisionWaveLength * 10 * waveState)
						{
							collPhase += (waveHeight * ((1.0 - waveState) * distanceToCenter) / (_CollisionWaveLength * 10 * waveState)) * sin(distanceToCenter - (_CollisionWaveLength * 10 * waveState));
						}
					}

					float phase0 = (_WaveHeight)* sin((_Time[1] * _WaveSpeed) + (v0.x * _WaveLength) + (v0.z * _WaveLength) + rand2(v0.xzz));
					float phase0_1 = (_RandomHeight)* sin(cos(rand(v0.xzz) * _RandomHeight * cos(_Time[1] * _RandomSpeed * sin(rand(v0.xxz)))));
					float phase0_2 = (_WaveHeight / 5.0) * sin((_Time[1] * _WaveSpeed * 3.0) + (v0.x * -_WaveLength * 4.0) + (v0.z * _WaveLength * 4.0) + rand2(v0.xzz));

					v0.y += collPhase + phase0 + phase0_1 + phase0_2;

					v.vertex.xyz = mul((float3x3)_World2Object, v0);

					v2f o;
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					TRANSFER_SHADOW_COLLECTOR(o)
					return o;

				}
			
				fixed4 frag(v2f i) : COLOR
				{
					SHADOW_COLLECTOR_FRAGMENT(i)
				}
			ENDCG
		}
	}
	Fallback "Diffuse"
}