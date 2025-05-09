using BattleSystem;
using GameService;
using Manager;


namespace GameLogic
{
    public class Item
    {
        public int itemId { get; private set; }
        public string name { get; private set; }
        public string itemInfo { get; private set; } //해당 아이템 설명 글
        public int atk { get; private set; }
        public int def { get; private set; }
        public int health { get; private set; }
        public int gold { get; private set; } //해당 아이템 판매 가격 or 구매 가격
        public bool equipped { get; private set; } //해당 아이템 장착 여부
        public int count { get; set; }
        public bool isGet { get; set; }


        //선언 방법 예시 : new Itme("검", "날카로운 검이다.", 100, "공격력", 10);
        //재료일 경우 : new Itme("재료", "쓸모없는 재료.", 5);
        public Item(int _itemId, string _name, string _itemInfo, int _gold = 0, string _ability = "", int _value = 0, int _count = 1)
        {
            itemId = _itemId;
            name = _name;
            itemInfo = _itemInfo;
            gold = _gold;
            count = _count;

            atk = 0;
            def = 0;
            health = 0;

            if (_ability != "")
            {
                switch (_ability)
                {
                    case "공격력":
                        atk = _value;
                        break;

                    case "방어력":
                        def = _value;
                        break;

                    case "체력":
                        health = _value;
                        break;
                    default:
                        Console.Write($"({_ability})라는 Ability는 존재하지 않음");
                        break;
                }
            }
        }

        public string Ability()
        {
            //상점에 표시될 해당 아이템 효과 메서드
            if (atk != 0) return $"공격력 {(atk < 0 ? "-" : "+")} {atk}";
            else if (def != 0) return $"방어력 {(def < 0 ? "-" : "+")} {def}";
            else if (health != 0) return $"체력 {(health < 0 ? "-" : "+")} {health}";

            return "";
        }

        public void EquippedItem(bool _equipped) => equipped = _equipped;

        public bool IsSameItem(Job _player, Item _item)
        {
            bool result = false;

            foreach (Item playerItem in _player.item)
            {
                if (playerItem.name == _item.name)
                {
                    result = true;
                }

                // 회복아이템이라면 검증제외
                if(_item.itemId == (int)ItemCode.Potion)
                {
                    result = false;
                }
            }
            return result;
        }

        public void SaveData()
        {
            GameManager.data.boolen.Add($"{itemId}Get", isGet);

        }

        public void LoadData()
        {
            isGet = GameManager.data.boolen.GetData($"{itemId}Get");

            if (isGet)
            {
                GameManager.player.item.Add(this);
            }
        }
    }

    public class Job
    {
        //캐릭터 직업 클래스
        private int fieldLevel;
        public int level
        {
            get => fieldLevel;
            set => fieldLevel = value > 99 ? 99 : value;
        }

        public string name { get; protected set; } = "";
        public string chad { get; protected set; } = "";

        public List<Item> item = new List<Item>();

        public int atk;
        public int def;
        public int health;
        public int gold;
        public int bonusAtk;
        public int bonusDef;
        public int exp;
        public int Mp;
        public double critRate;
        public double critDamage;
        public double dodgeRate;
        public int floor = 1;
        public int Skill1Cost { get; protected set; }
        public int Skill2Cost { get; protected set; }
        public int Skill1HpCost { get; protected set; }
        public int Skill2HpCost { get; protected set; }
        public string Skill1Name { get; protected set; }
        public string Skill2Name { get; protected set; }
        public void SetName(string _name) => name = _name;

        public void SaveData()
        {
            var tempChad = Mathod.ConvertJobLenguage(chad, false); 
            GameManager.data.stringMap.Add("Chad", tempChad);

            GameManager.data.stringMap.Add("PlayerName", name);
            GameManager.data.integer.Add($"{name}hp", health);
            GameManager.data.integer.Add($"{name}atk", atk);
            GameManager.data.integer.Add($"{name}def", def);
            GameManager.data.integer.Add($"{name}gold", gold);
            GameManager.data.integer.Add($"{name}level", level);
            GameManager.data.integer.Add($"{name}bonusAtk", bonusAtk);
            GameManager.data.integer.Add($"{name}bonusDef", bonusDef);
            GameManager.data.integer.Add($"{name}mp", Mp);
            GameManager.data.integer.Add($"{name}exp", exp);
            GameManager.data.integer.Add($"{name}floor", floor);

            // 캐릭터 인벤토리 아이템
            foreach (var playerItem in item)
            {
                playerItem.SaveData();
            }
        }

        public void LoadData()
        {
            var tempChad = GameManager.data.stringMap.GetData("Chad");
            chad = Mathod.ConvertJobLenguage(tempChad, true);

            name = GameManager.data.stringMap.GetData("PlayerName");
            health = GameManager.data.integer.GetData($"{name}hp");
            atk = GameManager.data.integer.GetData($"{name}atk");
            def = GameManager.data.integer.GetData($"{name}def");
            gold = GameManager.data.integer.GetData($"{name}gold");
            level = GameManager.data.integer.GetData($"{name}level");
            bonusAtk = GameManager.data.integer.GetData($"{name}bonusAtk");
            bonusDef = GameManager.data.integer.GetData($"{name}bonusDef");
            Mp = GameManager.data.integer.GetData($"{name}mp");
            exp = GameManager.data.integer.GetData($"{name}exp");
            floor = GameManager.data.integer.GetData($"{name}floor");

            // 캐릭터 인벤토리 아이템
            foreach (var playerItem in GameManager.ItemPooling)
            {
                playerItem.LoadData();
            }
        }



        public virtual int Attack(Monster target)
        {
            return 0;
        }
        public virtual int Skill1(Monster target)
        {
            return 0;
        }
        public virtual void Skill2(Monster[] target)
        {
        }

    }
}
