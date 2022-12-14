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


namespace Item
{
    enum Items {M,N }

    class Pedestal:Cave
    {
        //variavle
        #region
        Item I { get; set; }
        #endregion

        //coonstructer
        #region
        public Pedestal(Items I, Coord XY, RenderWindow Screen):base(Objet.P,Screen,XY)
        {
            this.I = Item.GetItem(I);
            base.S = this.I.S;
            
        }
        #endregion

        //function
        #region
        public override void ApplyEffect(Map.Map map)
        {
            I.Add(map.RockFord);
            I = Item.GetItem(Items.N);
        }
            
            

        #endregion

    }
    //class Item that herite from Displayable that dictate what is showable
    #region
    abstract class Item
    {
        //variavle
        #region
        public Sprite S { get; set; }
        #endregion

        //coonstructer
        #region
        public Item(string fichierImage) 
        {
            S=new Sprite(new Texture(fichierImage));
        }
        //null sprite
        public Item()
        {
            S = new Sprite();
        }
        #endregion

        //function
        #region
        //this make sure each item as an add function
        public abstract void Add(Hero.Personnage p);

        public static Item GetItem(Items I)
        {
            if (I == Items.M) { return new MiningGear() ; }
            else { return new NullItem() ; }
        }

        #endregion
    }
    #endregion
    class MiningGear :Item
    {
        public MiningGear():base("images/heros24.bmp")
        {

        }

        public override void Add(Hero.Personnage p)
        {
            p.Speed = 1;
        }
    }
    class NullItem : Item
    {
        public NullItem() : base()
        {

        }

        public override void Add(Hero.Personnage p)
        {
           
        }
    }


}
