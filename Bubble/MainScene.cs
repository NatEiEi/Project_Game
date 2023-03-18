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

        public Random rnd = new Random();
        public Color[] allColor = { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        public int[,] _bbColor;
        LinkedList<Point> _allFall;
        bool _isFalled;

        public Texture2D _circle, _bubble, _rect;

        public int[,] _bbPos;

        public Color[] allColor;
        LinkedList<Point> _test;
        LinkedListNode<Point> _node;

        enum GameState
        {
            WaitForReady,
            GamePaused,
            WaitingForShooting,
            ShootBubble,
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
            _bg = this.Content.Load<Texture2D>("BG_Bubble");
            _bubble = this.Content.Load<Texture2D>("bubble");
            _circle = this.Content.Load<Texture2D>("circle");
            _trident = this.Content.Load<Texture2D>("Trident");
            _font = this.Content.Load<SpriteFont>("GameFont");
            _rect = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] data = new Color[1];
            data[0] = Color.White;
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

                    break;
                case GameState.Shoot:

                    _possibleBlank(_bbColor);

                    _currentMouse = Mouse.GetState();

                    _tick += gameTime.ElapsedGameTime.Ticks / (float)TimeSpan.TicksPerSecond;
                    if (_tick >= 1 / _moveSpeed)
                    {
                        _tick = 0;

            // TODO: Add your update logic here
            

                            if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).First<Point>().X * 60 + _falling >= 900)
                            {
                                falled();
                            }
                        }
                    }

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
                        _currentGameState = GameState.ShootBubble;

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

                    if (_currentKey.IsKeyDown(Keys.S) && !_currentKey.Equals(_previousKey))
                    {
                        _falling = _bbHeight;
                        _moveSpeed = 100f;

                        _moveX = 0;
                        _moveY = 0;

                        _newbb = _nextbb;
                        _nextbb = rnd.Next(4);
                    }

                    break;
                case GameState.ShootBubble:

                    _possibleBlank(_bbColor);

                    _currentMouse = Mouse.GetState();


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

                        _moveY += _PlusY;
                        _moveX += _PlusX;

            _spriteBatch.Draw(_rect, Vector2.Zero, null, Color.DarkGoldenrod, 0f, Vector2.Zero, new Vector2(300 - 15, 900), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_rect, new Vector2(900+15, 0), null, Color.DarkGoldenrod, 0f, Vector2.Zero, new Vector2(300 - 15, 900), SpriteEffects.None, 0f);

            _spriteBatch.Draw(_rect, new Vector2(300-15, 5+(11*60)), null, Color.DarkRed, 0f, Vector2.Zero, new Vector2(630, 2), SpriteEffects.None, 0f);

            for (int i = 0; i < 11; i++) 
            { 
                for(int j = 0; j < 10; j++)
            {
                if(i % 2 == 0)
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
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Find(new Point(i, j)) == null)
                        {
                            if (_bbColor[i, j] > -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -2)
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (findBBcanFall(_bbColor, _whereNoTag(_bbColor)).Find(new Point(i, j)) == null)
                        {
                            if (_bbColor[i, j] > -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -2)
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                    }
                }
            }



            switch (_currentGameState)
            {
                case GameState.WaitForReady:
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
                case GameState.ShootBubble:


                    //หาค่าก่อนที่จะ rotate ตัว bubble  
                    //_X_Axis , _Y_Axis คือการเอาความสูงและกว้างของจุดหมุนมาแตกเป็นค่าแกน X และ Y
                    _X_Axis = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Sin(_moveAngle));
                    _Y_Axis = (float)(Math.Sqrt(30 * 30 + 240 * 240) * Math.Cos(_moveAngle));
                    //เช็คว่าไม่ออกนอกขอบ ถ้าชนขอบก็จะกลับด้าน
                    if (!(_moveX > 0 && 600 - _X_Axis - _moveX >= 315 - 30 * (float)Math.Sin(_moveAngle))
                        && !(_moveX <= 0 && _X_Axis + _moveX >= -285 + 30 * (float)Math.Sin(_moveAngle)))
                    {
                        _PlusX *= -1;
                    }
                    //วาด bubble ที่จะยิง
                    if (_isFalled)
                    {
                        _spriteBatch.Draw(_circle, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, allColor[_newbb], _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                        _spriteBatch.Draw(_bubble, new Vector2(TridentPos.X + _moveX, TridentPos.Y - _moveY), null, Color.White, _moveAngle, new Vector2(30, 240), 1f, SpriteEffects.None, 0f);
                    }

                    _spriteBatch.Draw(_circle, new Vector2(570, 850), allColor[_nextbb]);
                    _spriteBatch.Draw(_bubble, new Vector2(570, 850), Color.White);

                    break;
                case GameState.GameEnded:
                    break;

            }

            _spriteBatch.DrawString(_font, "Have : " + _currentGameState, new Vector2(600, 500), Color.Red);


            _spriteBatch.DrawString(_font, "Have : " + 2, new Vector2(600, 500), Color.Red);

            _spriteBatch.End();
            _graphics.BeginDraw();

            base.Draw(gameTime);
        }






        protected void Reset()
        {
            _bbHeight = 5;
            
            TridentPos = new Point(600, 880);

            _currentGameState = GameState.WaitingForShooting;
            _falling = _bbHeight;
            _moveSpeed = 100f;

            _moveSpeed = 100f;
            _falling = _bbHeight;
            _moveX = 0;
            _moveY = 0;
            _X_Axis = 0;
            _Y_Axis = 0;


            _newbb = _nextbb;
            _nextbb = rnd.Next(4);
            _isFalled = true;


        protected void falled()
        {
            foreach (Point p in findBBcanFall(_bbColor, _whereNoTag(_bbColor)))
            {
                _bbColor[p.X, p.Y] = -1;
            }
        }

            for (int i = 1; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (b[i, j] == -2)
                        {
                            list.AddLast(new Point(i, j));
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (b[i, j] == -2)
                        {
                            list.AddLast(new Point(i, j));
                        }
                    }
                }
            }

            return list;
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
                                if (topnear(i, j, b, -2) > 0 && rightnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else if (j > 0 && j < 18)
                            {
                                if (topnear(i, j, b, -2) == 2 && (leftnear(i, j, b, -2) > 0 || rightnear(i, j, b, -2) > 0))
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else
                            {
                                if (topnear(i, j, b, -2) == 2 && leftnear(i, j, b, -2) > 0)
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
                                if (topnear(i, j, b, -2) == 2 && rightnear(i, j, b, -2) > 0)
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else if (j > 1 && j < 19)
                            {
                                if (topnear(i, j, b, -2) == 2 && (leftnear(i, j, b, -2) > 0 || rightnear(i, j, b, -2) > 0))
                                {
                                    list.AddLast(new Point(i, j));
                                }
                            }
                            else
                            {
                                if (topnear(i, j, b, -2) > 0 && leftnear(i, j, b, -2) > 0)
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
                            if(near(i, j, b) > 0)
                            {
                                list.AddLast(new Point(i, j));
                            }
                            else
                            {
                                b[i, j] = -1;
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
                            else
                            {
                                b[i, j] = -1;
                            }
                        }
                    }
                }
            }

            foreach (Point p in list)
            {
                b[p.X, p.Y] = -2;
            }

            _test = list;
            _node = _test.Find(new Point(6, 14));
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

        protected void findBBcanFall(int[,] b, LinkedList<Point> l)
        {
            foreach (Point p1 in l)
            {
                Queue<Point> q = new Queue<Point>();
                int target = -1;

                q.Enqueue(new Point(p1.X, p1.Y));

                do
                {
                    Point point = q.Peek();

                    //left
                    if (point.Y > 1 && b[point.X, point.Y - 2] > target && _allFall.Find(new Point(point.X, point.Y - 2)) == null)
                    {
                        q.Enqueue(new Point(point.X, point.Y - 2));
                    }

                    //right
                    if (point.Y < 18 && b[point.X, point.Y + 2] > target && _allFall.Find(new Point(point.X, point.Y + 2)) == null)
                    {
                        q.Enqueue(new Point(point.X, point.Y + 2));
                    }

                    //up
                    //left
                    if (point.X > 0 && point.Y > 0 && b[point.X - 1, point.Y - 1] > target && _allFall.Find(new Point(point.X - 1, point.Y - 1)) == null)
                    {
                        q.Enqueue(new Point(point.X - 1, point.Y - 1));
                    }

                    //right
                    if (point.X > 0 && point.Y < 19 && b[point.X - 1, point.Y + 1] > target && _allFall.Find(new Point(point.X - 1, point.Y + 1)) == null)
                    {
                        q.Enqueue(new Point(point.X - 1, point.Y + 1));
                    }

                    //down
                    //left
                    if (point.X < 10 && point.Y > 0 && b[point.X + 1, point.Y - 1] > target && _allFall.Find(new Point(point.X + 1, point.Y - 1)) == null)
                    {
                        q.Enqueue(new Point(point.X + 1, point.Y - 1));
                    }

                    //right
                    if (point.X < 10 && point.Y < 19 && b[point.X + 1, point.Y + 1] > target && _allFall.Find(new Point(point.X + 1, point.Y + 1)) == null)
                    {
                        q.Enqueue(new Point(point.X + 1, point.Y + 1));
                    }

                    _allFall.AddLast(point);
                    q.Dequeue();


                } while (q.Count > 0);
            }
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
                    b[l.X, l.Y] = -2;
                }
                
            }else
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
        protected bool IsBubbleHit(int PointX, int PointY)
        {

            if (PointX < 600)
            {
                //การชนของแต่ละแถวไม่เหมือนกัน
                if ((PointY / 60) % 2 == 0 && _bbColor[PointY / 60, ((PointX) / 60) * 2] >= 0)
                {
                    _moveX = 0; _moveY = 0;
                    return true;
                }
                else if ((PointY / 60) % 2 == 1 && _bbColor[PointY / 60, ((PointX - 30) / 60) * 2 + 1] >= 0)
                {
                    _moveX = 0; _moveY = 0;
                    return true;
                }
            }

            return false;
        }
        protected bool IsAddedBubble(float X)
        {
            //กำหนดจุดhit point Y ด้านบนวงกลม และจะมี X 3 จุด คือซ้าย กลาง ขวาของรูปวงกลม
            //โดยค่าที่เราใช้ไม่ใช่ค่าจริงๆ เนื่องจากค่ามันถูกหมุนไปแล้ว ตำแหน่ง Hit point เลยต้องคำนวนค่าก่อนหมุนใหม่
            int pseudoY = (int)(880 - _moveY - _Y_Axis + 30 * (float)Math.Cos(_moveAngle)) - 30;
            int pseudoX_MidPoint = (int)(_X_Axis + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)) + 30;
            int pseudoX_LeftPoint = (int)(_X_Axis + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)) + 10;
            int pseudoX_RightPoint = (int)(_X_Axis + _moveX + 285 - 30 * (float)Math.Sin(_moveAngle)) + 50;

            if (pseudoY < 630 && pseudoX_MidPoint < 600)
            {
                //IsBubbleHit คือเช็คว่าชนรึยัง
                if (IsBubbleHit(pseudoX_MidPoint, pseudoY) || IsBubbleHit(pseudoX_LeftPoint, pseudoY + 10) || IsBubbleHit(pseudoX_RightPoint, pseudoY + 10))
                {
                    //ถ้ามันมี Y ห่างกันไม่เกิน 30 คือนับว่าอยู่แถวเดียวกัน
                    if (pseudoY / 60 == (pseudoY + 30) / 60)
                    {
                        if ((pseudoY / 60) % 2 == 0)
                        {
                            _bbColor[(pseudoY + 30) / 60, ((pseudoX_MidPoint) / 60) * 2] = _newbb;
                            popped(_bbColor, (pseudoY + 30) / 60, ((pseudoX_MidPoint) / 60) * 2);
                        }

                        else
                        {
                            _bbColor[(pseudoY + 30) / 60, ((pseudoX_MidPoint - 30) / 60) * 2 + 1] = _newbb;
                            popped(_bbColor, (pseudoY + 30) / 60, ((pseudoX_MidPoint - 30) / 60) * 2 + 1);
                        }
                    }
                    else
                    {
                        if ((pseudoY / 60) % 2 == 0)
                        {
                            _bbColor[(pseudoY + 30) / 60, ((pseudoX_MidPoint - 30) / 60) * 2 + 1] = _newbb;
                            popped(_bbColor, (pseudoY + 30) / 60, ((pseudoX_MidPoint - 30) / 60) * 2 + 1);
                        }
                        else
                        {
                            _bbColor[(pseudoY + 30) / 60, (pseudoX_MidPoint / 60) * 2] = _newbb;
                            popped(_bbColor, (pseudoY + 30) / 60, (pseudoX_MidPoint / 60) * 2);
                        }
                    }



                    return true;
                }
            }
            return false;

        }

        protected void fallingDown()
        {
            foreach (Point p in _allFall)
            {
                _bbColor[p.X, p.Y] = -1;
            }
            _allFall.Clear();
            _isFalled = true;

        }
        protected void DrawingBubble()
        {
            for (int i = 0; i < 11; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 0; j < 20; j += 2)
                    {
                        if (_allFall.Find(new Point(i, j)) == null)
                        {
                            if (_bbColor[i, j] > -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -2)
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 1; j < 20; j += 2)
                    {
                        if (_allFall.Find(new Point(i, j)) == null)
                        {
                            if (_bbColor[i, j] > -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), allColor[_bbColor[i, j]]);
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -2)
                            {
                                _spriteBatch.Draw(_bubble, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                            else if (_bbColor[i, j] == -1)
                            {
                                _spriteBatch.Draw(_circle, new Vector2(Singleton.LeftMargin + (Singleton.TileSize / 2) + (j / 2 * Singleton.TileSize), _bbHeight + (i * Singleton.TileSize)), Color.LightCyan);
                            }
                        }
                    }
                }
            }
        }
    }
}
