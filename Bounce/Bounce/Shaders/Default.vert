#version 330 core
layout (location = 0) in vec3 aPosition; // vertex coordinates
uniform vec2 uPosition;

void main() 
{
	// We add the uniform's x and y components to the vertex's x and y
	gl_Position = vec4(aPosition.x + uPosition.x, aPosition.y + uPosition.y, aPosition.z, 1.0); // coordinates
}