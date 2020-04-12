// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PixelBurner/PB_VertexColor"
{
	Properties
	{
		_Soften("Soften", Range( 0 , 1)) = 0.5
		_Range("Range", Range( 0 , 1)) = 0.2
		_BaseColor("BaseColor", Color) = (1,1,1,0)
		_Color1R("Color1(R)", Color) = (1,0.3897059,0.3897059,0)
		_Color2G("Color2(G)", Color) = (0.3897059,1,0.4191684,0)
		_Color3B("Color3(B)", Color) = (0.3897059,0.5959433,1,0)
		_Color4C("Color4(C)", Color) = (0.2635705,0.9191176,0.8648654,0)
		_Color5M("Color5(M)", Color) = (0.9191176,0.2635705,0.9010337,0)
		_Color6Y("Color6(Y)", Color) = (0.9191176,0.8467814,0.2635705,0)
		_EmissiveAmount("EmissiveAmount", Range( 0 , 1)) = 0.25
		_GreyReplacingWeight("GreyReplacingWeight", Range( 0 , 1)) = 1
		_ColorReplacingWeight("ColorReplacingWeight", Range( 0 , 1)) = 1
		_Base_Grey_Texture("Base_Grey_Texture", 2D) = "white" {}
		_Base_Color_Texture("Base_Color_Texture", 2D) = "white" {}
		_Base_Metalness("Base_Metalness", Range( 0 , 1)) = 0
		_Base_Gloss("Base_Gloss", Range( 0 , 1)) = 0
		_Col1R_Metalness("Col1(R)_Metalness", Range( 0 , 1)) = 0
		_Col1R_Gloss("Col1(R)_Gloss", Range( 0 , 1)) = 0
		_Col2G_Metalness("Col2(G)_Metalness", Range( 0 , 1)) = 0
		_Col2G_Gloss("Col2(G)_Gloss", Range( 0 , 1)) = 0
		_Col3B_Metalness("Col3(B)_Metalness", Range( 0 , 1)) = 0
		_Col3B_Gloss("Col3(B)_Gloss", Range( 0 , 1)) = 0
		_Col4C_Metalness("Col4(C)_Metalness", Range( 0 , 1)) = 0
		_Col4C_Gloss("Col4(C)_Gloss", Range( 0 , 1)) = 0
		_Col5M_Metalness("Col5(M)_Metalness", Range( 0 , 1)) = 0
		_Col5M_Gloss("Col5(M)_Gloss", Range( 0 , 1)) = 0
		_Col6Y_Metalness("Col6(Y)_Metalness", Range( 0 , 1)) = 0
		_Col6Y_Gloss("Col6(Y)_Gloss", Range( 0 , 1)) = 0
		_DebugVertColor("DebugVertColor", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Base_Color_Texture;
		uniform float4 _Base_Color_Texture_ST;
		uniform float4 _BaseColor;
		uniform float4 _Color1R;
		uniform float _Range;
		uniform float _Soften;
		uniform float4 _Color2G;
		uniform float4 _Color3B;
		uniform float4 _Color4C;
		uniform float4 _Color5M;
		uniform float4 _Color6Y;
		uniform float _ColorReplacingWeight;
		uniform sampler2D _Base_Grey_Texture;
		uniform float4 _Base_Grey_Texture_ST;
		uniform float _GreyReplacingWeight;
		uniform float _DebugVertColor;
		uniform float _EmissiveAmount;
		uniform float _Base_Metalness;
		uniform float _Col1R_Metalness;
		uniform float _Col2G_Metalness;
		uniform float _Col3B_Metalness;
		uniform float _Col4C_Metalness;
		uniform float _Col5M_Metalness;
		uniform float _Col6Y_Metalness;
		uniform float _Base_Gloss;
		uniform float _Col1R_Gloss;
		uniform float _Col2G_Gloss;
		uniform float _Col3B_Gloss;
		uniform float _Col4C_Gloss;
		uniform float _Col5M_Gloss;
		uniform float _Col6Y_Gloss;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Base_Color_Texture = i.uv_texcoord * _Base_Color_Texture_ST.xy + _Base_Color_Texture_ST.zw;
			float4 lerpResult11 = lerp( _BaseColor , _Color1R , saturate( ( 1.0 - ( ( distance( float3( 1,0,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult15 = lerp( lerpResult11 , _Color2G , saturate( ( 1.0 - ( ( distance( float3( 0,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult17 = lerp( lerpResult15 , _Color3B , saturate( ( 1.0 - ( ( distance( float3( 0,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult20 = lerp( lerpResult17 , _Color4C , saturate( ( 1.0 - ( ( distance( float3( 0,1,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult21 = lerp( lerpResult20 , _Color5M , saturate( ( 1.0 - ( ( distance( float3( 1,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult23 = lerp( lerpResult21 , _Color6Y , saturate( ( 1.0 - ( ( distance( float3( 1,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float4 lerpResult25 = lerp( ( tex2D( _Base_Color_Texture, uv_Base_Color_Texture ) * _BaseColor ) , lerpResult23 , _ColorReplacingWeight);
			float2 uv_Base_Grey_Texture = i.uv_texcoord * _Base_Grey_Texture_ST.xy + _Base_Grey_Texture_ST.zw;
			float4 lerpResult33 = lerp( lerpResult25 , ( tex2D( _Base_Grey_Texture, uv_Base_Grey_Texture ) * lerpResult23 ) , _GreyReplacingWeight);
			float4 lerpResult84 = lerp( lerpResult33 , i.vertexColor , ceil( _DebugVertColor ));
			o.Albedo = lerpResult84.rgb;
			o.Emission = ( lerpResult84 * _EmissiveAmount ).rgb;
			float lerpResult48 = lerp( _Base_Metalness , _Col1R_Metalness , saturate( ( 1.0 - ( ( distance( float3( 1,0,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult55 = lerp( lerpResult48 , _Col2G_Metalness , saturate( ( 1.0 - ( ( distance( float3( 0,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult58 = lerp( lerpResult55 , _Col3B_Metalness , saturate( ( 1.0 - ( ( distance( float3( 0,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult57 = lerp( lerpResult58 , _Col4C_Metalness , saturate( ( 1.0 - ( ( distance( float3( 0,1,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult56 = lerp( lerpResult57 , _Col5M_Metalness , saturate( ( 1.0 - ( ( distance( float3( 1,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult61 = lerp( lerpResult56 , _Col6Y_Metalness , saturate( ( 1.0 - ( ( distance( float3( 1,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			o.Metallic = lerpResult61;
			float lerpResult69 = lerp( _Base_Gloss , _Col1R_Gloss , saturate( ( 1.0 - ( ( distance( float3( 1,0,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult71 = lerp( lerpResult69 , _Col2G_Gloss , saturate( ( 1.0 - ( ( distance( float3( 0,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult74 = lerp( lerpResult71 , _Col3B_Gloss , saturate( ( 1.0 - ( ( distance( float3( 0,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult79 = lerp( lerpResult74 , _Col4C_Gloss , saturate( ( 1.0 - ( ( distance( float3( 0,1,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult82 = lerp( lerpResult79 , _Col5M_Gloss , saturate( ( 1.0 - ( ( distance( float3( 1,0,1 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			float lerpResult83 = lerp( lerpResult82 , _Col6Y_Gloss , saturate( ( 1.0 - ( ( distance( float3( 1,1,0 ) , i.vertexColor.rgb ) - _Range ) / max( _Soften , 1E-05 ) ) ) ));
			o.Smoothness = lerpResult83;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1927;35;1906;1008;7713.714;2022.603;6.246814;True;True
Node;AmplifyShaderEditor.CommentaryNode;45;-4688.317,-339.5842;Inherit;False;2026.48;1429.6;Color Masking;22;13;14;11;16;15;18;19;17;20;22;21;24;30;23;1;2;4;5;6;7;8;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-4869.266,3401.221;Inherit;False;Property;_Soften;Soften;0;0;Create;True;0;0;False;0;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-4866.451,3308.925;Inherit;False;Property;_Range;Range;1;0;Create;True;0;0;False;0;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;-4368.262,94.2173;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;2;-4144.985,189.9175;Inherit;False;Color Mask;-1;;1;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-3915.515,-23.11747;Inherit;False;Property;_BaseColor;BaseColor;2;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;-3909.014,159.7824;Inherit;False;Property;_Color1R;Color1(R);3;0;Create;True;0;0;False;0;1,0.3897059,0.3897059,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;4;-4146.487,329.5354;Inherit;False;Color Mask;-1;;2;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-3647.714,158.8822;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;16;-3906.406,307.0608;Inherit;False;Property;_Color2G;Color2(G);4;0;Create;True;0;0;False;0;0.3897059,1,0.4191684,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-3462.406,289.0609;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;18;-3902.406,455.0608;Inherit;False;Property;_Color3B;Color3(B);5;0;Create;True;0;0;False;0;0.3897059,0.5959433,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;5;-4147.487,474.5354;Inherit;False;Color Mask;-1;;3;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;63;-4453.448,2760.145;Inherit;False;2176.93;1121.508;Metalness;20;83;82;81;80;79;78;77;76;75;74;73;72;71;70;69;68;67;66;65;64;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;62;-4683.265,1310.856;Inherit;False;2176.93;1121.508;Metalness;20;44;39;36;41;40;43;42;47;49;50;51;52;53;54;48;55;58;57;56;61;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;17;-3286.406,464.0608;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;19;-3903.837,597.0159;Inherit;False;Property;_Color4C;Color4(C);6;0;Create;True;0;0;False;0;0.2635705,0.9191176,0.8648654,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;6;-4146.152,613.1238;Inherit;False;Color Mask;-1;;4;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;64;-4208.14,2912.128;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;36;-4363.208,1449.839;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;22;-3898.837,738.0158;Inherit;False;Property;_Color5M;Color5(M);7;0;Create;True;0;0;False;0;0.9191176,0.2635705,0.9010337,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;47;-4093.594,1360.856;Inherit;False;Property;_Base_Metalness;Base_Metalness;14;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;-3145.837,594.0159;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;7;-4147.654,752.7417;Inherit;False;Color Mask;-1;;5;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-3839.681,1638.767;Inherit;False;Property;_Col1R_Metalness;Col1(R)_Metalness;16;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-3863.776,2810.145;Inherit;False;Property;_Base_Gloss;Base_Gloss;15;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-3609.863,3088.056;Inherit;False;Property;_Col1R_Gloss;Col1(R)_Gloss;17;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;39;-4139.931,1545.539;Inherit;False;Color Mask;-1;;7;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;65;-3910.113,2994.828;Inherit;False;Color Mask;-1;;13;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;8;-4148.654,897.7418;Inherit;False;Color Mask;-1;;6;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;-3013.837,732.0158;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;35;-1767.403,-543.0417;Inherit;False;1114.804;576.4246;Coloring;7;31;26;34;32;25;28;33;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;29;-3961.457,-289.5842;Inherit;True;Property;_Base_Color_Texture;Base_Color_Texture;13;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;24;-3896.837,883.0158;Inherit;False;Property;_Color6Y;Color6(Y);8;0;Create;True;0;0;False;0;0.9191176,0.8467814,0.2635705,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;70;-3589.774,3201.412;Inherit;False;Property;_Col2G_Gloss;Col2(G)_Gloss;19;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;69;-3340.95,2972.719;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-3570.768,1523.43;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;40;-4141.433,1685.157;Inherit;False;Color Mask;-1;;8;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;68;-3911.615,3134.446;Inherit;False;Color Mask;-1;;14;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-3819.592,1752.123;Inherit;False;Property;_Col2G_Metalness;Col2(G)_Metalness;18;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;31;-1717.403,-493.0418;Inherit;True;Property;_Base_Grey_Texture;Base_Grey_Texture;12;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1674.034,-188.4562;Inherit;False;Property;_ColorReplacingWeight;ColorReplacingWeight;11;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-2845.837,868.0158;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-3619.969,-183.1654;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;71;-3136.349,3060.792;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;41;-4142.433,1830.157;Inherit;False;Color Mask;-1;;9;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;-3366.167,1611.503;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-3582.334,3304.723;Inherit;False;Property;_Col3B_Gloss;Col3(B)_Gloss;21;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;72;-3912.615,3279.446;Inherit;False;Color Mask;-1;;15;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-3812.152,1855.434;Inherit;False;Property;_Col3B_Metalness;Col3(B)_Metalness;20;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-909.6948,258.9;Inherit;False;Property;_DebugVertColor;DebugVertColor;28;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;74;-2928.465,3169.668;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1289.946,-487.287;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;25;-1306.135,-230.2268;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1352.817,-338.8814;Inherit;False;Property;_GreyReplacingWeight;GreyReplacingWeight;10;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-3796.634,1958.746;Inherit;False;Property;_Col4C_Metalness;Col4(C)_Metalness;22;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-3158.283,1720.379;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-3566.816,3408.035;Inherit;False;Property;_Col4C_Gloss;Col4(C)_Gloss;23;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;76;-3911.28,3418.034;Inherit;False;Color Mask;-1;;16;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;42;-4141.098,1968.745;Inherit;False;Color Mask;-1;;10;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;0,1,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;88;-549.6948,263.9;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;85;-823.6948,60.89996;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;33;-962.1348,-448.7242;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;57;-2987.357,1842.52;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-3549.596,3521.39;Inherit;False;Property;_Col5M_Gloss;Col5(M)_Gloss;25;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;43;-4142.6,2108.364;Inherit;False;Color Mask;-1;;11;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;-2757.539,3291.809;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;77;-3912.782,3557.653;Inherit;False;Color Mask;-1;;17;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,0,1;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-3779.414,2072.101;Inherit;False;Property;_Col5M_Metalness;Col5(M)_Metalness;24;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1164.833,-83.54357;Inherit;False;Property;_EmissiveAmount;EmissiveAmount;9;0;Create;True;0;0;False;0;0.25;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;-259.6948,48.89996;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-3773.675,2191.198;Inherit;False;Property;_Col6Y_Metalness;Col6(Y)_Metalness;26;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;82;-2608.311,3423.819;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;81;-3913.782,3702.654;Inherit;False;Color Mask;-1;;18;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-4143.6,2253.365;Inherit;False;Color Mask;-1;;12;eec747d987850564c95bde0e5a6d1867;0;4;1;FLOAT3;0,0,0;False;3;FLOAT3;1,1,0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;56;-2838.129,1974.53;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-3543.857,3640.487;Inherit;False;Property;_Col6Y_Gloss;Col6(Y)_Gloss;27;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-48.09908,104.4829;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;83;-2460.517,3590.266;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;61;-2664.289,2168.928;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;268.7974,-8.678453;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;PixelBurner/PB_VertexColor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;1;1;0
WireConnection;2;4;9;0
WireConnection;2;5;10;0
WireConnection;4;1;1;0
WireConnection;4;4;9;0
WireConnection;4;5;10;0
WireConnection;11;0;13;0
WireConnection;11;1;14;0
WireConnection;11;2;2;0
WireConnection;15;0;11;0
WireConnection;15;1;16;0
WireConnection;15;2;4;0
WireConnection;5;1;1;0
WireConnection;5;4;9;0
WireConnection;5;5;10;0
WireConnection;17;0;15;0
WireConnection;17;1;18;0
WireConnection;17;2;5;0
WireConnection;6;1;1;0
WireConnection;6;4;9;0
WireConnection;6;5;10;0
WireConnection;20;0;17;0
WireConnection;20;1;19;0
WireConnection;20;2;6;0
WireConnection;7;1;1;0
WireConnection;7;4;9;0
WireConnection;7;5;10;0
WireConnection;39;1;36;0
WireConnection;39;4;9;0
WireConnection;39;5;10;0
WireConnection;65;1;64;0
WireConnection;65;4;9;0
WireConnection;65;5;10;0
WireConnection;8;1;1;0
WireConnection;8;4;9;0
WireConnection;8;5;10;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;21;2;7;0
WireConnection;69;0;67;0
WireConnection;69;1;66;0
WireConnection;69;2;65;0
WireConnection;48;0;47;0
WireConnection;48;1;49;0
WireConnection;48;2;39;0
WireConnection;40;1;36;0
WireConnection;40;4;9;0
WireConnection;40;5;10;0
WireConnection;68;1;64;0
WireConnection;68;4;9;0
WireConnection;68;5;10;0
WireConnection;23;0;21;0
WireConnection;23;1;24;0
WireConnection;23;2;8;0
WireConnection;30;0;29;0
WireConnection;30;1;13;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;71;2;68;0
WireConnection;41;1;36;0
WireConnection;41;4;9;0
WireConnection;41;5;10;0
WireConnection;55;0;48;0
WireConnection;55;1;50;0
WireConnection;55;2;40;0
WireConnection;72;1;64;0
WireConnection;72;4;9;0
WireConnection;72;5;10;0
WireConnection;74;0;71;0
WireConnection;74;1;73;0
WireConnection;74;2;72;0
WireConnection;32;0;31;0
WireConnection;32;1;23;0
WireConnection;25;0;30;0
WireConnection;25;1;23;0
WireConnection;25;2;26;0
WireConnection;58;0;55;0
WireConnection;58;1;51;0
WireConnection;58;2;41;0
WireConnection;76;1;64;0
WireConnection;76;4;9;0
WireConnection;76;5;10;0
WireConnection;42;1;36;0
WireConnection;42;4;9;0
WireConnection;42;5;10;0
WireConnection;88;0;87;0
WireConnection;33;0;25;0
WireConnection;33;1;32;0
WireConnection;33;2;34;0
WireConnection;57;0;58;0
WireConnection;57;1;52;0
WireConnection;57;2;42;0
WireConnection;43;1;36;0
WireConnection;43;4;9;0
WireConnection;43;5;10;0
WireConnection;79;0;74;0
WireConnection;79;1;75;0
WireConnection;79;2;76;0
WireConnection;77;1;64;0
WireConnection;77;4;9;0
WireConnection;77;5;10;0
WireConnection;84;0;33;0
WireConnection;84;1;85;0
WireConnection;84;2;88;0
WireConnection;82;0;79;0
WireConnection;82;1;78;0
WireConnection;82;2;77;0
WireConnection;81;1;64;0
WireConnection;81;4;9;0
WireConnection;81;5;10;0
WireConnection;44;1;36;0
WireConnection;44;4;9;0
WireConnection;44;5;10;0
WireConnection;56;0;57;0
WireConnection;56;1;53;0
WireConnection;56;2;43;0
WireConnection;27;0;84;0
WireConnection;27;1;28;0
WireConnection;83;0;82;0
WireConnection;83;1;80;0
WireConnection;83;2;81;0
WireConnection;61;0;56;0
WireConnection;61;1;54;0
WireConnection;61;2;44;0
WireConnection;0;0;84;0
WireConnection;0;2;27;0
WireConnection;0;3;61;0
WireConnection;0;4;83;0
ASEEND*/
//CHKSM=23728ACD760960D6586DD0125E1704C4F497E4F0