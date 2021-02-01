/*******************************************************************************************
*
*   raylib [audio] example - Module playing (streaming)
*
*   NOTE: This example requires OpenAL Soft library installed
*
*   This example has been created using raylib 1.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class audio_module_playing
    {
        const int MAX_CIRCLES = 64;

        struct CircleWave
        {
            public Vector2 position;
            public float radius;
            public float alpha;
            public float speed;
            public Color color;
        }

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);      // NOTE: Try to enable MSAA 4X

            InitWindow(screenWidth, screenHeight, "raylib [audio] example - module playing (streaming)");

            InitAudioDevice();              // Initialize audio device

            Color[] colors = new Color[14] { ORANGE, RED, GOLD, LIME, BLUE, VIOLET, BROWN, LIGHTGRAY, PINK,
                             YELLOW, GREEN, SKYBLUE, PURPLE, BEIGE };

            // Creates ome circles for visual effect
            CircleWave[] circles = new CircleWave[MAX_CIRCLES];

            for (int i = MAX_CIRCLES - 1; i >= 0; i--)
            {
                circles[i].alpha = 0.0f;
                circles[i].radius = GetRandomValue(10, 40);
                circles[i].position.X = GetRandomValue((int)circles[i].radius, screenWidth - (int)circles[i].radius);
                circles[i].position.Y = GetRandomValue((int)circles[i].radius, screenHeight - (int)circles[i].radius);
                circles[i].speed = (float)GetRandomValue(1, 100) / 20000.0f;
                circles[i].color = colors[GetRandomValue(0, 13)];
            }

            Music xm = LoadMusicStream("resources/mini1111.xm");

            PlayMusicStream(xm);

            float timePlayed = 0.0f;
            bool pause = false;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateMusicStream(xm);        // Update music buffer with new stream data

                // Restart music playing (stop and play)
                if (IsKeyPressed(KEY_SPACE))
                {
                    StopMusicStream(xm);
                    PlayMusicStream(xm);
                }

                // Pause/Resume music playing
                if (IsKeyPressed(KEY_P))
                {
                    pause = !pause;

                    if (pause) PauseMusicStream(xm);
                    else ResumeMusicStream(xm);
                }

                // Get timePlayed scaled to bar dimensions
                timePlayed = GetMusicTimePlayed(xm) / GetMusicTimeLength(xm) * (screenWidth - 40);

                // Color circles animation
                for (int i = MAX_CIRCLES - 1; (i >= 0) && !pause; i--)
                {
                    circles[i].alpha += circles[i].speed;
                    circles[i].radius += circles[i].speed * 10.0f;

                    if (circles[i].alpha > 1.0f) circles[i].speed *= -1;

                    if (circles[i].alpha <= 0.0f)
                    {
                        circles[i].alpha = 0.0f;
                        circles[i].radius = GetRandomValue(10, 40);
                        circles[i].position.X = GetRandomValue((int)circles[i].radius, screenWidth - (int)circles[i].radius);
                        circles[i].position.Y = GetRandomValue((int)circles[i].radius, screenHeight - (int)circles[i].radius);
                        circles[i].color = colors[GetRandomValue(0, 13)];
                        circles[i].speed = (float)GetRandomValue(1, 100) / 20000.0f;
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                for (int i = MAX_CIRCLES - 1; i >= 0; i--)
                {
                    DrawCircleV(circles[i].position, circles[i].radius, Fade(circles[i].color, circles[i].alpha));
                }

                // Draw time bar
                DrawRectangle(20, screenHeight - 20 - 12, screenWidth - 40, 12, LIGHTGRAY);
                DrawRectangle(20, screenHeight - 20 - 12, (int)timePlayed, 12, MAROON);
                DrawRectangleLines(20, screenHeight - 20 - 12, screenWidth - 40, 12, GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadMusicStream(xm);          // Unload music stream buffers from RAM

            CloseAudioDevice();     // Close audio device (music streaming is automatically stopped)

            CloseWindow();          // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}