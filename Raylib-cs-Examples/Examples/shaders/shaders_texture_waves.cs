/*******************************************************************************************
*
*   raylib [shaders] example - Texture Waves
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Anata (@anatagawa) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Anata (@anatagawa) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.ShaderUniformDataType;

namespace Examples
{
    public class shaders_texture_waves
    {
        const int GLSL_VERSION = 330;

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - texture waves");

            // Load texture texture to apply shaders
            Texture2D texture = LoadTexture("resources/space.png");

            // Load shader and setup location points and values
            Shader shader = LoadShader(null, string.Format("resources/shaders/glsl{0}/wave.fs", GLSL_VERSION));

            int secondsLoc = GetShaderLocation(shader, "secondes");
            int freqXLoc = GetShaderLocation(shader, "freqX");
            int freqYLoc = GetShaderLocation(shader, "freqY");
            int ampXLoc = GetShaderLocation(shader, "ampX");
            int ampYLoc = GetShaderLocation(shader, "ampY");
            int speedXLoc = GetShaderLocation(shader, "speedX");
            int speedYLoc = GetShaderLocation(shader, "speedY");

            // Shader uniform values that can be updated at any time
            float freqX = 25.0f;
            float freqY = 25.0f;
            float ampX = 5.0f;
            float ampY = 5.0f;
            float speedX = 8.0f;
            float speedY = 8.0f;

            float[] screenSize = { (float)GetScreenWidth(), (float)GetScreenHeight() };
            Utils.SetShaderValue(shader, GetShaderLocation(shader, "size"), screenSize, UNIFORM_VEC2);
            Utils.SetShaderValue(shader, freqXLoc, freqX, UNIFORM_FLOAT);
            Utils.SetShaderValue(shader, freqYLoc, freqY, UNIFORM_FLOAT);
            Utils.SetShaderValue(shader, ampXLoc, ampX, UNIFORM_FLOAT);
            Utils.SetShaderValue(shader, ampYLoc, ampY, UNIFORM_FLOAT);
            Utils.SetShaderValue(shader, speedXLoc, speedX, UNIFORM_FLOAT);
            Utils.SetShaderValue(shader, speedYLoc, speedY, UNIFORM_FLOAT);

            float seconds = 0.0f;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //-------------------------------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                seconds += GetFrameTime();

                Utils.SetShaderValue(shader, secondsLoc, seconds, UNIFORM_FLOAT);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                BeginShaderMode(shader);

                DrawTexture(texture, 0, 0, WHITE);
                DrawTexture(texture, texture.width, 0, WHITE);

                EndShaderMode();

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);         // Unload shader
            UnloadTexture(texture);       // Unload texture

            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}