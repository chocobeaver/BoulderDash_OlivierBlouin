// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash


//library
#region
using System;
using System.Threading;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML.Audio;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
//other own code section
using Map;
using Display;
using Enemy;
#endregion

namespace Boulderdash
{
    
    class Program
    {
        //const
        #region
        const int NbColonnes = 40;          // number of row
        const int NbLignes = 22;            // number of line
        #endregion

        //load map
        #region
        /// <summary>
        /// this function is use to take a csv file and make a double array of Objet that is an enum of what each tile are
        /// </summary>
        /// <param name="map">map is instance of the class map that containe all the board/map info</param>
        /// <returns>return the double array/table 2D </returns>
        static Objet[,] ChargerCarteJeu(string map)
        {
            //creating the double array
            Objet[,] carte = new Objet[NbLignes, NbColonnes];
            //creating and calling reference of a library StreamReader to read csv file
            using (StreamReader reader = new StreamReader(map))
            {
                //loop for each line of the csv
                for (int indiceLigne = 0; indiceLigne < NbLignes; indiceLigne++)
                {
                    //read line
                    string line = reader.ReadLine();
                    //loop for each row of the csv
                    for (int indiceColonne = 0; indiceColonne < NbColonnes; indiceColonne++)
                    {
                        //create obj in double array by reading the file and seprate each objet by ","
                        string[] values = line.Split(',');
                        //put objet in the map
                        carte[indiceLigne, indiceColonne] = (Objet)Enum.Parse(typeof(Objet), values[indiceColonne]);
                    }
                }

            }
            //return the map
            return carte;
        }
        #endregion

        //the main
        static void Main(string[] args)
        {


            //initialise screen
            #region
            //this make the screen that we will display the game
            VideoMode mode = new VideoMode(NbColonnes * Displayable.NbPixelsParCase, NbLignes * Displayable.NbPixelsParCase);
            RenderWindow Screen = new RenderWindow(mode, "Boulder_Dash");
            #endregion

            //initialize Character
            #region
            //creating an instance of a character naming him RockFord giving it a Screen and a position what a vector Coord
            Hero.Personnage RockFord = new Hero.Personnage("images/heros24.bmp",Screen, new Coord(3, 2));
            #endregion

            //initialise map
            #region
            //creating an instance of a Map naming it  map giving it a the function that load a map from a csv file, a Screen and the character that we created
            Map.Map map = new Map.Map(ChargerCarteJeu("Boulderdash.csv"), Screen, RockFord);
            #endregion

            //initialize enemy
            #region
            //creating an instance of a ennemy as a moth naming it MOth giving it a vector as Coord for position to spawn in, a sceen and a map knowing the map
            enemy Moth = new Moth(new Coord(2, 15),Screen,map);
            //creating an instance of a ennemy as a moth naming it MOth giving it a vector as Coord for position to spawn in, a sceen and a map knowing the map
            enemy Mole = new Mole(new Coord(16,10), Screen,map);
            #endregion


            //initialise end game option
            #region
            //texture and sprite at a coord for lost
            Texture lostTexture = new Texture("images/perdu24.bmp");
            Sprite lostSprite = new Sprite(lostTexture);
            lostSprite.Position=new Vector2f(15*Displayable.NbPixelsParCase, 9*Displayable.NbPixelsParCase);
            //texture and sprite at a coord for win
            Texture WinTexture = new Texture("images/gagne24.bmp");
            Sprite WinSprite = new Sprite(WinTexture);
            WinSprite.Position = new Vector2f(15 * Displayable.NbPixelsParCase, 9 * Displayable.NbPixelsParCase);
            #endregion


            //loop to show screen
            #region
            //loop to show map till we press escape
            while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
               
                //display the new screen 
                Screen.DispatchEvents();
                //clear the old sprite to make new one
                Screen.Clear();

                //this call the map to make visible every objet in map and as it show do the action of the tile if it aucur
                map.Afficher(); 

                

                //check if the gane as ended and if false check the movement of the character
                if (map.GameContinue) { RockFord.Deplacement(map); }
                
                //check if the game as ended and if true show the sprite of the lot
                if (!map.GameContinue) { Screen.Draw(lostSprite); }

                //check if the game as ended if false show the moth and as it show make his action
                if (map.GameContinue) { Moth.Afficher(); }

                //check if the game as ended if false show the mole and as it show make his action
                if (map.GameContinue) { Mole.Afficher(); }

                //check if the game as ended if false show the character and as it show make his action
                if (map.GameContinue) { RockFord.Afficher(); }


                

                //make the screen apear
                Screen.Display();

                //delay to make it viable
                Thread.Sleep(100);

            }
            #endregion
        }

    }
}
