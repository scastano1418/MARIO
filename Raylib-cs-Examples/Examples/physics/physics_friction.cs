/*******************************************************************************************
*
*   Physac - Physics friction
*
*   NOTE 1: Physac requires multi-threading, when InitPhysics() a second thread is created to manage physics calculations.
*   NOTE 2: Physac requires static C library linkage to avoid dependency on MinGW DLL (-static -lpthread)
*
*   Copyright (c) 2016-2018 Victor Fisac
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Physac;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples
{
    public class physics_friction
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "Physac [raylib] - Physics friction");

            // Physac logo drawing position
            int logoX = screenWidth - MeasureText("Physac", 30) - 10;
            int logoY = 15;

            // Initialize physics and default physics bodies
            InitPhysics();

            // Create floor rectangle physics body
            PhysicsBodyData floor = CreatePhysicsBodyRectangle(new Vector2(screenWidth / 2, screenHeight), screenWidth, 100, 10);
            floor.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)
            PhysicsBodyData wall = CreatePhysicsBodyRectangle(new Vector2(screenWidth / 2, screenHeight * 0.8f), 10, 80, 10);
            wall.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)

            // Create left ramp physics body
            PhysicsBodyData rectLeft = CreatePhysicsBodyRectangle(new Vector2(25, screenHeight - 5), 250, 250, 10);
            rectLeft.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)
            SetPhysicsBodyRotation(rectLeft, 30 * DEG2RAD);

            // Create right ramp  physics body
            PhysicsBodyData rectRight = CreatePhysicsBodyRectangle(new Vector2(screenWidth - 25, screenHeight - 5), 250, 250, 10);
            rectRight.enabled = false; // Disable body state to convert it to static (no dynamics, but collisions)
            SetPhysicsBodyRotation(rectRight, 330 * DEG2RAD);

            // Create dynamic physics bodies
            PhysicsBodyData bodyA = CreatePhysicsBodyRectangle(new Vector2(35, screenHeight * 0.6f), 40, 40, 10);
            bodyA.staticFriction = 0.1f;
            bodyA.dynamicFriction = 0.1f;
            SetPhysicsBodyRotation(bodyA, 30 * DEG2RAD);

            PhysicsBodyData bodyB = CreatePhysicsBodyRectangle(new Vector2(screenWidth - 35, screenHeight * 0.6f), 40, 40, 10);
            bodyB.staticFriction = 1;
            bodyB.dynamicFriction = 1;
            SetPhysicsBodyRotation(bodyB, 330 * DEG2RAD);

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KEY_R))    // Reset physics input
                {
                    // Reset dynamic physics bodies position, velocity and rotation
                    bodyA.position = new Vector2(35, screenHeight * 0.6f);
                    bodyA.velocity = new Vector2(0, 0);
                    bodyA.angularVelocity = 0;
                    SetPhysicsBodyRotation(bodyA, 30 * DEG2RAD);

                    bodyB.position = new Vector2(screenWidth - 35, screenHeight * 0.6f);
                    bodyB.velocity = new Vector2(0, 0);
                    bodyB.angularVelocity = 0;
                    SetPhysicsBodyRotation(bodyB, 330 * DEG2RAD);
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(BLACK);

                DrawFPS(screenWidth - 90, screenHeight - 30);

                // Draw created physics bodies
                int bodiesCount = GetPhysicsBodiesCount();
                for (int i = 0; i < bodiesCount; i++)
                {
                    PhysicsBodyData body = GetPhysicsBody(i);

                    if (body.id != 0) // check for null?
                    {
                        int vertexCount = GetPhysicsShapeVerticesCount(i);
                        for (int j = 0; j < vertexCount; j++)
                        {
                            // Get physics bodies shape vertices to draw lines
                            // Note: GetPhysicsShapeVertex() already calculates rotation transformations
                            Vector2 vertexA = GetPhysicsShapeVertex(body, j);

                            int jj = (((j + 1) < vertexCount) ? (j + 1) : 0);   // Get next vertex or first to close the shape
                            Vector2 vertexB = GetPhysicsShapeVertex(body, jj);

                            DrawLineV(vertexA, vertexB, GREEN);     // Draw a line between two vertex positions
                        }
                    }
                }

                DrawRectangle(0, screenHeight - 49, screenWidth, 49, BLACK);

                DrawText("Friction amount", (screenWidth - MeasureText("Friction amount", 30)) / 2, 75, 30, WHITE);
                DrawText("0.1", (int)bodyA.position.x - MeasureText("0.1", 20) / 2, (int)bodyA.position.y - 7, 20, WHITE);
                DrawText("1", (int)bodyB.position.x - MeasureText("1", 20) / 2, (int)bodyB.position.y - 7, 20, WHITE);

                DrawText("Press 'R' to reset example", 10, 10, 10, WHITE);

                DrawText("Physac", logoX, logoY, 30, WHITE);
                DrawText("Powered by", logoX + 50, logoY - 7, 10, WHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            ClosePhysics();       // Unitialize physics

            CloseWindow();        // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}