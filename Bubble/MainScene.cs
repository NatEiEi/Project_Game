using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Bubble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Random rnd = new Random();

        public Texture2D _circle, _bubble, _rect;

        public int[,] _bbPos;

        public Color[] allColor;
        //sa
        public MainScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();

            allColor = new Color[6];
            _bbPos = new int[11,10];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _bubble = this.Content.Load<Texture2D>("bubble");
            _circle = this.Content.Load<Texture2D>("circle");
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            data[0] =Color.White;
            _rect.SetData(data);

            allColor[0] = Color.Red;
            allColor[1] = Color.Green;
            allColor[2] = Color.Blue;
            allColor[3] = Color.Yellow;
            allColor[4] = Color.Orange;
            allColor[5] = Color.Purple;

            for(int i = 0; i < 11; i++)
            {
                for(int j = 0; j < 10; j++)
                {
                    _bbPos[i, j] = rnd.Next(allColor.Length);
                    if (i > 6) _bbPos[i, j] = -1;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(_rect, Vector2.Zero, null, Color.DarkGoldenrod, 0f, Vector2.Zero, new Vector2(300 - 15, 900), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_rect, new Vector2(900+15, 0), null, Color.DarkGoldenrod, 0f, Vector2.Zero, new Vector2(300 - 15, 900), SpriteEffects.None, 0f);

            _spriteBatch.Draw(_rect, new Vector2(300-15, 5+(11*60)), null, Color.DarkRed, 0f, Vector2.Zero, new Vector2(630, 2), SpriteEffects.None, 0f);

            for (int i = 0; i < 11; i++) 
            { 
                for(int j = 0; j < 10; j++)
                {
                    if (_bbPos[i,j] != -1)
                    {
                        if(i % 2 == 0)
                        {
                            _spriteBatch.Draw(_circle, new Vector2(300 + (j * 60) -15, 5 + (i * 60)), allColor[_bbPos[i, j]]);
                            _spriteBatch.Draw(_bubble, new Vector2(300 + (j * 60) -15, 5 + (i * 60)), Color.White);
                        }
                        else
                        {
                            _spriteBatch.Draw(_circle, new Vector2(300 + (j * 60) +15, 5 + (i * 60)), allColor[_bbPos[i, j]]);
                            _spriteBatch.Draw(_bubble, new Vector2(300 + (j * 60) +15, 5 + (i * 60)), Color.White);
                        }
                    }
                }
            }



            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }
    }
}