/*******************************************************************************************
*
*   raylib [shapes] example - draw circle sector (with gui options)
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Vlad Adrian (@demizdor) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2018 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;

namespace Examples
{
    public class shapes_draw_circle_sector
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - draw circle sector");

            Vector2 center = new Vector2((GetScreenWidth() - 300) / 2, GetScreenHeight() / 2);

            float outerRadius = 180.0f;
            int startAngle = 0;
            int endAngle = 180;
            int segments = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                // NOTE: All variables update happens inside GUI control functions
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawLine(500, 0, 500, GetScreenHeight(), Fade(LIGHTGRAY, 0.6f));
                DrawRectangle(500, 0, GetScreenWidth() - 500, GetScreenHeight(), Fade(LIGHTGRAY, 0.3f));

                DrawCircleSector(center, outerRadius, startAngle, endAngle, segments, Fade(MAROON, 0.3f));
                DrawCircleSectorLines(center, outerRadius, startAngle, endAngle, segments, Fade(MAROON, 0.6f));

                // Draw GUI controls
                //------------------------------------------------------------------------------
                /*startAngle = GuiSliderBar(new Rectangle( 600, 40, 120, 20), "StartAngle", startAngle, 0, 720, true );
                endAngle = GuiSliderBar(new Rectangle( 600, 70, 120, 20), "EndAngle", endAngle, 0, 720, true);

                outerRadius = GuiSliderBar(new Rectangle( 600, 140, 120, 20), "Radius", outerRadius, 0, 200, true);
                segments = GuiSliderBar(new Rectangle( 600, 170, 120, 20), "Segments", segments, 0, 100, true);*/
                //------------------------------------------------------------------------------

                // DrawText(string.Format("MODE: %s", (segments >= 4)? "MANUAL" : "AUTO"), 600, 200, 10, (segments >= 4)? MAROON : DARKGRAY);

                DrawFPS(10, 10);

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