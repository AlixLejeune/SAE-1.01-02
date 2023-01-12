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
    public class Niveau4 : GameScreen
    {
        private Game1 _myGame; // pour récupérer le jeu en cours
        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; set; }
        //DEFITION MAP
        public readonly ScreenManager _screenManager;
        //camera
        private OrthographicCamera _camera;
        
        public SpriteFont _font;
        public Player _player;

        public Boss _boss;
        public Monstre1 _baltazar;

        private SpriteBatch _spriteBatch;
        private AnimatedSprite _E;
        private Vector2 _positionE;
        public TiledMap _tiledMap;
        
        private TiledMapRenderer _tiledMapRenderer;

        private TiledMapTileLayer _mapLayer;
        private TiledMapTileLayer _mapLayer2;
        private TiledMapTileLayer _mapLayer3;

        Vector2 _moveBalta = new Vector2(1017, 300);

        SpriteFont _police;

        Texte _texte;

        private Texture2D _box;
        private Texture2D _boxPerso;
        private Texture2D _boxBalta1;
        private Texture2D _boxBalta2;

        public SoundEffect _soundPerso;
        public SoundEffect _soundPoule;
        public SoundEffect _soundBaltazar;

        public Song _songBaltazar;
        public Song _songBataille;

        public bool inter;
        public bool move;
        public bool debut = true;
        public bool persoBox = true;
        bool baltazarBox = false;

        public int chrono;
        int etat = 0;
        int count = 120;

        string txtPerso = "Oups";
        string txtPerso2 = "Je fais la suite du jeu";
        string txtPerso3 = "...";
        string txtPerso4 = "Je...";
        string txtPerso5 = "C'est quoi cette... mer";
        string txtPerso6 = "Ca commence a etre long ...";
        string txtPerso7 = "On va remedier a ca";

        string txtBalta = "STOPPPPPPPP !!!!";
        string txtBalta2 = "TU FAIS QUOI LA ?!";
        string txtBalta3 = "AH OUI JE VOIS, LA SUITE DU ...";
        string txtBalta4 = "";
        string txtBalta5 = "Comment tu ...";
        string txtBalta6 = "MAIS C'EST BIEN SUR !!!!";
        string txtBalta7 = "TU DOIS PARLER DE CE JEU LA !!!!";
        string txtBalta8 = "LES REGLES DE CE JEU SONT\nTRES SIMPLES !!!!";
        string txtBalta9 = "TU NE DOIS PAS TE FAIRE\nATTRAPER PAR LE POULET\nDURANT LES 5 PROCHAINES\nHEURES !!!!";
        string txtBalta10 = "BONNE CHANCE !!!!";
        string txtBalta11 = "QUEL DOMMAGE !!!!";
        string txtBalta12 = "REESSAYE !!!!";

        public Niveau4(Game1 game) : base(game)
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
            _player = new Player(new Vector2(1017, 484));
            _boss = new Boss(new Vector2(1170, 300),true);
            _baltazar = new Monstre1(_moveBalta);

            chrono = 1400;

            _positionE = new Vector2(900, 900);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;


            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            var Width = _graphics.PreferredBackBufferWidth;
            var Height = _graphics.PreferredBackBufferHeight;
            var WindowSize = new Vector2(Width, Height);


            SpriteBatch = new SpriteBatch(GraphicsDevice);


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

            SpriteSheet[] spriteSheetBoss = { Content.Load<SpriteSheet>("chicken_sheet.sf",new JsonContentLoader()),
                                    Content.Load<SpriteSheet>("chicken_sheet.sf",new JsonContentLoader())};
            _boss.Load(spriteSheetBoss);

            SpriteSheet[] sheetBalta ={ Content.Load<SpriteSheet>("baltazar_sprite_sheet.sf", new JsonContentLoader()),
                                Content.Load<SpriteSheet>("baltazar_sprite_sheet.sf", new JsonContentLoader())};
            _baltazar.Load(sheetBalta);


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
            _texte.Load(_police, txtPerso);

            _soundPerso = Content.Load<SoundEffect>("voix_perso");
            _soundBaltazar = Content.Load<SoundEffect>("voix_baltazar");
            _soundPoule = Content.Load<SoundEffect>("voix_princesse");

            _songBaltazar = Content.Load<Song>("maxwell music");
            _songBataille = Content.Load<Song>("bataille");

        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if(etat == 26)
            chrono--;
            
            _positionE = new Vector2(960, 474);
            _player.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 16, move);

            if(etat == 26 || etat == 30 || etat == 31 || etat == 32)
            _boss.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 16, _player);


            _baltazar.Update(gameTime, _mapLayer, _mapLayer2, _mapLayer3, 12, _player, _moveBalta, 1);

            _E.Update(gameTime);
            _E.Play("anim_E");

            //update
            if((debut && count <=0) || etat == 11 || etat == 14 || etat == 16 || etat == 17 || etat >= 30 )
                _texte.Update(gameTime, _soundPerso, 2);

            if (inter && (etat == 2 || etat == 10 || etat == 12 || etat == 13) || etat == 15 || etat == 18 || etat == 19 || etat == 20 || etat == 23 || etat == 24 || etat == 25 || etat == 29 || etat == 28)
                _texte.Update(gameTime, _soundBaltazar, 2);

            if(etat == 22)
            _texte.Update(gameTime, _soundPerso, 4);


            //count
            if (count > 0)
                count--;

            //algo
            if (count <= 0 && etat == 0) //oups
            {

                _texte.Load(_police, txtPerso);
                etat = 1;

            }

            if(_texte.fini && etat == 1 && keyboardState.IsKeyDown(Keys.Enter))
            {

                debut = false;
                move = true;
                _texte.fini = false;
            }


            if (_player.interaction && keyboardState.IsKeyDown(Keys.E))//STOPPPPPPPP
            {
                inter = true;
                _texte.Load(_police, txtBalta);
                etat = 2;
                persoBox = false;
                baltazarBox = false;
                move = false;
            }
            if (_texte.fini && etat == 2 && keyboardState.IsKeyDown(Keys.Enter))
            {
                etat = 3;
                _texte.fini = false;
                inter = false;
                
            }
            if (etat == 8)
            {
                etat = 9;
                MediaPlayer.Play(_songBaltazar);
            }
            if (etat == 9 && count <= 0)//TU FAIS QUOI LA ?!
            {
                inter = true;
                _texte.Load(_police, txtBalta2);
                etat = 10;
                _texte.fini = false;

            }
            if (_texte.fini && etat == 10 && keyboardState.IsKeyDown(Keys.Enter))//je fais la suite du jeu
            {
                etat = 11;
                _texte.Load(_police, txtPerso2);
                _texte.fini = false;
                persoBox = true;
            }
            if (_texte.fini && etat == 11 && keyboardState.IsKeyDown(Keys.Enter))//AH OUI JE VOIS, LA SUITE DU ...
            {
                etat = 12;
                _texte.Load(_police, txtBalta3);
                _texte.fini = false;
                persoBox = false;
            }

            //4emm mur dcd--------------------------

            if (_texte.fini && etat == 12 && keyboardState.IsKeyDown(Keys.Enter))// blanc 1
            {
                etat = 13;
                _texte.Load(_police, txtBalta4);
                _texte.fini = false;
                baltazarBox = true;
                MediaPlayer.Pause();
                count = 20;
            }
            if (_texte.fini && etat == 13 && keyboardState.IsKeyDown(Keys.Enter)&& count <= 0)// blanc 2
            {
                etat = 14;
                _texte.Load(_police, txtBalta4);
                _texte.fini = false;
                persoBox = true;
                count = 20;
            }
            if (_texte.fini && etat == 14 && keyboardState.IsKeyDown(Keys.Enter) && count <= 0)//comment tu ...
            {
                etat = 15;
                _texte.Load(_police, txtBalta5);
                _texte.fini = false;
                persoBox = false;

            }
            if (_texte.fini  && etat == 15 && keyboardState.IsKeyDown(Keys.Enter) )//...
            {
                etat = 16;
                _texte.Load(_police, txtPerso3);
                _texte.fini = false;
                persoBox = true;

            }
            if (_texte.fini && etat == 16 && keyboardState.IsKeyDown(Keys.Enter))// je ...
            {
                etat = 17;
                _texte.Load(_police, txtPerso4);
                _texte.fini = false;

            }
            if (_texte.fini && etat == 17 && keyboardState.IsKeyDown(Keys.Enter))//...
            {
                etat = 18;
                _texte.Load(_police, txtPerso3);
                _texte.fini = false;
                persoBox = false;
            }
            if (_texte.fini  && etat == 18 && keyboardState.IsKeyDown(Keys.Enter))//MAIS C'EST BIEN SUR !!!!
            {
                etat = 19;
                _texte.Load(_police, txtBalta6);
                _texte.fini = false;
                baltazarBox = false;
                MediaPlayer.Resume();
            }
            if (_texte.fini && etat == 19 && keyboardState.IsKeyDown(Keys.Enter))//TU DOIS PARLER DE CE JEU LA !!!!
            {
                etat = 20;
                _texte.Load(_police, txtBalta7);
                _texte.fini = false;
            }
            if (_texte.fini && etat == 20 && keyboardState.IsKeyDown(Keys.Enter))
            {
                etat = 21;
                inter = false;
                _soundPoule.Play();
                MediaPlayer.Pause();
                count = 180;
            }
            if (etat == 21  && count <= 0)//c'est quoi cette m
            {
                etat = 22;
                inter = true;
                persoBox = true;
                _texte.Load(_police, txtPerso5);
                _texte.fini = false;
            }
            if(_texte.fini  && etat == 22)//LES REGLES DE CE JEU SON TRES SIMPLE
            {
                etat = 23;
                persoBox = false;
                _texte.Load(_police, txtBalta8);
                _texte.fini = false;
                MediaPlayer.Resume();

            }
            if (_texte.fini && etat == 23 && keyboardState.IsKeyDown(Keys.Enter))//TU NE DOIS PAS TE FAIRE ATTRAPER PAR LE POULET !!!!
            {
                etat = 24;
                _texte.Load(_police, txtBalta9);
                _texte.fini = false;

            }
            if (_texte.fini && etat == 24 && keyboardState.IsKeyDown(Keys.Enter))//BONNE CHANCE !!!!
            {
                etat = 25;
                _texte.Load(_police, txtBalta10);
                _texte.fini = false;

            }
            if (_texte.fini && etat == 25 && keyboardState.IsKeyDown(Keys.Enter))
            {
                etat = 26;
                inter = false;
                move = true;
                MediaPlayer.Stop();
                MediaPlayer.Play(_songBataille);

            }
            if (etat == 27)//QUEL DOMMAGE
            {
                MediaPlayer.Pause();
                etat = 28;
                inter = true;
                move = false;
                _texte.Load(_police, txtBalta11);
                _texte.fini = false;

            }
            if (_texte.fini && etat == 28 && keyboardState.IsKeyDown(Keys.Enter))//REESAYE !!!!
            {
                etat = 29;
                _texte.Load(_police, txtBalta12);
                _texte.fini = false;

            }
            if (_texte.fini && etat == 29 && keyboardState.IsKeyDown(Keys.Enter))
            {

                etat = 26;
                chrono = 1400;
                inter = false;
                move = true;
                _boss._pos.X = 800;
                _boss._pos.Y = 530;
                MediaPlayer.Resume();

            }

            if (etat == 30)//ca commence a etre long ...
            {
                etat = 31;
                inter = true;
                persoBox = true;
                _texte.Load(_police, txtPerso6);
                _texte.fini = false;
                count = 240;
                

            }
            if (_texte.fini && etat == 31 && count <= 0)//on va remedie a ca
            {
                etat = 32;
                _texte.Load(_police, txtPerso7);
                _texte.fini = false;
                count = 240;

            }
            if (_texte.fini && etat == 32 && count <=0)
            {
                etat = 33;
                inter = false;
                MediaPlayer.Stop();
                _myGame.LoadScreenNiveau5();

            }


            //moveBaltazar
            if (etat == 3 && _moveBalta.X < 1200)
                _moveBalta.X += 1;
            else if (etat == 3 && _moveBalta.X >= 1200)
                etat = 4;

            if (etat == 4 && _moveBalta.Y < 330)
                _moveBalta.Y += 1;
            else if (etat == 4 && _moveBalta.Y >= 330)
                etat = 5;


            if (etat == 5 && _moveBalta.X > 1150)
                _moveBalta.X -= 1;
            else if (etat == 5 && _moveBalta.X <= 1150)
                etat = 6;

            if (etat == 6 && _moveBalta.Y > 300)
                _moveBalta.Y -= 1;
            else if (etat == 6 && _moveBalta.Y <= 300)
                etat = 7;

            if (etat == 7 && _moveBalta.X < 1250)
                _moveBalta.X += 1;
            else if (etat == 7 && _moveBalta.X >= 1250)
            {
                etat = 8;
                count = 60;
            }

            _camera.LookAt(_player._pos);
            _tiledMapRenderer.Update(gameTime);

            if (etat == 26  && _boss._pos == _player._pos && chrono < 1220)
                etat = 27;

            if (chrono <= 0 && etat == 26)
            {
                etat = 30;
            }

            /* if (chrono<0 || boss.pos == player.pos)
             {
                 _myGame.LoadScreen6();
             }
            */
       



        }



        public override void Draw(GameTime gameTime)
        {
            //camera
            var matrix = _camera.GetViewMatrix();

            _myGame.GraphicsDevice.Clear(Color.LightGreen);
            //map
            _tiledMapRenderer.Draw(matrix);

            //perso
            if (etat > 20)
                _boss.Draw(_spriteBatch, matrix);

           


            //txt
            if ((debut && etat > 0) || inter)
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


            if (_player.interaction && etat == 1)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_E, _positionE);
                _spriteBatch.End();
            }


            _player.Draw(_spriteBatch, matrix);

            if(etat > 1 && etat < 26)
            _baltazar.Draw(_spriteBatch, matrix);






        }






    }
}
