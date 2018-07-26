using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformRunner
{
    public class GameMain : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch batch;
        SpriteFont font;

        TiledMap testlevel;
        TiledMapRenderer renderer;
        Camera2D camera;
        Spieler spieler;

        int GegnerAnzahl = 25;

        public static Random Zufall = new Random();
        List<Gegner> Gegner;

        KeyboardState oldState;

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.IsFullScreen = false;

            
        }

        protected override void Initialize()
        {
            batch = new SpriteBatch(GraphicsDevice);
            Services.AddService(batch);
            renderer = new TiledMapRenderer(GraphicsDevice);
            camera = new Camera2D(new BoxingViewportAdapter(Window, GraphicsDevice, 1024, 768));
            camera.ZoomOut(0.5f);

            spieler = new Spieler();
            Gegner = new List<PlatformRunner.Gegner>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            testlevel = Content.Load<TiledMap>("maps/testlevel");
            spieler.Animationen.Textur = Content.Load<Texture2D>("Assets/joey");
            spieler.Explosions.Textur = Content.Load<Texture2D>("Assets/explosion");
            spieler.HerzTexture = Content.Load<Texture2D>("Assets/herz");
            font = Content.Load<SpriteFont>("Fonts/Default");


           spieler.Animationen.Play("Stehen");
            spieler.Explosions.Play("Explosion");

            for(int i = 0; i < GegnerAnzahl; i++)
            {
                var zombie = new Zombie();
                zombie.LoadContent(Content);

                Gegner.Add(zombie);
            }
        }
        
        protected override void UnloadContent()
        {

            


        }
        

        protected override void Update(GameTime gameTime)
        {
            var keybd = Keyboard.GetState();
                       

            if (keybd.IsKeyDown(Keys.A))
            {
                spieler.Move(new Vector2(-1f * gameTime.ElapsedGameTime.Milliseconds, 0));
                spieler.Effects = SpriteEffects.FlipHorizontally;
                spieler.Animationen.Play("Laufen");

                spieler.Update(gameTime);
            }
            else if (keybd.IsKeyDown(Keys.D))
            {
                spieler.Move(new Vector2(1f * gameTime.ElapsedGameTime.Milliseconds, 0));
                spieler.Effects = SpriteEffects.None;
                spieler.Animationen.Play("Laufen");

                spieler.Update(gameTime);
            }
            else
                spieler.Animationen.Play("Stehen");

            if (keybd.IsKeyDown(Keys.L))
                spieler.Animationen.Play("Treten");

            if (keybd.IsKeyDown(Keys.W) || keybd.IsKeyDown(Keys.Space))
            {
                spieler.Move(new Vector2(0, -1f * gameTime.ElapsedGameTime.Milliseconds));
                spieler.Animationen.Play("Springen");
            }

            if (keybd.IsKeyDown(Keys.S))
            {
                spieler.Move(new Vector2(0, 1f * gameTime.ElapsedGameTime.Milliseconds));
                spieler.Animationen.Play("Ducken");
            }

            if (keybd.IsKeyDown(Keys.S) && (keybd.IsKeyDown(Keys.D) || keybd.IsKeyDown(Keys.A)))
            {
                spieler.Animationen.Play("Slide");
            }

            if (keybd.IsKeyDown(Keys.K))
                spieler.Animationen.Play("Schlagen");

            if (keybd.IsKeyDown(Keys.J))
                spieler.Animationen.Play("Morden");

            CheckCollision();

            camera.LookAt(spieler.Position);

            spieler.Velocity += new Vector2(0, Spieler.Geschwindigkeit);
            spieler.Position += spieler.Velocity;
            

            foreach (var gegner in Gegner)
                gegner.Update(gameTime);

            
            for (int i = 0; i < Gegner.Count; i++)
            {
                var gegner = Gegner[i];
                if ((gegner.Position - spieler.Position).Length() < 30)
                {
                    spieler.Leben -= 1;
                    gegner.Position += new Vector2(50, gegner.Position.Y);
                }

                if (spieler.Leben <= 0)
                    Exit();

                if ((gegner.Position - spieler.Position).Length() < 100 && oldState.IsKeyDown(Keys.K) && keybd.IsKeyUp(Keys.K))
                    gegner.Leben -= 2;

                if ((gegner.Position - spieler.Position).Length() < 70 && oldState.IsKeyDown(Keys.L) && keybd.IsKeyUp(Keys.L))
                {
                    gegner.Leben -= 3;
                    if(spieler.Effects == SpriteEffects.None)
                    {
                        //gegner.Velocity = new Vector2(1.5f, 0);
                    }
                }

                if (!gegner.IstAmLeben)
                    Gegner.Remove(gegner);
            }


            oldState = keybd;
            base.Update(gameTime);
        }

        private void CheckCollision()
        {
            if (testlevel.ObjectLayers.Count == 0)
                throw new Exception("Level hat keine Kollisionsebene.");

            var collisionLayer = testlevel.GetLayer<TiledMapObjectLayer>("CollisionLayer");
            if (collisionLayer == null)
                throw new Exception("Level hat keinen Layer mit dem Namen CollisionLayer.");

            if (collisionLayer.Objects.Length == 0 || !collisionLayer.Objects.Any(t => t is TiledMapPolylineObject))
                throw new Exception("Keine Polyline Objects...");

            foreach(var obj in collisionLayer.Objects.Where(t => t is TiledMapPolylineObject))
            {
                var pointsObject = (TiledMapPolylineObject)obj;

                for (int i = 0; i < pointsObject.Points.Length - 1; i++)
                {
                    var currentPoint = pointsObject.Points[i] + pointsObject.Position;
                    var nextPoint = pointsObject.Points[i + 1] + pointsObject.Position;

                    if ((spieler.Position.X + (spieler.Animationen.Aktuell.Breite / 2)) >= currentPoint.X && (spieler.Position.X + (spieler.Animationen.Aktuell.Breite / 2)) < nextPoint.X)
                    {
                        float linieY = SolveLinearEquation(currentPoint, nextPoint, (spieler.Position.X + (spieler.Animationen.Aktuell.Breite / 2)));
                        if ((spieler.Position.Y + spieler.Animationen.Aktuell.Höhe) > linieY && (spieler.Position.Y + spieler.Animationen.Aktuell.Höhe) < (linieY + testlevel.TileHeight))
                        {
                            spieler.Position = new Vector2(spieler.Position.X, linieY - spieler.Animationen.Aktuell.Höhe);
                            spieler.Velocity = new Vector2(0, Spieler.Geschwindigkeit);
                        }
                    }

                    foreach(var gegner in Gegner)
                    {
                        if ((gegner.Position.X + (gegner.Animationen.Aktuell.Breite / 2)) >= currentPoint.X && (gegner.Position.X + (gegner.Animationen.Aktuell.Breite / 2)) < nextPoint.X)
                        {
                            float linieY = SolveLinearEquation(currentPoint, nextPoint, (gegner.Position.X + (gegner.Animationen.Aktuell.Breite / 2)));
                            if ((gegner.Position.Y + gegner.Animationen.Aktuell.Höhe) > linieY && (gegner.Position.Y + gegner.Animationen.Aktuell.Höhe) < (linieY + testlevel.TileHeight))
                            {
                                gegner.Position = new Vector2(gegner.Position.X, linieY - gegner.Animationen.Aktuell.Höhe);
                                gegner.Velocity = new Vector2(gegner.Velocity.X, Spieler.Geschwindigkeit);
                            }
                        }
                    }
                }
            }
        }

        private float SolveLinearEquation(Point2 p1, Point2 p2, float x)
        {
            var m = (p1.Y - p2.Y) / (p1.X - p2.X);
            var b = p1.Y - (m * p1.X);
            return m * x + b;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            var groundLayer = testlevel.GetLayer("Ground");
            var viewMatrix = camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1);

            renderer.Draw(groundLayer, ref viewMatrix, ref projectionMatrix);

            batch.Begin(transformMatrix: viewMatrix);
            spieler.Draw(batch, gameTime);

            foreach (var gegner in Gegner)
                gegner.Draw(batch, gameTime);
            batch.End();

            batch.Begin();
            batch.DrawString(font, $"Position: {spieler.Position.ToString()}", Vector2.Zero, Color.White);
            batch.End();

            

            base.Draw(gameTime);
        }
    }
}