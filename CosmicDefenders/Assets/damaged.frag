uniform sampler2D texture;
uniform float time;
uniform float damageFlash;

void main() {
    vec4 pixel = texture2D(texture, gl_TexCoord[0].xy);

    vec3 baseColor = pixel.rgb;

    vec3 color = baseColor;

    if (damageFlash > 0.0)
    {
        float flicker = (sin(time * 50.0) + 1.0) / 2.0;

        vec3 redFlash = mix(color, vec3(1.0, 0.2, 0.2), damageFlash * flicker);

        color = redFlash;
    }

    gl_FragColor = vec4(color, pixel.a);
}