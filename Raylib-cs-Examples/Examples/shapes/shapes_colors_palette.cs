/*******************************************************************************************
*
*   raylib [shapes] example - Colors palette
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014-2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class shapes_colors_palette
    {
        const int MAX_COLORS_COUNT = 21;          // Number of colors available

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - colors palette");

            Color[] colors = new Color[] {
            DARKGRAY, MAROON, ORANGE, DARKGREEN, DARKBLUE, DARKPURPLE, DARKBROWN,
            GRAY, RED, GOLD, LIME, BLUE, VIOLET, BROWN, LIGHTGRAY, PINK, YELLOW,
            GREEN, SKYBLUE, PURPLE, BEIGE };

            string[] colorNames = new string[] {
            "DARKGRAY", "MAROON", "ORANGE", "DARKGREEN", "DARKBLUE", "DARKPURPLE",
            "DARKBROWN", "GRAY", "RED", "GOLD", "LIME", "BLUE", "VIOLET", "BROWN",
            "LIGHTGRAY", "PINK", "YELLOW", "GREEN", "SKYBLUE", "PURPLE", "BEIGE" };

            Rectangle[] colorsRecs = new Rectangle[MAX_COLORS_COUNT];     // Rectangles array

            // Fills colorsRecs data (for every rectangle)
            for (int i = 0; i < MAX_COLORS_COUNT; i++)
            {
                colorsRecs[i].x = 20 + 100 * (i % 7) + 10 * (i % 7);
                colorsRecs[i].y = 80 + 100 * (i / 7) + 10 * (i / 7);
                colorsRecs[i].width = 100;
                colorsRecs[i].height = 100;
            }

            int[] colorState = new int[MAX_COLORS_COUNT];           // Color state: 0-DEFAULT, 1-MOUSE_HOVER

            Vector2 mousePoint = new Vector2(0.0f, 0.0f);

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                mousePoint = GetMousePosition();

                for (int i = 0; i < MAX_COLORS_COUNT; i++)
                {
                    if (CheckCollisionPointRec(mousePoint, colorsRecs[i])) colorState[i] = 1;
                    else colorState[i] = 0;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(RAYWHITE);

                DrawText("raylib colors palette", 28, 42, 20, BLACK);
                DrawText("press SPACE to see all colors", GetScreenWidth() - 180, GetScreenHeight() - 40, 10, GRAY);

                for (int i = 0; i < MAX_COLORS_COUNT; i++)    // Draw all rectangles
                {
                    DrawRectangleRec(colorsRecs[i], Fade(colors[i], colorState[i] != 0 ? 0.6f : 1.0f));

                    if (IsKeyDown(KEY_SPACE) || colorState[i] != 0)
                    {
                        DrawRectangle((int)colorsRecs[i].x, (int)(colorsRecs[i].y + colorsRecs[i].height - 26), (int)colorsRecs[i].width, 20, BLACK);
                        DrawRectangleLinesEx(colorsRecs[i], 6, Fade(BLACK, 0.3f));
                        DrawText(colorNames[i], (int)(colorsRecs[i].x + colorsRecs[i].width - MeasureText(colorNames[i], 10) - 12),
                                (int)(colorsRecs[i].y + colorsRecs[i].height - 20), 10, colors[i]);
                    }
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}