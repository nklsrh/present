
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Data{
	//Document URL: https://spreadsheets.google.com/feeds/worksheets/1vOCuBaWoXD-GBEl4aChejG4XCYaoemyS4A36-LUYNqE/public/basic?alt=json-in-script

	//Sheet SheetStartingDeck
	public static DataTypes.SheetStartingDeck startingDeck = new DataTypes.SheetStartingDeck();
	//Sheet SheetCard
	public static DataTypes.SheetCard card = new DataTypes.SheetCard();
	//Sheet SheetLevel
	public static DataTypes.SheetLevel level = new DataTypes.SheetLevel();
	//Sheet SheetTimings
	public static DataTypes.SheetTimings timings = new DataTypes.SheetTimings();
	static Data(){
		//Static constructor that initialises each sheet data
		startingDeck.Init(); card.Init(); level.Init(); timings.Init(); 
	}
}


namespace DataTypes{
	public class StartingDeck{
		public string id;

		public StartingDeck(){}

		public StartingDeck(string id){
			this.id = id;
		}
	}
	public class SheetStartingDeck: IEnumerable{
		public System.DateTime updated = new System.DateTime(2018,12,18,6,15,46);
		public readonly string[] labels = new string[]{"string id"};
		private StartingDeck[] _rows = new StartingDeck[30];
		public void Init() {
			_rows = new StartingDeck[]{
					new StartingDeck("card_draw_1"),
					new StartingDeck("card_draw_2"),
					new StartingDeck("card_mood_52"),
					new StartingDeck("card_mood_53"),
					new StartingDeck("card_mood_54"),
					new StartingDeck("card_mood_56"),
					new StartingDeck("card_mood_57"),
					new StartingDeck("card_mood_60"),
					new StartingDeck("card_mood_28"),
					new StartingDeck("card_mood_9"),
					new StartingDeck("card_mood_12"),
					new StartingDeck("card_mood_16"),
					new StartingDeck("card_mood_20"),
					new StartingDeck("card_mood_24"),
					new StartingDeck("card_mood_28"),
					new StartingDeck("card_mood_32"),
					new StartingDeck("card_mood_33"),
					new StartingDeck("card_mood_34"),
					new StartingDeck("card_mood_35"),
					new StartingDeck("card_mood_36"),
					new StartingDeck("card_mood_37"),
					new StartingDeck("card_mood_38"),
					new StartingDeck("card_mood_39"),
					new StartingDeck("card_mood_40"),
					new StartingDeck("card_mood_41"),
					new StartingDeck("card_mood_42"),
					new StartingDeck("card_mood_44"),
					new StartingDeck("card_mood_45"),
					new StartingDeck("card_mood_48"),
					new StartingDeck("card_mood_29")
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetStartingDeck t;
			public SheetEnumerator(SheetStartingDeck t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}
		/// <summary>
		/// Length of rows of this sheet
		/// </summary>
		public int Length{ get{ return _rows.Length; } }
		/// <summary>
		/// Access row item by index
		/// </summary>
		public StartingDeck this[int index]{
			get{
				return _rows[index];
			}
		}
		/// <summary>
		/// Access row item by first culumn string identifier
		/// </summary>
		public StartingDeck this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].id == id){ return _rows[i]; }
				}
				return null;
			}
		}
		/// <summary>
		/// Does an item exist with the following key?
		/// </summary>
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].id == key){ return true; }
			}
			return false;
		}
		/// <summary>
		/// List of items
		/// </summary>
		/// <returns>Returns the internal array of items.</returns>
		public StartingDeck[] ToArray(){
			return _rows;
		}
		/// <summary>
		/// Random item
		/// </summary>
		/// <returns>Returns a random item.</returns>
		public StartingDeck Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}
		//Specific Items

		public StartingDeck card_draw_1{	get{ return _rows[0]; } }
		public StartingDeck card_draw_2{	get{ return _rows[1]; } }
		public StartingDeck card_mood_52{	get{ return _rows[2]; } }
		public StartingDeck card_mood_53{	get{ return _rows[3]; } }
		public StartingDeck card_mood_54{	get{ return _rows[4]; } }
		public StartingDeck card_mood_56{	get{ return _rows[5]; } }
		public StartingDeck card_mood_57{	get{ return _rows[6]; } }
		public StartingDeck card_mood_60{	get{ return _rows[7]; } }
		public StartingDeck card_mood_28{	get{ return _rows[8]; } }
		public StartingDeck card_mood_9{	get{ return _rows[9]; } }
		public StartingDeck card_mood_12{	get{ return _rows[10]; } }
		public StartingDeck card_mood_16{	get{ return _rows[11]; } }
		public StartingDeck card_mood_20{	get{ return _rows[12]; } }
		public StartingDeck card_mood_24{	get{ return _rows[13]; } }
		public StartingDeck card_mood_2801{	get{ return _rows[14]; } }
		public StartingDeck card_mood_32{	get{ return _rows[15]; } }
		public StartingDeck card_mood_33{	get{ return _rows[16]; } }
		public StartingDeck card_mood_34{	get{ return _rows[17]; } }
		public StartingDeck card_mood_35{	get{ return _rows[18]; } }
		public StartingDeck card_mood_36{	get{ return _rows[19]; } }
		public StartingDeck card_mood_37{	get{ return _rows[20]; } }
		public StartingDeck card_mood_38{	get{ return _rows[21]; } }
		public StartingDeck card_mood_39{	get{ return _rows[22]; } }
		public StartingDeck card_mood_40{	get{ return _rows[23]; } }
		public StartingDeck card_mood_41{	get{ return _rows[24]; } }
		public StartingDeck card_mood_42{	get{ return _rows[25]; } }
		public StartingDeck card_mood_44{	get{ return _rows[26]; } }
		public StartingDeck card_mood_45{	get{ return _rows[27]; } }
		public StartingDeck card_mood_48{	get{ return _rows[28]; } }
		public StartingDeck card_mood_29{	get{ return _rows[29]; } }

	}
}
namespace DataTypes{
	public class Card:DCard{

