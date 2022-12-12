// Activité Synthèse 420-KB1-LG
//olivier blouin
//commencer le 2 decembre et remit le xxxx decembre
//recreation du jeux Boulder Dash



//library
#region
using Display;
using Map;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.ComponentModel;
#endregion


namespace Hero
{
    //class hero
    #region
    class Personnage :Displayable
    {
        //variable
        #region
        public int Monney { get; set; }
        #endregion

        //constructer
        #region
        public Personnage(string fichierImage, RenderWindow Screen, Coord XY) : base(fichierImage, Screen, XY)
        {
            Monney = 0;
        }
        #endregion

        //function
        #region
        //make the moving part of the hero and his action as he move like to move a rock
        public void Deplacement(Map.Map map)
        {
            //the direction he looking reset
            Direction dir = Direction.Null;
            //movement
            if (Keyboard.IsKeyPressed(Keyboard.Key.S)) { dir = Direction.Bas; }             //move down
            else if (Keyboard.IsKeyPressed(Keyboard.Key.W)) { dir = Direction.Haut; }       //move up
            else if (Keyboard.IsKeyPressed(Keyboard.Key.A)) { dir = Direction.Gauche; }     //move left
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D)) { dir = Direction.Droite; }     //move right

            //the coord of the position he looking
            Coord dirCoord = new Coord(dir);

            //copy his own position
            Coord NewXY = dirCoord.Clone().Add(XY);

            //get the objet of his position
            Objet RN = map.GetObjet(NewXY);

            //move on moveble place
            if (RN == Objet.V || RN == Objet.T || RN == Objet.D) { XY = NewXY; map.SetObjet(XY, Objet.V); }
            if (RN == Objet.D) { Monney++; }//change his monney
            //checck if he can move the rock
            if (dir == Direction.Gauche || dir == Direction.Droite)
            {
                //copy his position 
                Coord RockCoord = NewXY.Clone().Add(dirCoord);
                //push the rock
                if (RN == Objet.R && map.IsEmpty(RockCoord)) { XY = NewXY; map.SetObjet(XY, Objet.V); map.SetObjet(RockCoord, Objet.R); }
            }


        }
        #endregion
    }
    #endregion


}
