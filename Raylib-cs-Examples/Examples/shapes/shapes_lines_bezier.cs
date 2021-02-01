/*******************************************************************************************
*
*   raylib [shapes] example - Cubic-bezier lines
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.MouseButton;

namespace Examples
{
    public class shapes_lines_bezier
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - cubic-bezier lines");

            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(screenWidth, screenHeight);

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsMouseButtonDown(MOUSE_LEFT_BUTTON)) start = GetMousePosition();
                else if (IsMouseButtonDown(MOUSE_RIGHT_BUTTON)) end = GetMousePosition();
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawText("USE MOUSE LEFT-RIGHT CLICK to DEFINE LINE START and END POINTS", 15, 20, 20, GRAY);

                DrawLineBezier(start, end, 2.0f, RED);

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