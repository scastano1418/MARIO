/*******************************************************************************************
*
*   raylib [text] example - Text Writing Animation
*
*   This example has been created using raylib 1.4 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class text_writing_anim
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - text writing anim");

            string message = "This sample illustrates a text writing\nanimation effect! Check it out! ;)";

            int framesCounter = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyDown(KEY_SPACE)) framesCounter += 8;
                else framesCounter++;

                if (IsKeyPressed(KEY_ENTER)) framesCounter = 0;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawText(message.SubText(0, framesCounter / 10), 210, 160, 20, MAROON);

                DrawText("PRESS [ENTER] to RESTART!", 240, 260, 20, LIGHTGRAY);
                DrawText("PRESS [SPACE] to SPEED UP!", 239, 300, 20, LIGHTGRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}