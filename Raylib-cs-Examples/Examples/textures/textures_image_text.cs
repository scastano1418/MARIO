/*******************************************************************************************
*
*   raylib [texture] example - Image text drawing using TTF generated spritefont
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class textures_image_text
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [texture] example - image text drawing");

            // TTF Font loading with custom generation parameters
            Font font = LoadFontEx("resources/KAISG.ttf", 64, null, 95);

            Image parrots = LoadImage("resources/parrots.png"); // Load image in CPU memory (RAM)

            // Draw over image using custom font
            ImageDrawTextEx(ref parrots, new Vector2(20, 20), font, "[Parrots font drawing]", font.baseSize, 0, WHITE);

            Texture2D texture = LoadTextureFromImage(parrots);  // Image converted to texture, uploaded to GPU memory (VRAM)
            UnloadImage(parrots);   // Once image has been converted to texture and uploaded to VRAM, it can be unloaded from RAM

            Vector2 position = new Vector2(screenWidth / 2 - texture.width / 2, screenHeight / 2 - texture.height / 2 - 20);

            bool showFont = false;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyDown(KEY_SPACE)) showFont = true;
                else showFont = false;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                if (!showFont)
                {
                    // Draw texture with text already drawn inside
                    DrawTextureV(texture, position, WHITE);

                    // Draw text directly using sprite font
                    DrawTextEx(font, "[Parrots font drawing]", new Vector2(position.X + 20,
                               position.Y + 20 + 280), font.baseSize, 0, WHITE);
                }
                else DrawTexture(font.texture, screenWidth / 2 - font.texture.width / 2, 50, BLACK);

                DrawText("PRESS SPACE to SEE USED SPRITEFONT ", 290, 420, 10, DARKGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);     // Texture unloading

            UnloadFont(font);     // Unload custom spritefont

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}