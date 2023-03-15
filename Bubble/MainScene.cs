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

        public Texture2D _circle, _bubble, _rect, _bg;

        public int _bbHeight;

        public MainScene()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = Singleton.ScreenWidth;
            _graphics.PreferredBackBufferHeight = Singleton.ScreenHeight;
            _graphics.ApplyChanges();

            Reset();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _bg = this.Content.Load<Texture2D>("BG_Bubble");
            _bubble = this.Content.Load<Texture2D>("bubble");
            _circle = this.Content.Load<Texture2D>("circle");
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            data[0] =Color.White;
            _rect.SetData(data);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Singleton.Instance._currentKey = Keyboard.GetState();

            if (Singleton.Instance._currentKey.IsKeyDown(Keys.Space) && !Singleton.Instance._currentKey.Equals(Singleton.Instance._previousKey))
            {
                _bbHeight += Singleton.TileSize;
            }

            Singleton.Instance._previousKey = Singleton.Instance._currentKey;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(_bg, Vector2.Zero, Color.White);

            for (int i = 0; i < 11; i++) 
            { 
                for(int j = 0; j < 10; j++)
                {
                    if (Singleton.Instance._bbColor[i,j] != -1)
                    {
                        if(i % 2 == 0)
                        {
                            _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Singleton.Instance.allColor[Singleton.Instance._bbColor[i, j]]);
                            _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                        }
                        else
                        {
                            _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize/2) + (j * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Singleton.Instance.allColor[Singleton.Instance._bbColor[i, j]]);
                            _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize/2) + (j * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                        }
                    }
                }
            }

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            _bbHeight = 5;
        }
    }
}