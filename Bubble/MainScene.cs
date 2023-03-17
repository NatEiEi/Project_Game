using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bubble
{
    public class MainScene : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Texture2D _circle, _bubble, _rect, _bg, _trident;
        public SpriteFont _font;

        public int _bbHeight;
        Point _clickPos;

        Point TridentPos;

        float _rotateAngle, _moveAngle;
        Vector2 relPoint;
        float _tick;
        float _moveSpeed, _PlusX, _PlusY, _moveX, _moveY, _falling;

        public KeyboardState _previousKey, _currentKey;
        public MouseState _previousMouse, _currentMouse;

        public Random rnd = new Random();

        public Color[] allColor = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        public int[,] _broad;
        public int[,] _bbColor;

        public int _newbb;
        public int _nextbb;

        enum GameState
        {
            GameStarted,
            GamePaused,
            WaitingForShooting,
            Shoot,
            GameEnded
        }

        Vector2 _CurrentBubblePos;
        int _CurrentBubble, _NextBubble;
        GameState _currentGameState;
        float CheckX, CheckY;

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

            _broad = new int[,] {
                { 0, -4, 1, -4, 2, -4, 3, -4, 1, -4, 2, -4, 3, -4, 2, -4, 3, -4, 3, -4 }
                , { -4, 0, -4, 0, -4, 3, -4, 0, -4, 3, -4, 2, -4, 0, -4, 3, -4, 2, -4, 1 }
                , { 2, -4, 3, -4, 0, -4, -1, -4, 3, -4, 0, -4, 1, -4, 0, -4, 1, -4, 1, -4 }
                , { -4, 2, -4, 2, -4, -1, -4, 2, -4, 1, -4, 0, -4, 2, -4, 1, -4, 0, -4, 3}
                , { 0, -4, 3, -4, -1, -4, 2, -4, 0, -4, 3, -4, 1, -4, 0, -4, 3, -4, 0, -4}
                , { -4, 1, -4, -1, -4, 0, -4, 1, -4, 1, -4, 1, -4, 1, -4, 1, -4, 2, -4, 1}
                , { -1, -4, 0, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4}
                , { -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1}
                , { -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4}
                , { -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1}
                , { -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4, -1, -4} };

            _nextbb = rnd.Next(4);

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
            _trident = this.Content.Load<Texture2D>("Trident");
            _font = this.Content.Load<SpriteFont>("GameFont");
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            data[0] = Color.White;
            _rect.SetData(data);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _currentKey = Keyboard.GetState();

            switch (_currentGameState)
            {
                case GameState.GameStarted:
                    break;
                case GameState.GamePaused:
                    break;
                case GameState.WaitingForShooting:

                    _possibleBlank(_bbColor);

                    _currentMouse = Mouse.GetState();
                    //Trident Move
                    relPoint = new Vector2((_currentMouse.X - TridentPos.X), (_currentMouse.Y - TridentPos.Y));
                    _rotateAngle = (MathHelper.ToDegrees(MathF.Atan2(relPoint.Y, relPoint.X)) + 450f) % 360f;
                    _rotateAngle = MathHelper.ToRadians(_rotateAngle);

                    if (_currentMouse.LeftButton == ButtonState.Pressed &&
                        _previousMouse.LeftButton == ButtonState.Released)
                    {
                        _moveAngle = _rotateAngle;
                        _currentGameState = GameState.Shoot;

                        _PlusY = 5 * (float)Math.Cos((_moveAngle));
                        _PlusX = 5 * (float)Math.Sin((_moveAngle));

                    }

                    if (_currentMouse.RightButton == ButtonState.Pressed &&
                        _previousMouse.RightButton == ButtonState.Released)
                    {
                        _newbb += _nextbb;
                        _nextbb = _newbb - _nextbb;
                        _newbb -= _nextbb;
                    }

                    if (_currentKey.IsKeyDown(Keys.Space) && !_currentKey.Equals(_previousKey))
                    {
                        _bbHeight += Singleton.TileSize;
                    }

                    break;
                case GameState.Shoot:

                    _possibleBlank(_bbColor);

                    _currentMouse = Mouse.GetState();

                    _tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (_tick >= 1 / _moveSpeed)
                    {
                        _tick = 0;

                        _moveY += _PlusY;
                        _moveX += _PlusX;

                        if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Count > 0)
                        {
                            _falling += 10;

                            
                        }
                    }

                    //Trident Move
                    relPoint = new Vector2((_currentMouse.X - TridentPos.X), (_currentMouse.Y - TridentPos.Y));
                    _rotateAngle = (MathHelper.ToDegrees(MathF.Atan2(relPoint.Y, relPoint.X)) + 450f) % 360f;
                    _rotateAngle = MathHelper.ToRadians(_rotateAngle);

                    if (_currentKey.IsKeyDown(Keys.P) && !_currentKey.Equals(_previousKey))
                    {
                        popped(_bbColor, 2, 0);
                    }

                    if (_currentKey.IsKeyDown(Keys.R) && !_currentKey.Equals(_previousKey))
                    {
                        Reset();
                    }
                    if (IsAddedBubble(600 - CheckX - _moveX))
                    {
                        _allBlankReset();
                        _currentGameState = GameState.WaitingForShooting;
                        _moveX = 0;
                        _moveY = 0;
                        _moveAngle = 0;
                        _newbb = _nextbb;
                        _nextbb = rnd.Next(4);

                        if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Count > 0)
                        {

                            if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).First<Point>().X * 60 + _falling >= 900)
                            {
                                
                                fallingDown();
                            }
                        }
                        
                    }

                    break;
                case GameState.GameEnded:
                    break;

            }

            _previousMouse = _currentMouse;
            _previousKey = _currentKey;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(_bg, Vector2.Zero, Color.White);

            _spriteBatch.Draw(_rect, new Vector2(285, 450), Color.Violet);

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Find(new Point(i, j)) == null)
                    {
                        if (_bbColor[i, j] > -1)
                        {
                            if (i % 2 == 0)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                        else if (_bbColor[i, j] == -2)
                        {
                            if (i % 2 == 0)
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                        else if (_bbColor[i, j] == -1)
                        {
                            if (i % 2 == 0)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                    }
                }
            }


            foreach (Point p in findBBcanFall(_bbColor, _whereNoTag(_bbColor)))
            {
                if (p.X % 2 == 0)
                {
                    _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (p.Y / 2 * Singleton.TileSize), _falling + (p.X * Singleton.TileSize)), Color.Black);
                    _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (p.Y / 2 * Singleton.TileSize), _falling + (p.X * Singleton.TileSize)), Color.LightCyan);
                }
                else
                {
                    _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (p.Y / 2 * Singleton.TileSize), _falling + (p.X * Singleton.TileSize)), Color.Black);
                    _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (p.Y / 2 * Singleton.TileSize), _falling + (p.X * Singleton.TileSize)), Color.LightCyan);
                }
            }

            //Trident Drawing
            _spriteBatch.Draw(_trident, new Vector2(TridentPos.X, TridentPos.Y), null, Color.White, _rotateAngle, new Vector2(0 + 80, 300), new Vector2(0.6f, 0.6f), SpriteEffects.None, 0f);

            switch (_currentGameState)
            {
                case GameState.GameStarted:
                    break;
                case GameState.GamePaused:
                    break;
                case GameState.WaitingForShooting:

                    //NewBubble Drawing

                    _spriteBatch.Draw(_circle, new Vector2(TridentPos.X, TridentPos.Y), null, allColor[_newbb], _rotateAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X, TridentPos.Y), null, Color.White, _rotateAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);

                    _spriteBatch.Draw(_circle, new Vector2(570, 850), allColor[_nextbb]);
                    _spriteBatch.Draw(_bubble, new Vector2(570, 850), Color.White);

                    break;
                case GameState.Shoot:
                    
                    //หาค่าก่อนที่จะ rotate ตัว bubble  
                    CheckX = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Sin(_moveAngle));
                    CheckY = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Cos(_moveAngle));
                    //เช็คว่าไม่ออกนอกขอบ ถ้าชนขอบก็จะกลับด้าน
                    if (!(_moveX > 0 && 600 - CheckX - _moveX >= 315 - 30 * (float)Math.Sin(_moveAngle))
                        && !(_moveX <= 0 && CheckX + _moveX >= -285 + 30 * (float)Math.Sin(_moveAngle)))
                    {
                        _PlusX *= -1;
                    }
                    //วาด bubble ที่จะยิง
                    _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[_newbb], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);

                    //IsAddedBubble(600 - CheckX - _moveX);
                    

                    _spriteBatch.Draw(_circle, new Vector2(570, 850), allColor[_nextbb]);
                    _spriteBatch.Draw(_bubble, new Vector2(570, 850), Color.White);

                    break;
                case GameState.GameEnded:
                    break;

            }

            CheckX = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Sin(_rotateAngle));
            float ttt = CheckX - _moveX;

            _spriteBatch.DrawString(_font, "At : " + MathHelper.ToDegrees(_rotateAngle) + " " + Math.Cos(_rotateAngle) + " " + Math.Sin(_rotateAngle) + "sss       " + (CheckX - _moveX), new Vector2(10, 40), Color.White);


            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }

        protected void Reset()
        {
            _bbHeight = 5;
            TridentPos = new Point(600, 880);

            _currentGameState = GameState.WaitingForShooting;

            _moveSpeed = 100f;
            _falling = _bbHeight;
            _moveX = 0;
            _moveY = 0;
            CheckX = 0;
            CheckY = 0;
            _bbColor = new int[11, 20];

            _bbColor = (int[,])_broad.Clone();

            _newbb = _nextbb;
            _nextbb = rnd.Next(4);

        }

        protected void _allBlankReset()
        {
            for (int i = 1; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (_bbColor[i, j] < -1)
                        {
                            _bbColor[i, j] = -1;
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (_bbColor[i, j] < -1)
                        {
                            _bbColor[i, j] = -1;
                        }
                    }
                }
            }
        }

        protected LinkedList<Point> _whereNoTag(int[,] b)
        {
            LinkedList<Point> list = new LinkedList<Point>();

            for (int i = 1; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (b[i, j] > -1)
                        {
                            if (j == 0)
                            {
                                if (topnear(i, j, b, -2) > 0 && downnear(i, j, b, -2) > 0 && rightnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else if (j > 0 && j < 18)
                            {
                                if (topnear(i, j, b, -2) == 2 && downnear(i, j, b, -2) > 0 && (leftnear(i, j, b, -2) > 0 || rightnear(i, j, b, -2) > 0))
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else
                            {
                                if (topnear(i, j, b, -2) == 2 && downnear(i, j, b, -2) > 0 && leftnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (b[i, j] > -1)
                        {
                            if (j == 1)
                            {
                                if (topnear(i, j, b, -2) == 2 && downnear(i, j, b, -2) > 0 && rightnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else if (j > 1 && j < 19)
                            {
                                if (topnear(i, j, b, -2) == 2 && downnear(i, j, b, -2) > 0 && (leftnear(i, j, b, -2) > 0 || rightnear(i, j, b, -2) > 0))
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else
                            {
                                if (topnear(i, j, b, -2) > 0 && downnear(i, j, b, -2) > 0 && leftnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }

        protected void _possibleBlank(int[,] b)
        {
            LinkedList<Point> list = new LinkedList<Point>();

            for (int i = 0; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (b[i, j] == -1)
                        {
                            int c = near(i, j, b);
                            if (c > 0)
                            {
                                list.AddLast(new Point(i, j));
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (b[i, j] == -1)
                        {
                            int c = near(i, j, b);
                            if (c > 0)
                            {
                                list.AddLast(new Point(i, j));
                            }
                        }
                    }
                }
            }

            foreach (Point p in list)
            {
                b[p.X, p.Y] = -2;
            }
        }

        protected int near(int m, int n, int[,] b)
        {
            int cnt = 0;

            if (m > 0 && n > 0 && b[m - 1, n - 1] > -1)
            {
                cnt++;
            }

            if (m > 0 && n < 19 && b[m - 1, n + 1] > -1)
            {
                cnt++;
            }

            if (m < 10 && n > 0 && b[m + 1, n - 1] > -1)
            {
                cnt++;
            }

            if (m < 10 && n < 19 && b[m + 1, n + 1] > -1)
            {
                cnt++;
            }

            if (n > 1 && b[m, n - 2] > -1)
            {
                cnt++;
            }

            if (n < 18 && b[m, n + 2] > -1)
            {
                cnt++;
            }

            return cnt;
        }

        protected int topnear(int m, int n, int[,] b, int traget)
        {
            int cnt = 0;

            if (m > 0 && n > 0 && b[m - 1, n - 1] == traget)
            {
                cnt++;
            }

            if (m > 0 && n < 19 && b[m - 1, n + 1] == traget)
            {
                cnt++;
            }

            return cnt;
        }

        protected int downnear(int m, int n, int[,] b, int traget)
        {
            int cnt = 0;

            if (m < 10 && n > 0 && b[m + 1, n - 1] == traget)
            {
                cnt++;
            }

            if (m < 10 && n < 19 && b[m + 1, n + 1] == traget)
            {
                cnt++;
            }

            return cnt;
        }

        protected int leftnear(int m, int n, int[,] b, int traget)
        {
            int cnt = 0;
            if (n > 1 && b[m, n - 2] == traget)
            {
                cnt++;
            }

            return cnt;
        }

        protected int rightnear(int m, int n, int[,] b, int traget)
        {
            int cnt = 0;

            if (n < 18 && b[m, n + 2] == traget)
            {
                cnt++;
            }

            return cnt;
        }

        protected LinkedList<Point> findBBcanFall(int[,] b, LinkedList<Point> l)
        {
            LinkedList<Point> p = new LinkedList<Point>();

            foreach (Point p1 in l)
            {
                Queue<Point> q = new Queue<Point>();
                int target = -1;

                q.Enqueue(new Point(p1.X, p1.Y));

                do
                {
                    Point point = q.Peek();

                    //left
                    if (point.Y > 1 && b[point.X, point.Y - 2] > target && p.Find(new Point(point.X, point.Y - 2)) == null)
                    {
                        q.Enqueue(new Point(point.X, point.Y - 2));
                    }

                    //right
                    if (point.Y < 18 && b[point.X, point.Y + 2] > target && p.Find(new Point(point.X, point.Y + 2)) == null)
                    {
                        q.Enqueue(new Point(point.X, point.Y + 2));
                    }

                    //up
                    //left
                    if (point.X > 0 && point.Y > 0 && b[point.X - 1, point.Y - 1] > target && p.Find(new Point(point.X - 1, point.Y - 1)) == null)
                    {
                        q.Enqueue(new Point(point.X - 1, point.Y - 1));
                    }

                    //right
                    if (point.X > 0 && point.Y < 19 && b[point.X - 1, point.Y + 1] > target && p.Find(new Point(point.X - 1, point.Y + 1)) == null)
                    {
                        q.Enqueue(new Point(point.X - 1, point.Y + 1));
                    }

                    //down
                    //left
                    if (point.X < 10 && point.Y > 0 && b[point.X + 1, point.Y - 1] > target && p.Find(new Point(point.X + 1, point.Y - 1)) == null)
                    {
                        q.Enqueue(new Point(point.X + 1, point.Y - 1));
                    }

                    //right
                    if (point.X < 10 && point.Y < 19 && b[point.X + 1, point.Y + 1] > target && p.Find(new Point(point.X + 1, point.Y + 1)) == null)
                    {
                        q.Enqueue(new Point(point.X + 1, point.Y + 1));
                    }

                    p.AddLast(point);
                    q.Dequeue();


                } while (q.Count > 0);
            }

            return p;
        }

        protected void popped(int[,] b, int x, int y)
        {
            Queue<Point> q = new Queue<Point>();
            LinkedList<Point> p = new LinkedList<Point>();
            int target = b[x, y];

            q.Enqueue(new Point(x, y));

            do
            {
                Point point = q.Peek();

                //left
                if (point.Y > 1 && b[point.X, point.Y - 2] == target && p.Find(new Point(point.X, point.Y - 2)) == null)
                {
                    q.Enqueue(new Point(point.X, point.Y - 2));
                }

                //right
                if (point.Y < 18 && b[point.X, point.Y + 2] == target && p.Find(new Point(point.X, point.Y + 2)) == null)
                {
                    q.Enqueue(new Point(point.X, point.Y + 2));
                }

                //up
                //left
                if (point.X > 0 && point.Y > 0 && b[point.X - 1, point.Y - 1] == target && p.Find(new Point(point.X - 1, point.Y - 1)) == null)
                {
                    q.Enqueue(new Point(point.X - 1, point.Y - 1));
                }

                //right
                if (point.X > 0 && point.Y < 19 && b[point.X - 1, point.Y + 1] == target && p.Find(new Point(point.X - 1, point.Y + 1)) == null)
                {
                    q.Enqueue(new Point(point.X - 1, point.Y + 1));
                }

                //down
                //left
                if (point.X < 10 && point.Y > 0 && b[point.X + 1, point.Y - 1] == target && p.Find(new Point(point.X + 1, point.Y - 1)) == null)
                {
                    q.Enqueue(new Point(point.X + 1, point.Y - 1));
                }

                //right
                if (point.X < 10 && point.Y < 19 && b[point.X + 1, point.Y + 1] == target && p.Find(new Point(point.X + 1, point.Y + 1)) == null)
                {
                    q.Enqueue(new Point(point.X + 1, point.Y + 1));
                }

                p.AddLast(point);
                q.Dequeue();


            } while (q.Count > 0);

            if (p.Count > 2)
            {
                foreach (Point l in p)
                {
                    b[l.X, l.Y] = -1;
                }
            }
            else
            {
                p.Clear();
            }

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
            }
            else Plus[0] = -1;

            Plus[1] = Math.Abs(m);


            return Plus;
        }

        protected bool IsAddedBubble(float X)
        {

            float YFirst = 880 - _moveY - CheckY;
            float YFinal = 0;
            int pseudoY = (int)(880 - _moveY - CheckY + 30 * (float)Math.Cos(_moveAngle)) - 30;
            int pseudoX_MidPoint = (int)(CheckX + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)) + 30;
            int pseudoX_LeftPoint = (int)(CheckX + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle));
            int pseudoX_RightPoint = (int)(CheckX + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)) + 60;


            if (pseudoY < 630 && pseudoX_MidPoint < 600)
            {



                if (IsBubbleHit(pseudoX_MidPoint, pseudoY))
                {

                    if ((pseudoY / 60) % 2 == 0)
                    {
                        if ((pseudoX_MidPoint / 60) * 60 + 30 - (pseudoX_MidPoint) < 0 && pseudoX_MidPoint / 60 < 10)
                        {
                            _bbColor[pseudoY / 60 + 1, (pseudoX_MidPoint / 60) * 2 + 1] = _newbb;
                            popped(_bbColor, (pseudoY / 60), (pseudoX_MidPoint / 60) * 2 + 1);
                        }

                        else
                        {
                            _bbColor[pseudoY / 60 + 1, (pseudoX_MidPoint / 60 - 1) * 2 + 1] = _newbb;
                            popped(_bbColor, pseudoY / 60 + 1, (pseudoX_MidPoint / 60 - 1) * 2 + 1);
                        }
                            
                    }
                    
                    else
                    {

                        if ((pseudoX_MidPoint / 60) * 60 - (pseudoX_MidPoint) < 0 && pseudoX_MidPoint / 60 < 10)
                        {
                            _bbColor[pseudoY / 60 + 1, (pseudoX_MidPoint / 60) * 2] = _newbb;
                            popped(_bbColor, pseudoY / 60 + 1, (pseudoX_MidPoint / 60) * 2);
                        }

                        else
                        {
                            _bbColor[pseudoY / 60 + 1, (pseudoX_MidPoint / 60 - 1) * 2] = _newbb;
                            popped(_bbColor, pseudoY / 60 + 1, (pseudoX_MidPoint / 60 - 1) * 2);
                        }
                            
                    }
                    return true;
                }


                /*
                if (IsBubbleHit(pseudoX_LeftPoint, pseudoY ))
                {
                    if ((pseudoY / 60) % 2 == 0)
                    {
                        if ((pseudoY / 60) * 60 + 30 - pseudoY < 0 && pseudoX_LeftPoint / 60 < 10)
                            _bbColor[pseudoY / 60 + 1, (pseudoX_LeftPoint / 60) * 2 + 1] = 1;
                        else
                            _bbColor[pseudoY / 60 , (pseudoX_LeftPoint / 60 + 1) * 2 ] = 1;
                    }
                    else
                    {
                        if ((pseudoY / 60) * 60 + 30 - pseudoY < 0 && pseudoX_LeftPoint / 60 < 10)
                            _bbColor[pseudoY / 60 + 1, (pseudoX_LeftPoint / 60) * 2 + 1] = 1;
                        else
                            _bbColor[pseudoY / 60, (pseudoX_LeftPoint / 60 + 1) * 2 ] = 1;
                    }
                }
                /*
                else if (pseudoX_RightPoint < 600 && pseudoX_MidPoint / 60 == pseudoX_RightPoint / 60)
                {
                    if (IsBubbleHit(pseudoX_RightPoint, pseudoY ))
                    {
                        if ((pseudoY / 60) % 2 == 0)
                        {
                            if ((pseudoY / 60) * 60 + 30 - pseudoY < 0 && pseudoX_LeftPoint / 60 < 10)
                                _bbColor[pseudoY / 60 + 1, (pseudoX_LeftPoint / 60) * 2 + 1] = 1;
                            else
                                _bbColor[pseudoY / 60, (pseudoX_LeftPoint / 60 - 1) * 2] = 1;
                        }
                        else
                        {
                            if ((pseudoY / 60) * 60 + 30 - pseudoY < 0 && pseudoX_LeftPoint / 60 < 10)
                                _bbColor[pseudoY / 60 + 1, (pseudoX_LeftPoint / 60) * 2 + 1] = 1;
                            else
                                _bbColor[pseudoY / 60, (pseudoX_LeftPoint / 60 - 1) * 2] = 1;
                        }
                    }
                }*/

                //_spriteBatch.DrawString(_font, "XXXXXXXaaaaaAt : " + _bbColor, new Vector2(10, 100), Color.White);


                /*
                if (_bbColor[pseudoY / 60 , (pseudoX_MidPoint / 60 )* 2] >= 0)
                {
                    //_bbColor[pseudoY / 60, (pseudoX_MidPoint / 60) * 2] = 1;
                    _PlusY = 0;
                    _PlusX = 0;
                }else if (_bbColor[pseudoY / 60, (pseudoX_LeftPoint / 60) * 2 + 1] >= 0)
                {
                    //_bbColor[pseudoY / 60, (pseudoX_MidPoint / 60) * 2 + 1] = 1;
                    _PlusY = 0;
                    _PlusX = 0;
                }*/


            }

            if (pseudoY <= 0)
            {
                _PlusY = 0;
                _PlusX = 0;
            }
            //_spriteBatch.DrawString(_font, "Mid : " + (pseudoX_MidPoint / 60) + "  Left" + ((pseudoX_MidPoint / 60) * 60 - (pseudoX_MidPoint)) + "  Right" + pseudoX_RightPoint, new Vector2(10, 100), Color.Black);


            /*
             * 
             * 
             * 
             * if (_rotateAngle >= 0 && _rotateAngle < 3)
                    {
                        _CurrentBubblePos = new Vector2(TridentPos.X + _moveX - 60 * (float)Math.Sin(_rotateAngle) + 30, TridentPos.Y - _moveY + 30);
                    }
                    else
                    {
                        _CurrentBubblePos = new Vector2(TridentPos.X + _moveX + 30, TridentPos.Y - _moveY + 60 * (float)Math.Cos(_rotateAngle) - 30);
                    }
             * 
             * 
             * 
            curPosX = CheckX + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)+30;
            curPosX = CheckX + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle);
            _spriteBatch.Draw(_Pellet, new Vector2(curPosX, TridentPos.Y - _moveY), null, Color.Red, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
            _spriteBatch.DrawString(_font, "XXXXXXXaaaaaAt : " + curPosX  , new Vector2(10, 100), Color.White);
            
            
            Level = FindLevel((int)curPosX);
            if (Level % 2 == 0 && !_Filled)
            {
                YFinal = (int)(Level * 60 + 35 - 30 * (float)Math.Cos(_moveAngle));
            }
            else if (Level % 2 == 1 && !_Filled)
            {
                YFinal = (int)(Level * 60 + 35 - 30 * (float)Math.Cos(_moveAngle));
            }
            _spriteBatch.DrawString(_font, "---At : " + _bbColor[5, 18] + "sss   " + (((X + 30) / 60) * 2), new Vector2(10, 800), Color.White);
            _spriteBatch.DrawString(_font, "---At : " + YFirst + "sss   " + curPosX, new Vector2(10, 850), Color.White);
            if (YFirst <= YFinal)
            {
                _PlusY = 0;
                _PlusX = 0;
                if (Level % 2 == 0 && !_Filled)
                {
                    _bbColor[Level, (int)((curPosX) / 60) * 2] = 1;
                    _Filled = true;
                }
                else if (Level % 2 == 1 && !_Filled)
                {
                    YFinal = (int)(Level * 60 + 35 - 30 * (float)Math.Cos(_moveAngle));
                    _Filled = true;
                }
                _bbColor[Level, (int)((curPosX) / 60) * 2 + 1] = 1;
                _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[rnd.Next(allColor.Length)], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
            */



            return false;

        }
        protected bool IsBubbleHit(int PointX, int PointY)
        {
            if ((PointY / 60) % 2 == 0 && _bbColor[PointY / 60, (PointX / 60) * 2] >= 0)
            {
                _PlusY = 0;
                _PlusX = 0;
                return true;
            }
            else if ((PointY / 60) % 2 == 1 && _bbColor[PointY / 60, (PointX / 60) * 2 + 1] >= 0)
            {
                _PlusY = 0;
                _PlusX = 0;
                return true;
            }
            return false;
        }
        protected void fallingDown()
        {
           _falling += 10;
            foreach (Point p in findBBcanFall(_bbColor, _whereNoTag(_bbColor)))
            {
                _bbColor[p.X, p.Y] = -1;
            }
            findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Clear();
        }
    }
}
