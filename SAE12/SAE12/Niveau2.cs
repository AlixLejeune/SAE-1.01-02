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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SAE12
{
    public class Niveau2 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        //DEFITION MAP
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;

        public Player _player;
        public Monstre1 _gardeSprite;
        //txt
        SpriteFont _police;
        SpriteFont _police2;
        Texte _texte;

        private Texture2D _box;
        private Texture2D _boxPerso;
        private Texture2D _boxGarde;
        //maps
        public TiledMap _tiledMap;
        public TiledMap _tiledMap2;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapRenderer _tiledMapRenderer2;
        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayer2;
        private TiledMapTileLayer _mapLayer3;

        Vector2 _moveGarde = new Vector2(1000, 660);
        //sons
        public SoundEffect _soundPerso;
        public SoundEffect _soundSorcier;

        public Song _songSorcier;

        public bool inter;
        public bool move;
        public bool perso = false;
        int etat = 0;
        int count = 120;
        //dialogues
        string txtSorcier = "Halte Heros !";
        string txtSorcier2 = "Tu ne peux passer par cette voie !";
        string txtSorcier3 = "Ta taille est bien trop consequente\npour emprunter ce chemin !";
        string txtSorcier4 = "Il te faut faire demi-tour afin de\nrecuperer la potion des minishs au\nniveau du donjon de la foie apres\navoir manger un fruit du demon il\nfaut aller passer ton examin hunter\net activer la mission de sortie de\nprison pour pouvoir faire en sorte de\nliberer le bon gros hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh";
        string txtSorcier5 = "heuu...";
        string txtSorcier6 = "Vas-y... j'imagine...";

        string txtPerso = "C'est bon la ?";
        string txtPerso2 = "Merci.";

        public Niveau2(Game1 game) : base(game)
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
            move = true;
            _texte = new Texte();
            _player = new Player(new Vector2(800, 660));
            _gardeSprite = new Monstre1(_moveGarde);


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
            SpriteSheet[] sheet ={ Content.Load<SpriteSheet>("garde_sprite.sf", new JsonContentLoader()),
                                Content.Load<SpriteSheet>("garde_sprite.sf", new JsonContentLoader())};
            _gardeSprite.Load(sheet);
           
            _tiledMap = Content.Load<TiledMap>("trial");
            _tiledMap2 = Content.Load<TiledMap>("trialswap");

            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _mapLayer2 = _tiledMap.GetLayer<TiledMapTileLayer>("interactions");
            _mapLayer3 = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _tiledMapRenderer2 = new TiledMapRenderer(GraphicsDevice, _tiledMap2);
            _police = Content.Load<SpriteFont>("fontuwu");
            _police2 = Content.Load<SpriteFont>("fontpetit");

            _box = Content.Load<Texture2D>("text_box");
            _boxPerso = Content.Load<Texture2D>("perso_box");
            _boxGarde = Content.Load<Texture2D>("garde");
            

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundSorcier = Content.Load<SoundEffect>("voix_sorcier");

            _songSorcier = Content.Load<Song>("song_sorcier");

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, move);
            _gardeSprite.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, _player, _moveGarde, 2);


            if (inter && (etat == 1 || etat == 2 || etat == 3 || etat == 7 || etat == 8))
                _texte.Update(gameTime, _soundSorcier, 2);

            if (inter && (etat == 4))
                _texte.Update(gameTime, _soundSorcier, 0.5);

            if (inter && etat == 6 || etat == 9)
                _texte.Update(gameTime, _soundPerso, 2);

            //load
            if (_player.interaction && etat < 10)
            {
                inter = true;
                move = false;

            }
            if (inter && etat == 0)
            {
                etat = 1;
                _texte.Load(_police, txtSorcier);

                _texte.fini = false;
                MediaPlayer.Play(_songSorcier);
            }
            if (_texte.fini == true && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 2;
                _texte.Load(_police, txtSorcier2);
                _texte.fini = false;

            }
            if (_texte.fini == true && etat == 2 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 3;
                _texte.Load(_police, txtSorcier3);
                _texte.fini = false;

            }
            if (_texte.fini == true && etat == 3 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 4;
                _texte.Load(_police, txtSorcier4);
                _texte.fini = false;

            }
            if (_texte.fini == true && etat == 4)
            {

                _graphics.PreferredBackBufferWidth = 940;
                _graphics.PreferredBackBufferHeight = 540;
                _graphics.ApplyChanges();
                MediaPlayer.Stop();

                etat = 5;
                _texte.fini = false;
            }

            if (etat == 5 && count > 0)
                count--;

            if (etat == 5 && count <= 0)
            {

                _texte.Load(_police2, txtPerso);
                perso = true;
                _texte.fini = false;
                etat = 6;
            }
            if (_texte.fini == true && etat == 6 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 7;
                _texte.Load(_police2, txtSorcier5);
                _texte.fini = false;
                perso = false;

            }
            if (_texte.fini == true && etat == 7 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 8;
                _texte.Load(_police2, txtSorcier6);
                _texte.fini = false;

            }
            if (_texte.fini == true && etat == 8 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 9;
                _texte.Load(_police2, txtPerso2);
                _texte.fini = false;
                perso = true;

            }
            if (_texte.fini == true && etat == 9 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 10;
                inter = false;
                _texte.fini = false;

            }
            if (etat == 10)
                _moveGarde.Y += 1;

            if (_moveGarde.Y >= 680)
            {
                etat = 11;
                move = true;
            }
            if (_player.interaction && etat == 11)
            {
                _myGame.LoadScreenNiveau3();
            }

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);

            //map
            if (etat < 5)
                _tiledMapRenderer.Draw(matrix);

            if (etat >= 5)
                _tiledMapRenderer2.Draw(matrix);

            //perso
            _player.Draw(_spriteBatch, matrix);

            _gardeSprite.Draw(_spriteBatch, matrix);

            if (inter && etat < 5)
            {

                _spriteBatch.Begin();
                if (perso)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);
                else
                    _spriteBatch.Draw(_boxGarde, new Rectangle(550, 500, 168, 192), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 550, 700);


            }

            if (inter && etat >= 5 && count <= 0)
            {
                _spriteBatch.Begin();
                if (perso)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(300, 265, 168 / 2, 192 / 2), Color.White);
                else
                    _spriteBatch.Draw(_boxGarde, new Rectangle(300, 265, 168 / 2, 192 / 2), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(250, 330, 960 / 2, 400 / 2), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch, 290, 360);

            }

        }
    }
}
