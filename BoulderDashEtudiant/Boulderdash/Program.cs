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

namespace Boulderdash
{
    
    class Program
    {

        const int NbColonnes = 40;          // Nombre de cases par ligne
        const int NbLignes = 22;            // Nombre de lignes
        const int NbPixelsParCase = 24;     // Nombre de pixels d'une case
        const int VitessePapillon = 5;      //le nombre de fois que le papillon prend de tour de boucle pour se deplacer

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

        static void Main(string[] args)
        {
            //creation du personage
            Personnage RockFord = new Personnage("images/heros24.bmp", 4,4);
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
            

            //loop to show screen
            while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
               
                //display the new screen 
                Screen.DispatchEvents();
                //clear the old sprite to make new one
                Screen.Clear();
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
                            PositionRock.X = j; PositionRock.Y = i;
                            RockSprite.Position = (Vector2f)PositionRock * NbPixelsParCase;
                            Screen.Draw(RockSprite);
                        }

                        if (carte[i, j] == Objet.D)
                        {
                            PositionDiamond.X = j; PositionDiamond.Y = i;
                            DiamondSprite.Position = (Vector2f)PositionDiamond * NbPixelsParCase;
                            Screen.Draw(DiamondSprite);
                        }



                    }
                }
                //Maybe function
                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    
                    if(carte[RockFord.Y+1, RockFord.X] == Objet.V) { RockFord.Y--; }
                    else if(carte[RockFord.Y + 1, RockFord.X] == Objet.T) { RockFord.Y--; carte[RockFord.Y, RockFord.X] = Objet.V; }
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    if (carte[RockFord.Y-1, RockFord.X] == Objet.V) { RockFord.Y++; }
                    else if (carte[RockFord.Y - 1, RockFord.X] == Objet.T) { RockFord.Y++; carte[RockFord.Y, RockFord.X] = Objet.V; }
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    if (carte[RockFord.Y, RockFord.X-1] == Objet.V) { RockFord.X--; }
                    if (carte[RockFord.Y, RockFord.X - 1] == Objet.T) { RockFord.X--; carte[RockFord.Y, RockFord.X] = Objet.V;}
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    if (carte[RockFord.Y, RockFord.X+1] == Objet.V) { RockFord.X++; }
                    if (carte[RockFord.Y, RockFord.X + 1] == Objet.T) { RockFord.X++; carte[RockFord.Y, RockFord.X] = Objet.V; }
                }
                //RockFord
                RockFord.Afficher(Screen,NbPixelsParCase);
                Screen.Display();
                Thread.Sleep(100);

            }
        }
  
    }
}
