
#ifndef __COORDINATES_HLSL__
#define __COORDINATES_HLSL__

// not tau
#ifndef PI
#define PI 3.1415926535897932384626433832795
#endif

#ifndef TWOPI
#define TWOPI 6.28318530718
#endif

// Returns (row, col) on xy,  returns cell internal uv on zw
// Describes a toroidal surface
// That is, the right neighbor of cells-1 cell is cell 0
float2 toroidal_neighbor(float2 cell_coords, float2 neighbor, float2 cells) 
{
	float2 neigh = cell_coords.xy + neighbor;
	neigh = lerp((cells-1) - (neigh % cells), neigh % cells, neigh > 0);

	return neigh;
}

// Returns (row, col) on xy,  returns cell internal uv on zw
float4 cell_coordinates(float2 uv, float2 cells) 
{
	float4 coords = 0.0f;
	float2 stretched = uv * cells;

	coords.xy = trunc(stretched);
	coords.zw = frac(stretched);

	return coords;
}

// input -> [0,1] uv coords and [0, cells-1] cell_coords
// outputs -> offset to cell uv coords. [start of cell, end of cell]
float2 sample_texture_cell(float2 uv, int2 cell_coords, float2 cells) 
{
		return (uv + cell_coords) / cells;
}

int mat2_to_array_index(int2 cell, int line_length) 
{
	return cell.x * line_length + cell.y;
}

int2 array_to_mat2_index(uint array_idx, uint line_length) 
{
	return int2(array_idx / line_length, array_idx % line_length);
}

// returns (x, y)
float2 polar2cart(float2 polar)
{
	float2 cart = 0;
	cart.x = polar.y * cos(polar.x);
	cart.y = polar.y * sin(polar.x);
	return cart;
}

// Returns (theta, r)
float2 cart2polar(float2 cart)
{
	float2 polar = 0;
	polar.y = sqrt(pow(cart.x, 2) + pow(cart.y, 2));

	polar.x = acos(cart.x / polar.y) / PI;


	return polar;
}



#endif 