		public Card(){}

		public Card(string id, int index, string name, string description, DCard.eCardType cardType, int cardsToDraw, string comboMood, int comboMultiplier, string[] moodChanges){
			this.id = id;
			this.index = index;
			this.name = name;
			this.description = description;
			this.cardType = cardType;
			this.cardsToDraw = cardsToDraw;
			this.comboMood = comboMood;
			this.comboMultiplier = comboMultiplier;
			this.moodChanges = moodChanges;
		}
	}
	public class SheetCard: IEnumerable{
		public System.DateTime updated = new System.DateTime(2018,12,18,6,15,46);
		public readonly string[] labels = new string[]{"id","index","name","description","enum cardType","int cardsToDraw","string comboMood","int comboMultiplier","string[] moodChanges"};
		private Card[] _rows = new Card[55];
		public void Init() {
			_rows = new Card[]{
					new Card("card_mood_1",1,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","0","C","1"}),
					new Card("card_mood_2",2,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","0","C","2"}),
					new Card("card_mood_3",3,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","0","C","3"}),
					new Card("card_mood_4",4,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","1","C","0"}),
					new Card("card_mood_5",5,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","1","C","1"}),
					new Card("card_mood_6",6,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","1","C","2"}),
					new Card("card_mood_7",7,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","1","C","3"}),
					new Card("card_mood_8",8,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","2","C","0"}),
					new Card("card_mood_9",9,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","2","C","1"}),
					new Card("card_mood_10",10,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","2","C","2"}),
					new Card("card_mood_11",11,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","2","C","3"}),
					new Card("card_mood_12",12,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","3","C","0"}),
					new Card("card_mood_13",13,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","3","C","1"}),
					new Card("card_mood_14",14,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","3","C","2"}),
					new Card("card_mood_15",15,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","0","B","3","C","3"}),
					new Card("card_mood_16",16,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","0","C","0"}),
					new Card("card_mood_17",17,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","0","C","1"}),
					new Card("card_mood_18",18,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","0","C","2"}),
					new Card("card_mood_19",19,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","0","C","3"}),
					new Card("card_mood_20",20,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","1","C","0"}),
					new Card("card_mood_21",21,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","1","C","1"}),
					new Card("card_mood_22",22,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","1","C","2"}),
					new Card("card_mood_23",23,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","1","C","3"}),
					new Card("card_mood_24",24,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","2","C","0"}),
					new Card("card_mood_25",25,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","2","C","1"}),
					new Card("card_mood_26",26,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","2","C","2"}),
					new Card("card_mood_27",27,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","2","C","3"}),
					new Card("card_mood_28",28,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","3","C","0"}),
					new Card("card_mood_29",29,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","3","C","1"}),
					new Card("card_mood_30",30,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","1","B","3","C","2"}),
					new Card("card_mood_32",32,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","0","C","0"}),
					new Card("card_mood_33",33,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","0","C","1"}),
					new Card("card_mood_34",34,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","0","C","2"}),
					new Card("card_mood_35",35,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","0","C","3"}),
					new Card("card_mood_36",36,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","1","C","0"}),
					new Card("card_mood_37",37,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","1","C","1"}),
					new Card("card_mood_38",38,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","1","C","2"}),
					new Card("card_mood_39",39,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","1","C","3"}),
					new Card("card_mood_40",40,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","2","C","0"}),
					new Card("card_mood_41",41,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","2","C","1"}),
					new Card("card_mood_42",42,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","2","C","2"}),
					new Card("card_mood_44",44,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","3","C","0"}),
					new Card("card_mood_45",45,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","2","B","3","C","1"}),
					new Card("card_mood_48",48,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","0","C","0"}),
					new Card("card_mood_49",49,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","0","C","1"}),
					new Card("card_mood_50",50,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","0","C","2"}),
					new Card("card_mood_51",51,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","0","C","3"}),
					new Card("card_mood_52",52,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","1","C","0"}),
					new Card("card_mood_53",53,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","1","C","1"}),
					new Card("card_mood_54",54,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","1","C","2"}),
					new Card("card_mood_56",56,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","2","C","0"}),
					new Card("card_mood_57",57,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","2","C","1"}),
					new Card("card_mood_60",60,"","",DCard.eCardType.mood,0,"",0,new string[]{"A","3","B","3","C","0"}),
					new Card("card_draw_1",73,"","",DCard.eCardType.draw,2,"",0,new string[]{}),
					new Card("card_draw_2",74,"","",DCard.eCardType.draw,3,"",0,new string[]{})
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetCard t;
			public SheetEnumerator(SheetCard t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}
		/// <summary>
		/// Length of rows of this sheet
		/// </summary>
		public int Length{ get{ return _rows.Length; } }
		/// <summary>
		/// Access row item by index
		/// </summary>
		public Card this[int index]{
			get{
				return _rows[index];
			}
		}
		/// <summary>
		/// Access row item by first culumn string identifier
		/// </summary>
		public Card this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].id == id){ return _rows[i]; }
				}
				return null;
			}
		}
		/// <summary>
		/// Does an item exist with the following key?
		/// </summary>
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].id == key){ return true; }
			}
			return false;
		}
		/// <summary>
		/// List of items
		/// </summary>
		/// <returns>Returns the internal array of items.</returns>
		public Card[] ToArray(){
			return _rows;
		}
		/// <summary>
		/// Random item
		/// </summary>
		/// <returns>Returns a random item.</returns>
		public Card Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}
		//Specific Items

		public Card card_mood_1{	get{ return _rows[0]; } }
		public Card card_mood_2{	get{ return _rows[1]; } }
		public Card card_mood_3{	get{ return _rows[2]; } }
		public Card card_mood_4{	get{ return _rows[3]; } }
		public Card card_mood_5{	get{ return _rows[4]; } }
		public Card card_mood_6{	get{ return _rows[5]; } }
		public Card card_mood_7{	get{ return _rows[6]; } }
		public Card card_mood_8{	get{ return _rows[7]; } }
		public Card card_mood_9{	get{ return _rows[8]; } }
		public Card card_mood_10{	get{ return _rows[9]; } }
		public Card card_mood_11{	get{ return _rows[10]; } }
		public Card card_mood_12{	get{ return _rows[11]; } }
		public Card card_mood_13{	get{ return _rows[12]; } }
		public Card card_mood_14{	get{ return _rows[13]; } }
		public Card card_mood_15{	get{ return _rows[14]; } }
		public Card card_mood_16{	get{ return _rows[15]; } }
		public Card card_mood_17{	get{ return _rows[16]; } }
		public Card card_mood_18{	get{ return _rows[17]; } }
		public Card card_mood_19{	get{ return _rows[18]; } }
		public Card card_mood_20{	get{ return _rows[19]; } }
		public Card card_mood_21{	get{ return _rows[20]; } }
		public Card card_mood_22{	get{ return _rows[21]; } }
		public Card card_mood_23{	get{ return _rows[22]; } }
		public Card card_mood_24{	get{ return _rows[23]; } }
		public Card card_mood_25{	get{ return _rows[24]; } }
		public Card card_mood_26{	get{ return _rows[25]; } }
		public Card card_mood_27{	get{ return _rows[26]; } }
		public Card card_mood_28{	get{ return _rows[27]; } }
		public Card card_mood_29{	get{ return _rows[28]; } }
		public Card card_mood_30{	get{ return _rows[29]; } }
		public Card card_mood_32{	get{ return _rows[30]; } }
		public Card card_mood_33{	get{ return _rows[31]; } }
		public Card card_mood_34{	get{ return _rows[32]; } }
		public Card card_mood_35{	get{ return _rows[33]; } }
		public Card card_mood_36{	get{ return _rows[34]; } }
		public Card card_mood_37{	get{ return _rows[35]; } }
		public Card card_mood_38{	get{ return _rows[36]; } }
		public Card card_mood_39{	get{ return _rows[37]; } }
		public Card card_mood_40{	get{ return _rows[38]; } }
		public Card card_mood_41{	get{ return _rows[39]; } }
		public Card card_mood_42{	get{ return _rows[40]; } }
		public Card card_mood_44{	get{ return _rows[41]; } }
		public Card card_mood_45{	get{ return _rows[42]; } }
		public Card card_mood_48{	get{ return _rows[43]; } }
		public Card card_mood_49{	get{ return _rows[44]; } }
		public Card card_mood_50{	get{ return _rows[45]; } }
		public Card card_mood_51{	get{ return _rows[46]; } }
		public Card card_mood_52{	get{ return _rows[47]; } }
		public Card card_mood_53{	get{ return _rows[48]; } }
		public Card card_mood_54{	get{ return _rows[49]; } }
		public Card card_mood_56{	get{ return _rows[50]; } }
		public Card card_mood_57{	get{ return _rows[51]; } }
		public Card card_mood_60{	get{ return _rows[52]; } }
		public Card card_draw_1{	get{ return _rows[53]; } }
		public Card card_draw_2{	get{ return _rows[54]; } }

	}
}
namespace DataTypes{
	public class Level:DLevel{

		public Level(){}

		public Level(string id, int startA, int startB, int startC, int scoreA, int scoreB, int scoreC, int startAnger, int angerLimit, int angerReduction, int deckSize, string[] moodChanges){
			this.id = id;
			this.startA = startA;
			this.startB = startB;
			this.startC = startC;
			this.scoreA = scoreA;
			this.scoreB = scoreB;
			this.scoreC = scoreC;
			this.startAnger = startAnger;
			this.angerLimit = angerLimit;
			this.angerReduction = angerReduction;
			this.deckSize = deckSize;
			this.moodChanges = moodChanges;
		}
	}
	public class SheetLevel: IEnumerable{
		public System.DateTime updated = new System.DateTime(2018,12,18,6,15,46);
		public readonly string[] labels = new string[]{"id","int startA","int startB","int startC","int scoreA","int scoreB","int scoreC","int startAnger","int angerLimit","int angerReduction","int deckSize","string[] moodChanges"};
		private Level[] _rows = new Level[5];
		public void Init() {
			_rows = new Level[]{
					new Level("Level 1",10,0,0,25,0,0,0,40,10,30,new string[]{"0;1;0;0","0;1;0;0","0;2;0;0","0;3;0;0","0;2;0;0","0;1;0;0","0;2;0;0","0;1;0;0","0;3;0;0","0;1;0;0","0;2;0;0","0;1;0;0","0;1;0;0","0;1;0;0","0;3;0;0","0;1;0;0","0;2;0;0","0;1;0;0","0;1;0;0","0;2;0;0","0;1;0;0","0;1;0;0","0;0;1;0","0;2;0;0"}),
					new Level("Level 2",10,10,10,20,20,0,0,30,10,30,new string[]{"0;1;0;0","0;0;1;0","0;1;1;0","0;0;1;0","0;1;0;0","0;1;1;0","0;1;0;0","0;1;0;0","0;1;0;0","0;0;1;0","0;0;2;0","0;1;0;0","0;0;1;0","0;1;1;0","0;1;2;0","0;1;1;0","0;0;1;0","0;0;1;0","0;1;0;0","0;1;1;0","0;1;0;0","0;3;0;0","0;0;1;0","0;0;3;0"}),
					new Level("Level 3",15,15,15,30,30,25,0,25,10,30,new string[]{"0;1;1;0","0;0;2;2","0;1;2;1","0;1;2;1","0;1;1;3","0;1;2;1","0;1;1;0","0;0;0;1","0;1;0;1","0;2;1;0","0;0;2;1","0;1;0;2","0;2;0;1","0;1;1;1","0;1;2;3","0;2;2;0","0;1;0;1","0;1;1;0","0;1;0;0","0;2;2;0","0;1;1;0","0;2;1;1","0;0;1;0","0;2;2;3"}),
					new Level("Level 4",15,15,15,30,40,30,0,20,10,30,new string[]{"0;0;1;0","0;0;2;0","0;2;2;0","0;2;0;0","0;1;2;0","0;1;3;0","0;0;0;1","0;0;1;2","0;1;1;3","0;2;1;0","0;2;3;0","0;1;2;0","0;0;2;0","0;1;1;1","0;1;2;2","0;1;2;3","0;0;3;0","0;1;2;2","0;1;1;0","0;0;2;0","0;2;1;0","0;0;1;0","0;0;2;0","0;0;3;0"}),
					new Level("Level 5",20,10,60,40,25,40,0,15,10,30,new string[]{"0;1;0;1","0;1;1;0","0;2;1;3","0;1;3;1","0;2;2;3","0;1;1;2","0;2;1;2","0;1;0;2","0;1;1;1","0;1;0;1","0;0;2;0","0;0;3;0","0;3;0;3","0;2;1;2","0;3;3;0","0;1;1;2","0;2;1;3","0;3;1;3","0;1;0;0","0;2;2;0","0;1;0;0","0;0;2;0","0;0;1;0","0;3;3;0"})
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetLevel t;
			public SheetEnumerator(SheetLevel t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}
		/// <summary>
		/// Length of rows of this sheet
		/// </summary>
		public int Length{ get{ return _rows.Length; } }
		/// <summary>
		/// Access row item by index
		/// </summary>
		public Level this[int index]{
			get{
				return _rows[index];
			}
		}
		/// <summary>
		/// Access row item by first culumn string identifier
		/// </summary>
		public Level this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].id == id){ return _rows[i]; }
				}
				return null;
			}
		}
		/// <summary>
		/// Does an item exist with the following key?
		/// </summary>
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].id == key){ return true; }
			}
			return false;
		}
		/// <summary>
		/// List of items
		/// </summary>
		/// <returns>Returns the internal array of items.</returns>
		public Level[] ToArray(){
			return _rows;
		}
		/// <summary>
		/// Random item
		/// </summary>
		/// <returns>Returns a random item.</returns>
		public Level Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}
		//Specific Items

		public Level level1{	get{ return _rows[0]; } }
		public Level level2{	get{ return _rows[1]; } }
		public Level level3{	get{ return _rows[2]; } }
		public Level level4{	get{ return _rows[3]; } }
		public Level level5{	get{ return _rows[4]; } }

	}
}
namespace DataTypes{
	public class Timings:DTimings{
		public string id;
		public string[] moodChanges;

		public Timings(){}

		public Timings(string id, string[] moodChanges){
			this.id = id;
			this.moodChanges = moodChanges;
		}
	}
	public class SheetTimings: IEnumerable{
		public System.DateTime updated = new System.DateTime(2018,12,18,6,15,46);
		public readonly string[] labels = new string[]{"id","string[] moodChanges"};
		private Timings[] _rows = new Timings[5];
		public void Init() {
			_rows = new Timings[]{
					new Timings("Level 1",new string[]{"5","3","2","2","3","2_COMBO","1.5","2","4","2","3","2","2","2","1.5","1.5","2","2","3","1.5","1.5","1.5","2","1.5"}),
					new Timings("Level 2",new string[]{"5","3","2","2","3","2_COMBO","1.5","2","4","2","3","2","2","2","1.5","1.5","2","2","3","1.5","1.5","1.5","2","1.5"}),
					new Timings("Level 3",new string[]{"5","3","2","2","3","2_COMBO","1.5","2","4","2","3","2","2","2","1.5","1.5","2","2","3","1.5","1.5","1.5","2","1.5"}),
					new Timings("Level 4",new string[]{"5","3","2","2","3","2_COMBO","1.5","2","4","2","3","2","2","2","1.5","1.5","2","2","3","1.5","1.5","1.5","2","1.5"}),
					new Timings("Level 5",new string[]{"5","3","2","2","3","2_COMBO","1.5","3","4","2","3","2","2","3","2","4","2","3","3","1.5","1.5","1.5","2","1.5"})
				};
		}
			
		public IEnumerator GetEnumerator(){
			return new SheetEnumerator(this);
		}
		private class SheetEnumerator : IEnumerator{
			private int idx = -1;
			private SheetTimings t;
			public SheetEnumerator(SheetTimings t){
				this.t = t;
			}
			public bool MoveNext(){
				if (idx < t._rows.Length - 1){
					idx++;
					return true;
				}else{
					return false;
				}
			}
			public void Reset(){
				idx = -1;
			}
			public object Current{
				get{
					return t._rows[idx];
				}
			}
		}
		/// <summary>
		/// Length of rows of this sheet
		/// </summary>
		public int Length{ get{ return _rows.Length; } }
		/// <summary>
		/// Access row item by index
		/// </summary>
		public Timings this[int index]{
			get{
				return _rows[index];
			}
		}
		/// <summary>
		/// Access row item by first culumn string identifier
		/// </summary>
		public Timings this[string id]{
			get{
				for (int i = 0; i < _rows.Length; i++) {
					if( _rows[i].id == id){ return _rows[i]; }
				}
				return null;
			}
		}
		/// <summary>
		/// Does an item exist with the following key?
		/// </summary>
		public bool ContainsKey(string key){
			for (int i = 0; i < _rows.Length; i++) {
				if( _rows[i].id == key){ return true; }
			}
			return false;
		}
		/// <summary>
		/// List of items
		/// </summary>
		/// <returns>Returns the internal array of items.</returns>
		public Timings[] ToArray(){
			return _rows;
		}
		/// <summary>
		/// Random item
		/// </summary>
		/// <returns>Returns a random item.</returns>
		public Timings Random() {
			return _rows[ UnityEngine.Random.Range(0, _rows.Length) ];
		}
		//Specific Items

		public Timings level1{	get{ return _rows[0]; } }
		public Timings level2{	get{ return _rows[1]; } }
		public Timings level3{	get{ return _rows[2]; } }
		public Timings level4{	get{ return _rows[3]; } }
		public Timings level5{	get{ return _rows[4]; } }

	}
}