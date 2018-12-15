
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Data{
	//Document URL: https://spreadsheets.google.com/feeds/worksheets/1vOCuBaWoXD-GBEl4aChejG4XCYaoemyS4A36-LUYNqE/public/basic?alt=json-in-script

	//Sheet SheetCard
	public static DataTypes.SheetCard card = new DataTypes.SheetCard();
	//Sheet SheetLevel
	public static DataTypes.SheetLevel level = new DataTypes.SheetLevel();
	//Sheet SheetTimings
	public static DataTypes.SheetTimings timings = new DataTypes.SheetTimings();
	static Data(){
		//Static constructor that initialises each sheet data
		card.Init(); level.Init(); timings.Init(); 
	}
}


namespace DataTypes{
	public class Card:DCard{
		public string id;
		public int index;
		public string name;
		public string description;
		public enum eCardType{
			mood,
			combo,
			draw,
		}
		public eCardType cardType;
		public int cardsToDraw;
		public string comboMood;
		public float comboMultiplier;
		public string[] moodChanges;

		public Card(){}

		public Card(string id, int index, string name, string description, Card.eCardType cardType, int cardsToDraw, string comboMood, float comboMultiplier, string[] moodChanges){
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
		public System.DateTime updated = new System.DateTime(2018,12,15,7,31,26);
		public readonly string[] labels = new string[]{"id","index","name","description","enum cardType","int cardsToDraw","string comboMood","float comboMultiplier","string[] moodChanges"};
		private Card[] _rows = new Card[75];
		public void Init() {
			_rows = new Card[]{
					new Card("card_mood_0",0,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","0","C","0"}),
					new Card("card_mood_1",1,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","0","C","1"}),
					new Card("card_mood_2",2,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","0","C","2"}),
					new Card("card_mood_3",3,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","0","C","3"}),
					new Card("card_mood_4",4,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","1","C","0"}),
					new Card("card_mood_5",5,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","1","C","1"}),
					new Card("card_mood_6",6,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","1","C","2"}),
					new Card("card_mood_7",7,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","1","C","3"}),
					new Card("card_mood_8",8,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","2","C","0"}),
					new Card("card_mood_9",9,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","2","C","1"}),
					new Card("card_mood_10",10,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","2","C","2"}),
					new Card("card_mood_11",11,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","2","C","3"}),
					new Card("card_mood_12",12,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","3","C","0"}),
					new Card("card_mood_13",13,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","3","C","1"}),
					new Card("card_mood_14",14,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","3","C","2"}),
					new Card("card_mood_15",15,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","0","B","3","C","3"}),
					new Card("card_mood_16",16,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","0","C","0"}),
					new Card("card_mood_17",17,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","0","C","1"}),
					new Card("card_mood_18",18,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","0","C","2"}),
					new Card("card_mood_19",19,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","0","C","3"}),
					new Card("card_mood_20",20,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","1","C","0"}),
					new Card("card_mood_21",21,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","1","C","1"}),
					new Card("card_mood_22",22,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","1","C","2"}),
					new Card("card_mood_23",23,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","1","C","3"}),
					new Card("card_mood_24",24,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","2","C","0"}),
					new Card("card_mood_25",25,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","2","C","1"}),
					new Card("card_mood_26",26,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","2","C","2"}),
					new Card("card_mood_27",27,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","2","C","3"}),
					new Card("card_mood_28",28,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","3","C","0"}),
					new Card("card_mood_29",29,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","3","C","1"}),
					new Card("card_mood_30",30,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","3","C","2"}),
					new Card("card_mood_31",31,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","1","B","3","C","3"}),
					new Card("card_mood_32",32,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","0","C","0"}),
					new Card("card_mood_33",33,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","0","C","1"}),
					new Card("card_mood_34",34,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","0","C","2"}),
					new Card("card_mood_35",35,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","0","C","3"}),
					new Card("card_mood_36",36,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","1","C","0"}),
					new Card("card_mood_37",37,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","1","C","1"}),
					new Card("card_mood_38",38,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","1","C","2"}),
					new Card("card_mood_39",39,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","1","C","3"}),
					new Card("card_mood_40",40,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","2","C","0"}),
					new Card("card_mood_41",41,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","2","C","1"}),
					new Card("card_mood_42",42,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","2","C","2"}),
					new Card("card_mood_43",43,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","2","C","3"}),
					new Card("card_mood_44",44,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","3","C","0"}),
					new Card("card_mood_45",45,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","3","C","1"}),
					new Card("card_mood_46",46,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","3","C","2"}),
					new Card("card_mood_47",47,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","2","B","3","C","3"}),
					new Card("card_mood_48",48,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","0","C","0"}),
					new Card("card_mood_49",49,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","0","C","1"}),
					new Card("card_mood_50",50,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","0","C","2"}),
					new Card("card_mood_51",51,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","0","C","3"}),
					new Card("card_mood_52",52,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","1","C","0"}),
					new Card("card_mood_53",53,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","1","C","1"}),
					new Card("card_mood_54",54,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","1","C","2"}),
					new Card("card_mood_55",55,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","1","C","3"}),
					new Card("card_mood_56",56,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","2","C","0"}),
					new Card("card_mood_57",57,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","2","C","1"}),
					new Card("card_mood_58",58,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","2","C","2"}),
					new Card("card_mood_59",59,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","2","C","3"}),
					new Card("card_mood_60",60,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","3","C","0"}),
					new Card("card_mood_61",61,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","3","C","1"}),
					new Card("card_mood_62",62,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","3","C","2"}),
					new Card("card_mood_63",63,"","",Card.eCardType.mood,0,"",0f,new string[]{"A","3","B","3","C","3"}),
					new Card("card_combo_1",64,"","",Card.eCardType.combo,0,"A",2f,new string[]{}),
					new Card("card_combo_2",65,"","",Card.eCardType.combo,0,"A",3f,new string[]{}),
					new Card("card_combo_3",66,"","",Card.eCardType.combo,0,"A",5f,new string[]{}),
					new Card("card_combo_4",67,"","",Card.eCardType.combo,0,"B",2f,new string[]{}),
					new Card("card_combo_5",68,"","",Card.eCardType.combo,0,"B",3f,new string[]{}),
					new Card("card_combo_6",69,"","",Card.eCardType.combo,0,"B",5f,new string[]{}),
					new Card("card_combo_7",70,"","",Card.eCardType.combo,0,"C",2f,new string[]{}),
					new Card("card_combo_8",71,"","",Card.eCardType.combo,0,"C",3f,new string[]{}),
					new Card("card_combo_9",72,"","",Card.eCardType.combo,0,"C",5f,new string[]{}),
					new Card("card_draw_1",73,"","",Card.eCardType.draw,2,"",0f,new string[]{}),
					new Card("card_draw_2",74,"","",Card.eCardType.draw,3,"",0f,new string[]{})
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

		public Card card_mood_0{	get{ return _rows[0]; } }
		public Card card_mood_1{	get{ return _rows[1]; } }
		public Card card_mood_2{	get{ return _rows[2]; } }
		public Card card_mood_3{	get{ return _rows[3]; } }
		public Card card_mood_4{	get{ return _rows[4]; } }
		public Card card_mood_5{	get{ return _rows[5]; } }
		public Card card_mood_6{	get{ return _rows[6]; } }
		public Card card_mood_7{	get{ return _rows[7]; } }
		public Card card_mood_8{	get{ return _rows[8]; } }
		public Card card_mood_9{	get{ return _rows[9]; } }
		public Card card_mood_10{	get{ return _rows[10]; } }
		public Card card_mood_11{	get{ return _rows[11]; } }
		public Card card_mood_12{	get{ return _rows[12]; } }
		public Card card_mood_13{	get{ return _rows[13]; } }
		public Card card_mood_14{	get{ return _rows[14]; } }
		public Card card_mood_15{	get{ return _rows[15]; } }
		public Card card_mood_16{	get{ return _rows[16]; } }
		public Card card_mood_17{	get{ return _rows[17]; } }
		public Card card_mood_18{	get{ return _rows[18]; } }
		public Card card_mood_19{	get{ return _rows[19]; } }
		public Card card_mood_20{	get{ return _rows[20]; } }
		public Card card_mood_21{	get{ return _rows[21]; } }
		public Card card_mood_22{	get{ return _rows[22]; } }
		public Card card_mood_23{	get{ return _rows[23]; } }
		public Card card_mood_24{	get{ return _rows[24]; } }
		public Card card_mood_25{	get{ return _rows[25]; } }
		public Card card_mood_26{	get{ return _rows[26]; } }
		public Card card_mood_27{	get{ return _rows[27]; } }
		public Card card_mood_28{	get{ return _rows[28]; } }
		public Card card_mood_29{	get{ return _rows[29]; } }
		public Card card_mood_30{	get{ return _rows[30]; } }
		public Card card_mood_31{	get{ return _rows[31]; } }
		public Card card_mood_32{	get{ return _rows[32]; } }
		public Card card_mood_33{	get{ return _rows[33]; } }
		public Card card_mood_34{	get{ return _rows[34]; } }
		public Card card_mood_35{	get{ return _rows[35]; } }
		public Card card_mood_36{	get{ return _rows[36]; } }
		public Card card_mood_37{	get{ return _rows[37]; } }
		public Card card_mood_38{	get{ return _rows[38]; } }
		public Card card_mood_39{	get{ return _rows[39]; } }
		public Card card_mood_40{	get{ return _rows[40]; } }
		public Card card_mood_41{	get{ return _rows[41]; } }
		public Card card_mood_42{	get{ return _rows[42]; } }
		public Card card_mood_43{	get{ return _rows[43]; } }
		public Card card_mood_44{	get{ return _rows[44]; } }
		public Card card_mood_45{	get{ return _rows[45]; } }
		public Card card_mood_46{	get{ return _rows[46]; } }
		public Card card_mood_47{	get{ return _rows[47]; } }
		public Card card_mood_48{	get{ return _rows[48]; } }
		public Card card_mood_49{	get{ return _rows[49]; } }
		public Card card_mood_50{	get{ return _rows[50]; } }
		public Card card_mood_51{	get{ return _rows[51]; } }
		public Card card_mood_52{	get{ return _rows[52]; } }
		public Card card_mood_53{	get{ return _rows[53]; } }
		public Card card_mood_54{	get{ return _rows[54]; } }
		public Card card_mood_55{	get{ return _rows[55]; } }
		public Card card_mood_56{	get{ return _rows[56]; } }
		public Card card_mood_57{	get{ return _rows[57]; } }
		public Card card_mood_58{	get{ return _rows[58]; } }
		public Card card_mood_59{	get{ return _rows[59]; } }
		public Card card_mood_60{	get{ return _rows[60]; } }
		public Card card_mood_61{	get{ return _rows[61]; } }
		public Card card_mood_62{	get{ return _rows[62]; } }
		public Card card_mood_63{	get{ return _rows[63]; } }
		public Card card_combo_1{	get{ return _rows[64]; } }
		public Card card_combo_2{	get{ return _rows[65]; } }
		public Card card_combo_3{	get{ return _rows[66]; } }
		public Card card_combo_4{	get{ return _rows[67]; } }
		public Card card_combo_5{	get{ return _rows[68]; } }
		public Card card_combo_6{	get{ return _rows[69]; } }
		public Card card_combo_7{	get{ return _rows[70]; } }
		public Card card_combo_8{	get{ return _rows[71]; } }
		public Card card_combo_9{	get{ return _rows[72]; } }
		public Card card_draw_1{	get{ return _rows[73]; } }
		public Card card_draw_2{	get{ return _rows[74]; } }

	}
}
namespace DataTypes{
	public class Level:DLevel{

		public Level(){}

		public Level(string id, int startA, int startB, int startC, int scoreA, int scoreB, int scoreC, string[] moodChanges){
			this.id = id;
			this.startA = startA;
			this.startB = startB;
			this.startC = startC;
			this.scoreA = scoreA;
			this.scoreB = scoreB;
			this.scoreC = scoreC;
			this.moodChanges = moodChanges;
		}
	}
	public class SheetLevel: IEnumerable{
		public System.DateTime updated = new System.DateTime(2018,12,15,7,31,26);
		public readonly string[] labels = new string[]{"id","int startA","int startB","int startC","int scoreA","int scoreB","int scoreC","string[] moodChanges"};
		private Level[] _rows = new Level[1];
		public void Init() {
			_rows = new Level[]{
					new Level("test_level",20,10,60,70,60,50,new string[]{"1","1;1","1;2","1;1","3;2","2;1","1;1","0;1","0;1","0;0","1;0","2;0","3;0","1;1","1;3","3;1","1;2","1;0","1;1","0;2","2;1","0;0","2;0","1;3","3"})
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

		public Level test_level{	get{ return _rows[0]; } }

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
		public System.DateTime updated = new System.DateTime(2018,12,15,7,31,26);
		public readonly string[] labels = new string[]{"id","string[] moodChanges"};
		private Timings[] _rows = new Timings[1];
		public void Init() {
			_rows = new Timings[]{
					new Timings("test_level",new string[]{"1;2;1;1;2;0.5;0.5;1;3;1;2;1;1;1;0.5;0.5;1;1;2;0.5;0.5;1;0.5;1"})
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

		public Timings test_level{	get{ return _rows[0]; } }

	}
}