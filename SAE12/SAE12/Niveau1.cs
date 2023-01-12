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
    public class Niveau1 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;
        
        public Player _player;
        public Monstre1 _baltazar;

        private SpriteBatch _spriteBatch;
        //map
        public TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayer2;
        private TiledMapTileLayer _mapLayer3;

        //sons
        public SoundEffect _soundPerso;
        public SoundEffect _soundBaltazar;
        public Song _songBaltazar;
        //images
        private Texture2D _box;
        private Texture2D _boxPerso;
        private Texture2D _boxBalta1;
        private Texture2D _boxBalta2;

        SpriteFont _police;
        Texte _texte;

        //PAS ACCENTS !!!
        string txtSpawn = "Flute ...";
        string txtSpawn2 = "Je suis ou encore ...";
        string txtBaltazar1 = "BONSOIR HEROS !!!!";
        string txtBaltazar2 = "BIENVENUE DANS LE TUTORIEL !!!!";
        string txtBaltazar3 = "JE VAIS T'EXPLIQUER COMMENT\nBIEN JOUER AVEC LES TOUCHES\nET TOUT !!!!";
        string txtBaltazar4 = "Tu devrais tout lire.";
        string txtBaltazar5 = "IL FAUT UTILISER ZQSD \nPOUR SE DEPLACER LOLOLOLOLOLOLOLOLOLOLOLOLOLOL";
        string txtBaltazar6 = "Voila c'est tout";
        string txtBaltazar7 = "ALLER SO LONG !!!!";
        string txtBaltazar8 = "Pas par la";

        int etat = 0;
        int count = 60;

        public bool inter;
        public bool move;
        bool perso = false;
        bool baltazarBox = false;
        

        Vector2 _moveBalta = new Vector2(870, 1352);



        public Niveau1(Game1 game) : base(game)
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
            _player = new Player(new Vector2(1015, 1100));
            _baltazar = new Monstre1(_moveBalta);


            GraphicsDevice.BlendState = BlendState.AlphaBlend;


            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            var WindowSize = new Vector2(Width, Height);


            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texte = new Texte();

            base.Initialize();


        }
        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteSheet[] sheets = { Content.Load<SpriteSheet>("playerSheetIdle.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("playerSheetWalk.sf",new JsonContentLoader())};
            _player.Load(sheets);

            SpriteSheet[] sheet ={ Content.Load<SpriteSheet>("baltazar_sprite_sheet.sf", new JsonContentLoader()),
                                Content.Load<SpriteSheet>("baltazar_sprite_sheet.sf", new JsonContentLoader())};
            _baltazar.Load(sheet);


            _tiledMap = Content.Load<TiledMap>("foret1");
            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("obstacles");
            _mapLayer2 = _tiledMap.GetLayer<TiledMapTileLayer>("interaction");
            _mapLayer3 = _tiledMap.GetLayer<TiledMapTileLayer>("map3");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _box = Content.Load<Texture2D>("text_box");
            _police = Content.Load<SpriteFont>("fontuwu");
            _boxPerso = Content.Load<Texture2D>("perso_box");
            _boxBalta1 = Content.Load<Texture2D>("baltazar1_box");
            _boxBalta2 = Content.Load<Texture2D>("baltazar2_box");

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundBaltazar = Content.Load<SoundEffect>("voix_baltazar");

            _songBaltazar = Content.Load<Song>("maxwell music");



        }
        public override void Update(GameTime gameTime)
        {


            KeyboardState keyboardState = Keyboard.GetState();

            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, move);
            _baltazar.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, _player, _moveBalta, 1);



            if ((keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Z) || keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Q)) && move)
            {
                inter = false;
                perso = false;
                _texte.fini = false;
                MediaPlayer.Stop();

            }

            //txt
            if (etat == 0 && count <= 0)
            {
                inter = true;
                perso = true;
                _texte.Load(_police, txtSpawn);

                etat = 1;
            }

            //update txt
            if (inter && (etat == 1 || etat == 2))
                _texte.Update(gameTime, _soundPerso, 2);

            if (inter && (etat == 3 || etat == 4 || etat == 5 || etat == 6 || etat == 7 || etat == 8 || etat == 9 || etat == 12))
                _texte.Update(gameTime, _soundBaltazar, 2);


            //entré dialogue
            if (_texte.fini == true && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtSpawn2);
                _texte.fini = false;
                etat = 2;
            }

            if (_texte.fini == true && etat == 3 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar2);
                _texte.fini = false;
                baltazarBox = false;
                etat = 4;
            }
            if (_texte.fini == true && etat == 4 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar3);
                _texte.fini = false;
                baltazarBox = false;
                etat = 5;
            }
            if (_texte.fini == true && etat == 5 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar4);
                _texte.fini = false;
                baltazarBox = true;
                etat = 6;
            }
            if (_texte.fini == true && etat == 6 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar5);
                _texte.fini = false;
                baltazarBox = false;
                etat = 7;
            }
            if (_texte.fini == true && etat == 7 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar6);
                _texte.fini = false;
                baltazarBox = true;
                etat = 8;
            }
            if (_texte.fini == true && etat == 8 && keyboardState.IsKeyDown(Keys.Enter))
            {
                _texte.Load(_police, txtBaltazar7);
                _texte.fini = false;
                baltazarBox = false;
                etat = 9;
            }
            if (_texte.fini == true && etat == 9 && keyboardState.IsKeyDown(Keys.Enter))
            {

                _texte.fini = false;
                inter = false;
                MediaPlayer.Stop();

                etat = 10;
            }
            if (etat == 11 && count <= 0)
            {

                _texte.fini = false;
                baltazarBox = false;
                inter = true;
                _texte.Load(_police, txtBaltazar8);
                etat = 12;

            }
            if (_texte.fini == true && etat == 12 && count == 0)
            {

                _texte.fini = false;
                inter = false;

                etat = 13;
            }



            //dialogue
            if (etat == 2 && _texte.fini && keyboardState.IsKeyDown(Keys.Enter)) 
            { 
                move = true;
            inter = false; 
            }

            if (_player.interaction && etat == 2)
            {
                MediaPlayer.Play(_songBaltazar);
                move = false;
                inter = true;
                _texte.Load(_police, txtBaltazar1);
                etat = 3;
                

            }

            if (etat == 10 && _moveBalta.X < 1007)
            {
                _moveBalta.X += 1;
            }

            if (_moveBalta.X >= 1007 && count == 0 && etat == 10)
            {
                etat = 11;
                count = 60;
            }

            if (etat == 12 && count == 0)
                count = 80;

            if (etat == 13 && _moveBalta.X > 870)
                _moveBalta.X -= 1;

            if (etat == 13 && _moveBalta.X <= 870 && _moveBalta.Y < 1500)
                _moveBalta.Y += 1;

            if (_moveBalta.Y >= 1500)
            {

                move = true;
                etat = 14;

            }
            if (count > 0)
                count--;

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

            //passer a la scene suivant


            if (_player.map3)
            {
                _myGame.LoadScreenNiveau2();
            }


        }



        public override void Draw(GameTime gameTime)
        {
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);

            //perso
            _player.Draw(_spriteBatch, matrix);

            if(etat<14)
            {
                _baltazar.Draw(_spriteBatch, matrix);
            }

            if (inter)
            {
                _spriteBatch.Begin();
                if (perso)
                    _spriteBatch.Draw(_boxPerso, new Rectangle(550, 500, 168, 192), Color.White);
                else if (baltazarBox)
                    _spriteBatch.Draw(_boxBalta1, new Rectangle(550, 450, 252, 288), Color.White);
                else
                    _spriteBatch.Draw(_boxBalta2, new Rectangle(550, 450, 252, 288), Color.White);

                _spriteBatch.Draw(_box, new Rectangle(480, 630, 960, 400), Color.White);

                _spriteBatch.End();

                _texte.Draw(_spriteBatch,550,700);


            }

        }






    }
}
