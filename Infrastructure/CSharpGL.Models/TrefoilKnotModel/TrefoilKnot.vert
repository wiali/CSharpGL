﻿#version 150 core

in vec3 inPosition;
in float inTexCoord;

out float passTexCoord;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform mat4 modelMatrix;

void main(void) {
	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(inPosition, 1.0);

	passTexCoord = inTexCoord;
}
