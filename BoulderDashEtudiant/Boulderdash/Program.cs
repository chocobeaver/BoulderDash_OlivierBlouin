// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash



using System;
using System.Threading;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using static System.Net.Mime.MediaTypeNames;

namespace Boulderdash
{
    
    class Program
    {

        const int NbColonnes = 40;          // Nombre de cases par ligne
        const int NbLignes = 22;            // Nombre de lignes
        const int NbPixelsParCase = 24;     // Nombre de pixels d'une case
        const int VitessePapillon = 5;      //le nombre de fois que le papillon prend de tour de boucle pour se deplacer
        const bool end = true;
        const bool test = true;

        // Représente ce que une case du tableau peut etre 
        // {Vide, Mur, Terre, Rocher, RocherTombant, Diamant}
        enum Objet { V, M, T, R, RT, D };

        // Représente les directions de déplacement de l'ennemi ==> **maybe other
         enum Direction { Gauche, Haut, Droite, Bas };

        
        /// <summary>
        /// La classe Personnage sert à créer Boulderdash et son ennemi ==> ** may change
        /// </summary>
        class Personnage
        {
            /// <summary>
            /// Constructeur du personnage 
            /// </summary>
            /// <param name="fichierImage">Nom du fichier image</param>
            /// <param name="x">Position initiale en x</param>
            /// <param name="y">Position initiale en y</param>
            /// <param name="dir">Direction intiale</param>
            public Personnage(string fichierImage, int x, int y, Direction dir=Direction.Gauche)
            {
                X = x;
                Y = y;
                Direction = dir;
                Texture = new Texture(fichierImage);
                Sprite = new Sprite(Texture);
                int monney = 0;
            }

            /// <summary>
            /// Utilisez la méthode Afficher du personnage pour afficher le personnage
            /// dans une fenêtre
            /// </summary>
            /// <param name="Screen">Objet représentant la fenêtre</param>
            /// <param name="nbPixelsParCase">Facteur multiplicatif permettant de passer de la position
            /// dans le tableau 2D à la position dans l'image</param>
            public void Afficher(RenderWindow Screen, float nbPixelsParCase)
            {
                Vector2f position = new Vector2f(X * nbPixelsParCase, Y * nbPixelsParCase);
                Sprite.Position = position;
                Screen.Draw(Sprite);
            }
            public int Monney { get; set; }
            /// <value>
            /// Utilisez la propriété X afin de modifier la position horizontale
            /// </value>     
            public int X { get; set; }
            /// <value>
            /// Utilisez la propriété Y afin de modifier la position verticale
            /// </value>     
            public int Y { get; set; }
            /// <value>
            /// Utilisez la propriété Direction afin de conserver la direction du personnage
            /// </value>   
            public void Deplacement(Objet[,] carte, Keyboard.Key touch, Personnage RockFord)
            {
                if (touch == (Keyboard.Key.S))
                {

                    if (carte[RockFord.Y + 1, RockFord.X] == Objet.V) { RockFord.Y++; }
                    else if (carte[RockFord.Y + 1, RockFord.X] == Objet.T) { RockFord.Y++; carte[RockFord.Y, RockFord.X] = Objet.V; }
                    else if (carte[RockFord.Y + 1, RockFord.X] == Objet.D) { RockFord.Y++; carte[RockFord.Y, RockFord.X] = Objet.V; RockFord.Monney++; }

                }
                else if (touch == (Keyboard.Key.W))
                {
                    if (carte[RockFord.Y - 1, RockFord.X] == Objet.V) { RockFord.Y--; }
                    else if (carte[RockFord.Y - 1, RockFord.X] == Objet.T) { RockFord.Y--; carte[RockFord.Y, RockFord.X] = Objet.V; }
                    else if (carte[RockFord.Y - 1, RockFord.X] == Objet.D) { RockFord.Y--; carte[RockFord.Y, RockFord.X] = Objet.V; RockFord.Monney++; }
                }
                else if (touch == (Keyboard.Key.A))
                {
                    if (carte[RockFord.Y, RockFord.X - 1] == Objet.V) { RockFord.X--; }
                    else if (carte[RockFord.Y, RockFord.X - 1] == Objet.T) { RockFord.X--; carte[RockFord.Y, RockFord.X] = Objet.V; }
                    else if (carte[RockFord.Y, RockFord.X - 1] == Objet.D) { RockFord.X--; carte[RockFord.Y, RockFord.X] = Objet.V; RockFord.Monney++; }
                    else if (carte[RockFord.Y, RockFord.X - 1] == Objet.R && carte[RockFord.Y, RockFord.X - 2] == Objet.V) { RockFord.X--; carte[RockFord.Y, RockFord.X] = Objet.V; carte[RockFord.Y, RockFord.X - 1] = Objet.R; }
                }
                if (touch == (Keyboard.Key.D))
                {
                    if (carte[RockFord.Y, RockFord.X + 1] == Objet.V) { RockFord.X++; }
                    else if (carte[RockFord.Y, RockFord.X + 1] == Objet.T) { RockFord.X++; carte[RockFord.Y, RockFord.X] = Objet.V; }
                    else if (carte[RockFord.Y, RockFord.X + 1] == Objet.D) { RockFord.X++; carte[RockFord.Y, RockFord.X] = Objet.V; RockFord.Monney++; }
                    else if (carte[RockFord.Y, RockFord.X + 1] == Objet.R && carte[RockFord.Y, RockFord.X + 2] == Objet.V) { RockFord.X++; carte[RockFord.Y, RockFord.X] = Objet.V; carte[RockFord.Y, RockFord.X + 1] = Objet.R; }
                }

            }
            public Direction Direction { get; set; }

