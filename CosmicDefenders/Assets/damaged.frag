uniform sampler2D texture;

void main() {
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);
    vec3 red = vec3(1.0, 0.0, 0.0);
    vec3 finalColor = mix(pixel.rgb, red, 0.8);
    gl_FragColor = vec4(finalColor, pixel.a);
}