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
    public class Niveau3 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        //DEFITION MAP
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;
        //perso
        private Vector2 _persoPosition;


        public SpriteFont _font;
        public Player _player;

        private AnimatedSprite _E;

        private Vector2 _positionE;
        public TiledMap _tiledMap;

        private TiledMapRenderer _tiledMapRenderer;

        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayer2;
        private TiledMapTileLayer _mapLayer3;

        SpriteFont _police;

        Texte _texte;

        private Texture2D _box;
        private Texture2D _boxPerso;
        private Texture2D _bomb;

        public SoundEffect _soundPerso;
        public SoundEffect _soundBomb;
        public SoundEffect _soundPose;

        public bool inter;
        public bool move;
        public bool debut = true;
        public bool perso = false;

        int etat = 0;
        int count = 120;

        string txtPerso = "Par hasard j'ai trouve une\nbombe sur le chemin.";
        string txtPerso1 = "Je me demande pourquoi ce mur \na droite est bizarre ...";
        string txtPerso2 = "C'est fou quand meme ...";
        string txtPerso3 = "Juste la";


        public Niveau3(Game1 game) : base(game)
        {
            _texte = new Texte();
            _myGame = game;
            //camera
            var viewport = new BoxingViewportAdapter(_myGame.Window, GraphicsDevice, 1920 / 2, 1080 / 2);

            _camera = new OrthographicCamera(viewport);
            _camera.Zoom = 2f;
            //perso
            _persoPosition = new Vector2(795, 480);

            _graphics = _myGame._graphics;
            Content.RootDirectory = "Content";

        }

        public override void Initialize()
        {
            _player = new Player(_persoPosition);

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


            _tiledMap = Content.Load<TiledMap>("grotte");

            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _mapLayer2 = _tiledMap.GetLayer<TiledMapTileLayer>("interaction");
            _mapLayer3 = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _police = Content.Load<SpriteFont>("fontuwu");
            _texte.Load(_police, txtPerso);

            _box = Content.Load<Texture2D>("text_box");
            _boxPerso = Content.Load<Texture2D>("perso_box");
            _bomb = Content.Load<Texture2D>("bomb");

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundBomb = Content.Load<SoundEffect>("explosion");
            _soundPose = Content.Load<SoundEffect>("voix_baltazar");

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            _positionE = new Vector2(960, 474);
            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 16, move);

            _E.Update(gameTime);
            _E.Play("anim_E");

            if (debut || inter)
                _texte.Update(gameTime, _soundPerso, 2);

            if (debut && etat == 0)
            {

                _texte.Load(_police, txtPerso);
                etat = 1;
            }
            if (_texte.fini == true && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtPerso1);
                etat = 2;
                _texte.fini = false;
            }
            if (_texte.fini == true && etat == 2 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtPerso2);
                etat = 3;
                _texte.fini = false;

            }

            if (_texte.fini == true && etat == 3 && keyboardState.IsKeyDown(Keys.Enter))
            {
                debut = false;
                move = true;

            }

            if (_player.interaction && keyboardState.IsKeyDown(Keys.E))
            {
                inter = true;
                _texte.Load(_police, txtPerso3);
                etat = 4;
                _texte.fini = false;
                move = false;

            }
            if (_texte.fini == true && etat == 4 && keyboardState.IsKeyDown(Keys.Enter))
            {
                inter = true;
                etat = 5;
                _soundPose.Play();

            }


            if (etat == 5 && count > 0)
                count--;

            if (etat == 5 && count <= 0)
                etat = 6;

            if (etat == 6)
            {
                _soundBomb.Play();
                _myGame.LoadScreenNiveau4();
            }

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            //camera
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);

            if (debut || inter)
            {


                _spriteBatch.Begin();
                _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 550, 700);

            }


            if (etat == 5 || etat == 6)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_bomb, new Rectangle(1000, 530, 48, 48), Color.White);
                _spriteBatch.End();

            }



            if (_player.interaction && etat == 3)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }

            _player.Draw(_spriteBatch, matrix);

        }

    }
}