            #region
            private Texture Texture { get; set; }
            private Sprite Sprite { get; set; }
                      
            #endregion
        }
        
        #region
        static Objet[,] ChargerCarteJeu(string map)
        {
            Objet[,] carte = new Objet[NbLignes, NbColonnes];
            using (StreamReader reader = new StreamReader(map))

            {
                for (int indiceLigne = 0; indiceLigne < NbLignes; indiceLigne++)
                {
                    string line = reader.ReadLine();
                    for (int indiceColonne = 0; indiceColonne < NbColonnes; indiceColonne++)
                    {
                        string[] values = line.Split(',');
                        carte[indiceLigne, indiceColonne] = (Objet)Enum.Parse(typeof(Objet), values[indiceColonne]);
                    }
                }

            }
            return carte;
        }
        #endregion
       static void TestMap(Objet[,] carte)
       {
            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    Console.Write(carte[i, j]);
                }
                Console.WriteLine();
            }
       }
       static void showMap(Objet[,] carte, RenderWindow Screen, bool test, Personnage RockFord,bool end,
            Vector2i PositionDiamond, Vector2i PositionMur , Vector2i PositionMoth, Vector2i PositionGround, Vector2i PositionRock,
            Sprite WallSprite, Sprite GroundSprite, Sprite RockSprite, Sprite MothSprite, Sprite DiamondSprite)
        {
            //loop to draw every map part
            for (int i = 0; i < carte.GetLength(0); i++)
            {
                for (int j = 0; j < carte.GetLength(1); j++)
                {
                    //function maybe
                    if (carte[i, j] == Objet.M)
                    {
                        PositionMur.X = j; PositionMur.Y = i;
                        WallSprite.Position = (Vector2f)PositionMur * NbPixelsParCase;
                        Screen.Draw(WallSprite);
                    }

                    if (carte[i, j] == Objet.T)
                    {
                        PositionGround.X = j; PositionGround.Y = i;
                        GroundSprite.Position = (Vector2f)PositionGround * NbPixelsParCase;
                        Screen.Draw(GroundSprite);
                    }

                    if (carte[i, j] == Objet.R)
                    {
                        if (carte[i + 1, j] == Objet.V && !(RockFord.X == j && RockFord.Y == i + 1))
                        {
                            carte[i, j] = Objet.RT;
                            PositionRock.X = j; PositionRock.Y = i;
                            RockSprite.Position = (Vector2f)PositionRock * NbPixelsParCase;
                            Screen.Draw(RockSprite);
                        }
                        else if (carte[i + 1, j] == Objet.R && (carte[i + 1, j - 1] == Objet.V && carte[i, j - 1] == Objet.V))
                        {
                            carte[i, j - 1] = Objet.RT;
                            carte[i, j] = Objet.V;

                        }
                        else if (carte[i + 1, j] == Objet.R && (carte[i + 1, j + 1] == Objet.V && carte[i, j + 1] == Objet.V))
                        {

                            carte[i, j + 1] = Objet.RT;
                            carte[i, j] = Objet.V;
                        }
                        else
                        {
                            PositionRock.X = j; PositionRock.Y = i;
                            RockSprite.Position = (Vector2f)PositionRock * NbPixelsParCase;
                            Screen.Draw(RockSprite);
                        }
                    }

                    if (carte[i, j] == Objet.D)
                    {
                        PositionDiamond.X = j; PositionDiamond.Y = i;
                        DiamondSprite.Position = (Vector2f)PositionDiamond * NbPixelsParCase;
                        Screen.Draw(DiamondSprite);
                    }
                    if (carte[i, j] == Objet.RT && test)
                    {
                        test = false;
                        if (carte[i + 1, j] == Objet.V)
                        {
                            if (i + 1 == RockFord.Y && j == RockFord.X)
                            {
                                end = false;

                            }
                            else
                            {
                                carte[i, j] = Objet.V;
                                carte[i + 1, j] = Objet.RT;
                            }
                        }
                        else
                        {
                            carte[i, j] = Objet.R;
                        }
                        PositionRock.X = j; PositionRock.Y = i;
                        RockSprite.Position = (Vector2f)PositionRock * NbPixelsParCase;
                        Screen.Draw(RockSprite);
                    }
                }
            }
        }

        
        static void Main(string[] args)
        {
            
            //creation du personage
            Personnage RockFord = new Personnage("images/heros24.bmp", 3,2);
            //creating the grid
            Objet[,] carte = ChargerCarteJeu("Boulderdash.csv"); // On charge la carte (fonction déjà écrite, cette ligne ne devrait pas changer)

            // **************************************************************************
            //initialisation of sprites
            //initialise screen
            VideoMode mode = new VideoMode(NbColonnes*NbPixelsParCase,NbLignes*NbPixelsParCase);
            RenderWindow Screen = new RenderWindow(mode, "Boulder_Dash");
            //initialise wall    
            Texture MurTexture = new Texture("images/mur24.bmp");
            Sprite WallSprite = new Sprite(MurTexture);
            Vector2i PositionMur = new Vector2i(0, 0);
            //initialise Ground
            Texture GroundTexture = new Texture("images/terre24.bmp");
            Sprite GroundSprite = new Sprite(GroundTexture);
            Vector2i PositionGround = new Vector2i(0, 0);
            //initialise Rock
            Texture RockTexture = new Texture("images/roche24.bmp");
            Sprite RockSprite = new Sprite(RockTexture);
            Vector2i PositionRock = new Vector2i(0, 0);
            //initialise diamond
            Texture DiamondTexture = new Texture("images/diamant24.bmp");
            Sprite DiamondSprite = new Sprite(DiamondTexture);
            Vector2i PositionDiamond = new Vector2i(0, 0);
            //initialise hero
            Texture HeroTexture = new Texture("images/heros24.bmp");
            Sprite HeroSprite = new Sprite(HeroTexture);
            Vector2i PositionHero = new Vector2i(0, 0);
            //initialise moth
            Texture MothTexture = new Texture("images/mur24.bmp");
            Sprite MothSprite = new Sprite(MothTexture);
            Vector2i PositionMoth = new Vector2i(0, 0);
            //initialise lost
            Texture lostTexture = new Texture("images/perdu24.bmp");
            Sprite lostSprite = new Sprite(lostTexture);
            lostSprite.Position=new Vector2f(14*NbPixelsParCase, 9*NbPixelsParCase);


            //loop to show screen
            while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                bool test = true;
               
                //display the new screen 
                Screen.DispatchEvents();
                //clear the old sprite to make new one
                Screen.Clear();
                showMap(carte,Screen,test, RockFord,end,
                PositionDiamond, PositionMur, PositionMoth, PositionGround, PositionRock,
                WallSprite, GroundSprite, RockSprite, MothSprite, DiamondSprite);

                //Maybe function
                if (Keyboard.IsKeyPressed(Keyboard.Key.S)&&end)
                {
                    
                    RockFord.Deplacement(carte, Keyboard.Key.S, RockFord);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.W, RockFord);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.A, RockFord);
                  
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D) && end)
                {
                    RockFord.Deplacement(carte, Keyboard.Key.D, RockFord);
                }
                if (!end) { Screen.Draw(lostSprite); }
                //RockFord
                RockFord.Afficher(Screen,NbPixelsParCase);
                //show on screen
                Screen.Display();
                Console.WriteLine(RockFord.Monney);
                Thread.Sleep(100);

            }
        }
  
    }
}
