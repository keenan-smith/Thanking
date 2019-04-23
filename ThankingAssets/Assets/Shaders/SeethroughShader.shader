Shader "SeethroughShader" {
	Properties
	{
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	}

		Category
	{
		SubShader
		{
			   Tags { "Queue" = "Overlay+100"
				"RenderType" = "Transparent"}

			Pass
			{
				ZWrite Off
				ZTest Greater
				SetTexture[_MainTex] //{combine texture}
			}

				Pass
			{
				ZTest LEqual
				SetTexture[_MainTex]// {combine texture}
			}

		}
	}

		FallBack "Diffuse"
}