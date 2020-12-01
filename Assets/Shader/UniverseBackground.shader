Shader "Unlit/UniverseBackground"
{
    Properties
    {
        _PixelCountU("Pixel Count U", float) = 100
        _PixelCountV("Pixel Count V", float) = 100
        _FineNoiseTex("Fine Noise Texture", 2D) = "white" {}
        _CoarseNoiseTex("Coarse Noise Texture", 2D) = "white" {}
        _NoiseSlice ("NoiseSlice", Vector) = (1,1,1,1)
        _StarHarmonics ("Star Harmonics", Range(1, 10)) = 3
        _StarGridSide ("Bright Star Grid Side", Float) = 5
        _StarBright ("Star Brightness", Float) = 0.3
        _StarSat ("Star Saturation", Float) = 0.6
        _TimeScale ("TimeScale", Float) = 1.0
        _BGSat ("BG Saturation", Float) = 0.2
        _BGBright ("BG Brightnes", Float) = 0.2
        _BGIntensity ("BGIntensity", Float) = 0.3
        _SlideIntensity ("SlideIntensity", Float) = 5
        _StarDensity ("Star Density", Float) = 0.2
        _NebulaIntensity ("Nebula Density", Float) = 0.2
        _NebulaSat ("Nebula Saturation", Float) = 0.2
        _NebulaBright ("Nebula Brightness", Float) = 0.2
        _NebulaHarmonics("Nebula Harmonics", Vector) = (2, 5, 0, 0)
        _NebulaHue ("Nebula Hue", Float) = 0.3
        _NebulaRampTex("Nebula Ramp Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "coordinates.hlsl"
            #include "hsv.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _CoarseNoiseTex_ST;
            sampler2D _FineNoiseTex, _CoarseNoiseTex, _NebulaRampTex;
            float2 _NoiseSlice;
            uint _StarHarmonics;
            float _StarGridSide, _NebulaIntensity, _StarDensity, _StarBright, _StarSat, _TimeScale, _BGSat, _BGBright, _BGIntensity, _SlideIntensity;
            float _NebulaSat, _NebulaBright, _NebulaHue;
            float2 _NebulaHarmonics;
            float2 _MousePosition;
            float _PixelCountU;
            float _PixelCountV;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _CoarseNoiseTex);
                return o;
            }


            float2 hash22(float2 xy) {
                return float2(frac(sin(dot(xy, float2(64.432432, 27.564749))) * 514.9456)
                    , frac(sin(dot(xy, float2(631.6554897, 78.16556))) * 897.5898489));
            }

            float hash21(float2 x)
            {
                return frac(sin(dot(x, float2(29.54983, 3.123))) * 54398.13124);
            }

            
            float hash11(float x)
            {
                return frac(sin(x + 445.5234) * 117.523354);
            }

            
            float4 Nebula(float2 uv, float harmonic)
            {
                float2 mouse_offset = (_MousePosition / _ScreenParams.xy) * _SlideIntensity * 5 * harmonic;
                float4 sliding_fine_noise = tex2D(_FineNoiseTex, ((uv + mouse_offset + (_Time.x + harmonic) / 30) / 2 ) * harmonic);

                float r = 1-sliding_fine_noise.r;
                float g = sliding_fine_noise.g;
                float b = sliding_fine_noise.b;

                float4 nebula_color = float4(HSV2RGB(float3(_NebulaHue, _NebulaSat, _NebulaBright)), 1.0);
                //return sliding_fine_noise;
                //float nebula_noise = step(_NoiseSlice.x, sliding_fine_noise.r);
                //nebula_noise = nebula_noise * step(sliding_fine_noise.r, _NoiseSlice.y);

                float nebula_noise = smoothstep(_NoiseSlice.x, _NoiseSlice.y, r * r);

                nebula_noise = (_NoiseSlice.y - nebula_noise) / (_NoiseSlice.y - _NoiseSlice.x);

                float dist_noise = (tex2D(_NebulaRampTex, float2(g * b, 0.5f)) - 0.5f) * 2;
                //float dist_noise = tex2D(_NebulaRampTex, float2(g * b, 0.5f));
                //float dist_noise = g * b;
                
                //nebula_color *= nebula_noise * _NebulaIntensity / dist_noise;
                nebula_color *= nebula_noise * _NebulaIntensity / pow(dist_noise, 2);

                return float4(nebula_color.xyz, nebula_noise);
            }

            float4 BrightStar(float2 uv, float4 noise_col, float harmonic) 
            {
                harmonic *= 0.3f;
                float2 mouse_offset = (_MousePosition / _ScreenParams.xy) * 5* _SlideIntensity * harmonic;
                float2 time_offset = (_Time.y / harmonic) / _TimeScale;
                float2 harmonic_offset = uv * harmonic;

                float4 star_grid_coords = cell_coordinates(uv + mouse_offset + time_offset + harmonic_offset, _StarGridSide * harmonic);


                float4 color = 0.0f;
                float intensity = abs(sin((_Time.x / 10 + star_grid_coords.z / 10)) /2 );

                

                for (int k = -1; k <= 1; k++)
                {
                    for (int l = -1; l <= 1; l++)
                    {
                        float2 offset = float2(k, l);
                        float2 offsetcoords = star_grid_coords.xy +  offset;

                        float3 col = HSV2RGB(float3(hash11(hash21(offsetcoords)), _StarSat, _StarBright));

                        float2 rand_point = hash22(offsetcoords * harmonic);

                        float density = hash21(offsetcoords + 37.0f);

                        float d = distance(star_grid_coords.zw  , rand_point.xy + offset);
                        color += float4(col / (d * d), 1.0) * step(density, _StarDensity);
                    }
                }
                return color * intensity;
            }



            fixed4 frag(v2f i) : SV_Target
            {


                float pixelWidth = 1.0f / _PixelCountU;
                float pixelHeight = 1.0f / _PixelCountV;
                float4 col = float4(0.0f, 0.0f, 0.0f, 0.0f);
                float2 uv = float2((int)(i.uv.x / pixelWidth) * pixelWidth, (int)(i.uv.y / pixelHeight) * pixelHeight);
                // sample the texture
                float4 sliding_coarse_noise = tex2D(_CoarseNoiseTex, (uv + _Time.x / 30) /2);
                float4 fine_noise = tex2D(_FineNoiseTex, uv);
                
                
                
                float4 bg_color = float4( HSV2RGB(float3(sliding_coarse_noise.x, _BGSat, _BGBright)) * sliding_coarse_noise.y * _BGIntensity, 1.0f);
                

                float4 nebula_color = float4(0.0f, 0.0f, 0.0f, 0.0f);
                float nebula_noise = 0.0f;
                for (float j = _NebulaHarmonics.x; j <= _NebulaHarmonics.y; j++)
                {
                    float4 nebula = Nebula(i.uv, j);
                    nebula_color += float4(nebula.xyz, 0.0f) / j ;
                    nebula_noise = max(nebula_noise, nebula.w);
                }


                float4 front_stars = float4(0.0f, 0.0f, 0.0f, 1.0f);
                float4 back_stars = float4(0.0f, 0.0f, 0.0f, 1.0f);

                for (float j = 1; j < _StarHarmonics; j++)
                {
                    front_stars += BrightStar(uv, fine_noise, j) / j;
                }
                back_stars = BrightStar(uv, fine_noise, _StarHarmonics) / _StarHarmonics;

                float4 nebula_backstars = lerp(back_stars, nebula_color, 1);

                col = front_stars + nebula_backstars;


                
                /*half4 col = tex2D(_MainTex, uv);*/

                return bg_color + col;
            }
            ENDCG
        }
    }
}
