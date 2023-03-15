using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Bubble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Random rnd = new Random();

        public Texture2D _circle, _bubble, _rect , _Trident;
        SpriteFont _font;
        public int[,] _bbPos;

        public Color[] allColor;

        MouseState _mouseState , _previousMouseState;
        Point TridentPos ;
        
        float _rotateAngle , _moveAngle;
        Vector2 relPoint;
        float _tick;
        //Point _move;
        float _moveSpeed , _PlusX , _PlusY , _moveX , _moveY;
        KeyboardState _currentKey, _previousKey;



        Vector2 _CurrentBubblePos;
        int _CurrentBubble , _NextBubble;
        GameState _currentGameState;
        float CheckX , CheckY;




        enum GameState
        {
            GameStarted,
            GamePaused,
            WaitingForShooting,
            Shoot,
            GameEnded
        }


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
            TridentPos = new Point(600, 880);

            _currentGameState = GameState.WaitingForShooting;
            Reset();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _bubble = this.Content.Load<Texture2D>("bubble");
            _circle = this.Content.Load<Texture2D>("circle");
            _Trident = this.Content.Load<Texture2D>("Trident");
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _font = this.Content.Load<SpriteFont>("GameFont");
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
            _previousMouseState = _mouseState;


            


            switch (_currentGameState)
            {
                case GameState.GameStarted:
                    break;
                case GameState.GamePaused:
                    break;
                case GameState.WaitingForShooting:



                    

                    _mouseState = Mouse.GetState();
                    //Trident Move
                    relPoint = new Vector2((_mouseState.X - TridentPos.X) ,(_mouseState.Y - TridentPos.Y)) ;
                    _rotateAngle = (MathHelper.ToDegrees(MathF.Atan2(relPoint.Y, relPoint.X)) + 450f) % 360f;
                    _rotateAngle = MathHelper.ToRadians(_rotateAngle);



                    if (_mouseState.LeftButton == ButtonState.Pressed &&
                        _previousMouseState.LeftButton == ButtonState.Released)
                    {
                        _moveAngle = _rotateAngle;
                        _currentGameState = GameState.Shoot;

                        _PlusY = 5*(float)Math.Cos((_moveAngle));
                        _PlusX = 5*(float)Math.Sin((_moveAngle));

                    }

                        break;



                case GameState.Shoot:
                    _mouseState = Mouse.GetState();


                    _tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (_tick >= 1 / _moveSpeed)
                    {
                        _tick = 0;
                        
                        _moveY += _PlusY;
                        _moveX += _PlusX;
                    }


                    //Trident Move
                    relPoint = new Vector2((_mouseState.X - TridentPos.X), (_mouseState.Y - TridentPos.Y));
                    _rotateAngle = (MathHelper.ToDegrees(MathF.Atan2(relPoint.Y, relPoint.X)) + 450f) % 360f;
                    _rotateAngle = MathHelper.ToRadians(_rotateAngle);



                    break;
                case GameState.GameEnded:
                    break;

            }
            

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

                            _spriteBatch.Draw(_circle, new Vector2(300 + (j * 60) + 15, 5 + (i * 60)), allColor[_bbPos[i, j]]); ;
                            _spriteBatch.Draw(_bubble, new Vector2(300 + (j * 60) +15, 5 + (i * 60)), Color.White);
                        }
                    }
                }
            }



            //Trident Drawing
            _spriteBatch.Draw(_Trident, new Vector2(TridentPos.X, TridentPos.Y), null, Color.White, _rotateAngle, new Vector2(0 + 80, 300), new Vector2(0.6f, 0.6f), SpriteEffects.None, 0f);

            switch (_currentGameState)
            {
                case GameState.GameStarted:
                    break;
                case GameState.GamePaused:
                    break;
                case GameState.WaitingForShooting:

                    //NewBubble Drawing
                    
                    _spriteBatch.Draw(_circle, new Vector2(TridentPos.X , TridentPos.Y), null, allColor[rnd.Next(allColor.Length)], _rotateAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X, TridentPos.Y), null, Color.White, _rotateAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                   


                    break;
                case GameState.Shoot:
                    //NewBubble Drawing

                    CheckX = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Sin(_moveAngle)) ;
                    CheckY = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Cos(_moveAngle));

                    if (_moveX > 0 && 600 - CheckX - _moveX >= 315 - 30 * (float)Math.Sin(_moveAngle))
                    {
                        _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[rnd.Next(allColor.Length)], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    }
                    else if (_moveX <= 0 && CheckX + _moveX   >= -285 + 30 * (float)Math.Sin(_moveAngle))
                    {
                        _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[rnd.Next(allColor.Length)], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    }

                    else
                    {

                        _PlusX *= -1;
                        _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[rnd.Next(allColor.Length)], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    }



                    if (880 - _moveY - CheckY <= 425 + 30 * (float)Math.Cos(_moveAngle))
                    {
                        _PlusY = 0;
                        _PlusX = 0;
                        _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[rnd.Next(allColor.Length)], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    
                    }









                    break;
                case GameState.GameEnded:
                    break;

            }
            CheckX = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Sin(_rotateAngle)) ;
            float ttt = CheckX - _moveX;

            _spriteBatch.DrawString(_font, "Score : " + MathHelper.ToDegrees(_rotateAngle) + " " + Math.Cos(_rotateAngle) + " " + Math.Sin(_rotateAngle) + "sss       "+(CheckX - _moveX) , new Vector2(10, 40), Color.White);


            //_spriteBatch.DrawString(_font, "Score : " + MathHelper.ToDegrees(_rotateAngle) + " " + ttt + " " + Check, new Vector2(10, 40), Color.White);




            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }


        protected void Reset()
        {
            _moveSpeed = 100f;
        }

        protected float[] naive(int x2, int y2)
        {

            int x1 = 600;
            int y1 = 880;
            float[] Plus = new float[2];
            float dx = x2 - x1;
            float dy = y2 - y1;

            float m = dy / dx;

            if (dx < 0)
            {
                Plus[0] = 1;
            }else Plus[0] = -1;

            Plus[1] = Math.Abs(m);


            return Plus;
        }
    }
}