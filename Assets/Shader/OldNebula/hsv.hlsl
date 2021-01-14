
//acquired from https://stackoverflow.com/questions/3018313/algorithm-to-convert-rgb-to-hsv-and-hsv-to-rgb-in-range-0-255-for-both
#ifndef __HSV_HLSL
#define __HSV_HLSL

float3  HSV2RGB(float3 _HSV)
{
	_HSV.x = fmod(100.0 + _HSV.x, 1.0);                                       // Ensure [0,1[

	float   HueSlice = 6.0 * _HSV.x;                                            // In [0,6[
	float   HueSliceInteger = floor(HueSlice);
	float   HueSliceInterpolant = HueSlice - HueSliceInteger;                   // In [0,1[ for each hue slice

	float3  TempRGB = float3(_HSV.z * (1.0 - _HSV.y),
		_HSV.z * (1.0 - _HSV.y * HueSliceInterpolant),
		_HSV.z * (1.0 - _HSV.y * (1.0 - HueSliceInterpolant)));


	float   IsOddSlice = fmod(HueSliceInteger, 2.0);                          // 0 if even (slices 0, 2, 4), 1 if odd (slices 1, 3, 5)
	float   ThreeSliceSelector = 0.5 * (HueSliceInteger - IsOddSlice);          // (0, 1, 2) corresponding to slices (0, 2, 4) and (1, 3, 5)

	float3  ScrollingRGBForEvenSlices = float3(_HSV.z, TempRGB.zx);           // (V, Temp Blue, Temp Red) for even slices (0, 2, 4)
	float3  ScrollingRGBForOddSlices = float3(TempRGB.y, _HSV.z, TempRGB.x);  // (Temp Green, V, Temp Red) for odd slices (1, 3, 5)
	float3  ScrollingRGB = lerp(ScrollingRGBForEvenSlices, ScrollingRGBForOddSlices, IsOddSlice);

	float   IsNotFirstSlice = saturate(ThreeSliceSelector);                   // 1 if NOT the first slice (true for slices 1 and 2)
	float   IsNotSecondSlice = saturate(ThreeSliceSelector - 1.0);              // 1 if NOT the first or second slice (true only for slice 2)

	return  lerp(ScrollingRGB.xyz, lerp(ScrollingRGB.zxy, ScrollingRGB.yzx, IsNotSecondSlice), IsNotFirstSlice);    // Make the RGB rotate right depending on final slice index
}

float max3_comp(float3 vec) 
{
	return max(vec.x, max(vec.y, vec.z));
}

float min3_comp(float3 vec) 
{
	return min(vec.x, min(vec.y, vec.z));
}

//Not sure this works.
float3 RGB2HSV(float3 _RGB) 
{
	float mx = max3_comp(_RGB);
	float mn = min3_comp(_RGB);
	float mxmn = 1.0 / (mx - mn);

	float h0 = 60 * (4 + (_RGB.x - _RGB.y) / mxmn);
	float h1 = lerp(h0, 60 * (2 + (_RGB.z - _RGB.x) / mxmn), mx == _RGB.y);
	float h2 = lerp(h1, 60 * ((_RGB.y - _RGB.z) / mxmn), mx == _RGB.x);
	float h3 = lerp(h2 , 0, mx == mn & (_RGB.x == _RGB.y & _RGB.y == _RGB.z));

	float H = lerp(h3, h3 + 360, h3 < 0);

	float S = lerp(mxmn / mx, 0, mx == 0);

	float V = mx;

	return float3(H / 360, S, V);

}



#endif

