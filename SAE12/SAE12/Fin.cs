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
    public class Fin : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        //DEFITION MAP
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;

        public Monstre1 _boss;

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
        private Texture2D _boxBalta1;
        private Texture2D _boxBalta2;

        public SoundEffect _soundPerso;
        public SoundEffect _soundBaltazar;

        public bool inter;
        public bool move;
        public bool debut = true;
        public bool active = false;
        public bool persoBox = true;
        bool baltazarBox = false;

        public int chrono;
        int etat = 0;
        int count = 120;

        string txtPerso = "Ouais non ...";
        string txtPerso2 = "Flemme ...";

        string txtBalta = "Oh mon dieu !";
        string txtBalta2 = "Le poulet ne fait plus parti de\nl'ensemble des reels";
        string txtBalta3 = "POUR TA PEINE, TU VAS\nAFFRONTER UN BOSS\nINVINCIBLE !!!!";
        string txtBalta4 = "Bonne chance !";


        Vector2 _pos = new Vector2(795, 520);

        public Fin(Game1 game) : base(game)
        {
            _myGame = game;
            
            //camera
            var viewport = new BoxingViewportAdapter(_myGame.Window, GraphicsDevice, 1920 / 2, 1080 / 2);

            _camera = new OrthographicCamera(viewport);
            _camera.Zoom = 2f;

            _graphics = _myGame._graphics;
            Content.RootDirectory = "Content";

            _texte = new Texte();

        }

        public override void Initialize()
        {
            _player = new Player(_pos);

            _boss = new Monstre1(new Vector2(700, 620));

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

            SpriteSheet[] sheetBoss ={ Content.Load<SpriteSheet>("Character_sheet.sf", new JsonContentLoader()),
                                Content.Load<SpriteSheet>("Character_sheet.sf", new JsonContentLoader())};
            _boss.Load(sheetBoss);

            //touche e
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("anim_E.sf", new JsonContentLoader());
            _E = new AnimatedSprite(spriteSheet);

            _tiledMap = Content.Load<TiledMap>("grottedetruite");

            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _mapLayer2 = _tiledMap.GetLayer<TiledMapTileLayer>("interaction");
            _mapLayer3 = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _box = Content.Load<Texture2D>("text_box");
            _boxPerso = Content.Load<Texture2D>("perso_box");
            _boxBalta1 = Content.Load<Texture2D>("baltazar1_box");
            _boxBalta2 = Content.Load<Texture2D>("baltazar2_box");

            _police = Content.Load<SpriteFont>("fontuwu");
            _texte.Load(_police, txtBalta);

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundBaltazar = Content.Load<SoundEffect>("voix_baltazar");

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            _positionE = new Vector2(960, 474);
            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 16, move);
            _boss.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, _player, new Vector2(700, 620), 3);

            _E.Update(gameTime);
            _E.Play("anim_E");



            if (inter && (etat == 1 || etat == 2 || etat == 3 || etat == 4))
            _texte.Update(gameTime, _soundBaltazar, 2);

            if (inter && (etat == 6 || etat == 7))
                _texte.Update(gameTime, _soundPerso, 2);

            if (count > 0)
                count--;

            if(debut && etat == 0 && count <= 0)
            {

                etat = 1;
                _texte.Load(_police, txtBalta);
                inter = true;
                persoBox = false;
                baltazarBox = true;
                _texte.fini = false;

            }
            if (_texte.fini && debut && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 2;
                _texte.Load(_police, txtBalta2);
                _texte.fini = false;

            }
            if (_texte.fini && debut && etat == 2 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 3;
                _texte.Load(_police, txtBalta3);
                _texte.fini = false;
                baltazarBox = false;

            }
            if (_texte.fini && debut && etat == 3 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 4;
                _texte.Load(_police, txtBalta4);
                _texte.fini = false;
                baltazarBox = true;

            }
            if (_texte.fini && debut && etat == 4 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 5;
                inter = false;
                move = true;
                count = 300;

            }

            if(etat == 5 && count <= 0)
            {
                etat = 6;
                move = false;
                inter = true;
                _texte.Load(_police, txtPerso);
                _texte.fini = false;
                persoBox = true;

            }
            if (_texte.fini && debut && etat == 6 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 7;
                _texte.fini = false;
                _texte.Load(_police, txtPerso2);

            }
            if (_texte.fini && debut && etat == 7 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 8;
                

            }

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

            if(etat == 8 && _texte.fini)
            {
                _myGame.LoadScreenCredits();
            }

        }


        public override void Draw(GameTime gameTime)
        {
            //camera
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);

            _boss.Draw(_spriteBatch, matrix);

            if (inter)
            {

                _spriteBatch.Begin();
                if (persoBox)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);
                else if (baltazarBox)
                    _spriteBatch.Draw(_boxBalta1, new Rectangle(550, 450, 252, 288), Color.White);
                else
                    _spriteBatch.Draw(_boxBalta2, new Rectangle(550, 450, 252, 288), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 550, 700);

            }

            if (_player.interaction)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }
            _player.Draw(_spriteBatch, matrix);

        }

    }
}
