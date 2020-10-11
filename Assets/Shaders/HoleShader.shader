Shader "ShaderBooks/HoleShader"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent"
		       "Queue" = "Transparent"
		       "IgnoreProjector" = "True"  }

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            Cull Off
		    Lighting Off
		    ZWrite Off
		    ZTest [unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
            #pragma multi_compile_local HOLE_ONE HOLE_TWO HOLE_THREE HOLE_FOUR

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _HoleTex1;
            float4 _HoleTex1_ST;
            float4 _HoleRect1;
            float4 _HoleUv1;
            
            sampler2D _HoleTex2; 
            float4 _HoleTex2_ST;
            float4 _HoleRect2;
            float4 _HoleUv2;
            
            sampler2D _HoleTex3;
            float4 _HoleTex3_ST;
            float4 _HoleRect3;
            float4 _HoleUv3;
            
            sampler2D _HoleTex4;
            float4 _HoleTex4_ST;
            float4 _HoleRect4;
            float4 _HoleUv4;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }
            
            fixed4 getColor(fixed4 col,v2f i,sampler2D holeTex,float4 holeRect,float4 uv)
            {
                fixed2 wh = (holeRect.zw - holeRect.xy);
                float2 holeUv = (i.uv - holeRect.xy) / wh;
                holeUv = uv.xy + holeUv * (uv.zw - uv.xy);
                fixed flag = UnityGet2DClipping(i.uv, holeRect);
                fixed4 holeCol = tex2D(holeTex,holeUv);
                fixed targetA = holeCol.a;
                holeCol.a = 0;
                holeCol = lerp(col,holeCol,targetA);
                col = col * (1-step(1,flag)); 
                return col + holeCol * flag;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                #ifdef HOLE_ONE
                    fixed4 col = tex2D(_MainTex, i.uv) *i.color;
                    return getColor(col,i,_HoleTex1,_HoleRect1,_HoleUv1);
                #endif 
                
                #ifdef HOLE_TWO
                    fixed4 col = tex2D(_MainTex, i.uv) *i.color;
                    col = getColor(col,i,_HoleTex1,_HoleRect1,_HoleUv1);
                    return getColor(col,i,_HoleTex2,_HoleRect2,_HoleUv2);
                #endif
                
                #ifdef HOLE_THREE
                    fixed4 col = tex2D(_MainTex, i.uv) *i.color;
                    col = getColor(col,i,_HoleTex1,_HoleRect1,_HoleUv1);
                    col = getColor(col,i,_HoleTex2,_HoleRect2,_HoleUv2);
                    return getColor(col,i,_HoleTex3,_HoleRect3,_HoleUv3);
                #endif
                
                #ifdef HOLE_FOUR
                    fixed4 col = tex2D(_MainTex, i.uv) *i.color;
                    col = getColor(col,i,_HoleTex1,_HoleRect1,_HoleUv1);
                    col = getColor(col,i,_HoleTex2,_HoleRect2,_HoleUv2);
                    col = getColor(col,i,_HoleTex3,_HoleRect3,_HoleUv3);
                    return getColor(col,i,_HoleTex4,_HoleRect4,_HoleUv4);
                #endif
            }
            
            
            
            ENDCG
        }
    }
}
