using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework.Audio;

namespace SAE12
{
    public class Lobby : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;
        public Player _player;
        public TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayerInter;
        private TiledMapTileLayer _mapLayerPorte;
        private Vector2 _positionE;
        private AnimatedSprite _E;
        public Texte _texte;
        public Texture2D _box;

        public Texture2D _boxPerso;
        //sons
        public SoundEffect _soundPrincesse;
        public SoundEffect _soundPerso;
        //police
        public SpriteFont _police;

        int count = 120;
        public bool inter;
        bool spawn = false;
        bool move = false;
        bool write = false;
        bool draw = false;
        bool boite = false;
        bool perso = true;
        bool reponse = false;
        bool reponseDraw = false;
        //textes
        string txtDebut = "Je dois aller chercher le courrier.";
        string txtLetre = "Heros ! Au secours ! J'ai ete\ncapturee par un mechant ! Viens\nvite me sauver ! \n\nPrincesse Roxane";
        string txtRep = "Flemme... Je rentre chez moi.";

        public Lobby(Game1 game) : base(game)
        {
            
            _myGame = game;
            //camera
            var viewport = new BoxingViewportAdapter(_myGame.Window, GraphicsDevice, 1920 / 2, 1080 / 2);

            _camera = new OrthographicCamera(viewport);
            _camera.Zoom = 2f;
             _graphics = _myGame._graphics;
            Content.RootDirectory = "Content";
        }

        public override void Initialize()
        {
            _player = new Player(new Vector2(744, 720));
            
            _positionE = new Vector2(900, 900);
            
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            var WindowSize = new Vector2(Width, Height);

            _spriteBatch = _myGame._spriteBatch;

            _texte = new Texte();

            base.Initialize();
            
        }
        public override void LoadContent()
        {
            
            //sprites
            
            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("playerSheetWalk.sf",new JsonContentLoader())};
            _player.Load(sheets);
            
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim_E.sf", new JsonContentLoader());
            _E = new AnimatedSprite(spriteSheet);
            //txt
            _box = Content.Load<Texture2D>("text_box");
            _police = Content.Load<SpriteFont>("fontuwu");
            _boxPerso = Content.Load<Texture2D>("perso_box");

            //sons
            _soundPrincesse = Content.Load<SoundEffect>("voix_princesse");
            _soundPerso = Content.Load<SoundEffect>("voix_perso");

            //maps
            _tiledMap = Content.Load<TiledMap>("test1");
            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Collisions");
            _mapLayerInter = _tiledMap.GetLayer<TiledMapTileLayer>("Inter");
            _mapLayerPorte = _tiledMap.GetLayer<TiledMapTileLayer>("Porte");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            
        }
        public override void Update(GameTime gameTime)
        {
            _player.Update(gameTime,_mapLayer,_mapLayerInter,_mapLayerPorte,16,move);
            //touche E et animation
            _positionE = new Vector2(960,474);
            _E.Update(gameTime);
            _E.Play("anim_E");
            
             _camera.LookAt(_player._pos);

            _tiledMapRenderer.Update(gameTime);
            
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (_player.porte && keyboardState.IsKeyDown(Keys.E)&& boite) 
            {
                _myGame.LoadScreenNiveau1();
            }
            if(_player.interaction && keyboardState.IsKeyDown(Keys.E)&& move)
            {
                inter = true;
                move = false;
                _texte.fini = false;

            }
            if(inter)
            {
                _texte.Update(gameTime, _soundPrincesse,2);
            }
               
            if((keyboardState.IsKeyDown(Keys.S)|| keyboardState.IsKeyDown(Keys.Z)|| keyboardState.IsKeyDown(Keys.D)|| keyboardState.IsKeyDown(Keys.Q))&& move)
            {
                draw = false;
                write = false;
                inter = false;
                perso = false;
                reponseDraw = false;
                _texte.fini = false;
            }
            //cinematique debut

            if (write && !draw)
            {
                _texte.Load(_police, txtDebut);
                draw = true;
            }
            if (inter && !boite && !reponse)
            {
                _texte.Load(_police, txtLetre);
                boite = true;
                _texte.fini = false;
            }

            if (spawn && count <= 0 && !move)
                write = true;

            if (draw && !move)
            {
                _texte.Update(gameTime, _soundPerso,2);
            }

            if (!move && !inter)
                count--;

            if (count < 0 && !write)
            {
                spawn = true;
                count = 60;
            }


            if (_texte.fini && !boite)
            {
                move = true;

            }

            if (reponse)
            {
                _texte.Load(_police, txtRep);
                draw = true;
                perso = true;
                reponse = false;
                reponseDraw = true;
                _texte.fini = false;
            }

            if (_texte.fini && boite && !reponseDraw && (keyboardState.IsKeyDown(Keys.Enter)))
            {
                reponse = true;
                _texte.fini = false;
            }

            if (reponseDraw && _texte.fini)
            {
                move = true;
                reponseDraw = false;
            }

        }

        public override void Draw(GameTime gameTime)
        {

            //camera
            var matrix = _camera.GetViewMatrix();
            //
            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);
            //E porte
            if (_player.porte && boite)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }
            //E boitte lettre
            if (_player.interaction)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }
            //texte boite lettre
            if (inter)
            {
                _spriteBatch.Begin();
                if (perso)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 550, 700);

            }

            if (draw)
            {
                _spriteBatch.Begin();
                if (perso)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);



                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 550, 700);

            }

            //perso
            if (spawn)
            {
                _player.Draw(_spriteBatch, matrix);
            }
        }
    }
}
