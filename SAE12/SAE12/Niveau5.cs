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
    public class Niveau5 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;
        private Vector2 _positionE;


        public SpriteFont _font;
        public Player _player;
       
        private SpriteBatch _spriteBatch;
        private AnimatedSprite _E;
        //maps
        public TiledMap _tiledMap;
        public TiledMap _tiledMap2;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapRenderer _tiledMapRenderer2;
        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayer2;
        private TiledMapTileLayer _mapLayer3;

        SpriteFont _police;

        Texte texte;

        private Texture2D _box;
        private Texture2D _boxPerso;

        public SoundEffect _soundPerso;
        public SoundEffect _soundActive;

        public bool inter;
        public bool move = false;
        public bool debut = true;
        public bool active = false;

        int etat = 0;
        int count = 120;

        string txtPerso = "Voyons voir ...";
        string txtPerso2 = "Impeccable";


        public Niveau5(Game1 game) : base(game)
        {
            _myGame = game;
            //camera
            var viewport = new BoxingViewportAdapter(_myGame.Window, GraphicsDevice, 1920 / 2, 1080 / 2);

            _camera = new OrthographicCamera(viewport);
            _camera.Zoom = 2f;
            

            _graphics = _myGame._graphics;
            Content.RootDirectory = "Content";

            texte = new Texte();

        }

        public override void Initialize()
        {
            
            _player = new Player(new Vector2(500, 600));
           
            _positionE = new Vector2(900, 900);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            var WindowSize = new Vector2(Width, Height);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();

        }
        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("playerSheetWalk.sf",new JsonContentLoader())};
            _player.Load(sheets);
            
            //touche e
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim_E.sf", new JsonContentLoader());
            _E = new AnimatedSprite(spriteSheet);

            _tiledMap = Content.Load<TiledMap>("VS2");
            _tiledMap2 = Content.Load<TiledMap>("VSFalse");

            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _mapLayer2 = _tiledMap.GetLayer<TiledMapTileLayer>("interactions");
            _mapLayer3 = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _tiledMapRenderer2 = new TiledMapRenderer(GraphicsDevice, _tiledMap2);

            _box = Content.Load<Texture2D>("text_box");
            _boxPerso = Content.Load<Texture2D>("perso_box");

            _police = Content.Load<SpriteFont>("fontuwu");
            texte.Load(_police, txtPerso);

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundActive = Content.Load<SoundEffect>("voix_princesse");

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _positionE = new Vector2(960, 474);
            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 32, move);
            texte.Update(gameTime, _soundPerso, 2);

            _E.Update(gameTime);
            _E.Play("anim_E");

            if(debut && etat == 0)
            {
                etat = 1;
                texte.Load(_police, txtPerso);
                inter = true;
                texte.fini = false;
            }
            if(texte.fini && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {
                etat = 2;
                inter = false;
                move = true;
            }

            if (etat == 3 && active && count > 0)
                count--;

            if (etat == 3 && count <=0)
            {
                etat = 4;
                inter = true;
                texte.Load(_police, txtPerso2);
                texte.fini = false;

            }
            if (texte.fini && etat == 4 && keyboardState.IsKeyDown(Keys.Enter))
            {
                etat = 5;
                _myGame.LoadScreenFin();
            }

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

            if(_player.interaction && keyboardState.IsKeyDown(Keys.E) && !active)
            {
                active = true;
                move = false;
                etat = 3;
                _soundActive.Play();

            }
        }

        public override void Draw(GameTime gameTime)
        {
            //camera
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);

            if (active)
            {
                _tiledMapRenderer2.Draw(matrix);
            }

            if (inter)
            {

                _spriteBatch.Begin();
                
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                texte.Draw(_spriteBatch, 550, 700);

            }

            //perso
            _player.Draw(_spriteBatch, matrix);

            if (_player.interaction && !inter && !active)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }
            _player.Draw(_spriteBatch, matrix);

        }

    }
}
