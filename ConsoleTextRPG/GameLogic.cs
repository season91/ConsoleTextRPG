using GameService;
using System.Numerics;
using Manager;
using System.Reflection.Emit;


namespace GameLogic
{
    public class Item
    {
        public string name { get; private init; }
        public string itemInfo { get; private init; } //해당 아이템 설명 글
        public int atk { get; private init; }
        public int def { get; private init; }
        public int health { get; private init; }
        public int gold { get; private init; } //해당 아이템 판매 가격 or 구매 가격
        public bool equipped { get; private set; } //해당 아이템 장착 여부


        //선언 방법 예시 : new Itme("검", "날카로운 검이다.", 100, "공격력", 10);
        //재료일 경우 : new Itme("재료", "쓸모없는 재료.", 5);
        public Item(string _name, string _itemInfo, int _gold = 0, string _ability = "", int _value = 0)
        {
            name = _name;
            itemInfo = _itemInfo;
            gold = _gold;

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

            foreach(Item playerItem in _player.item)
            {
                if (playerItem.name == _item.name)
                {
                    result = true;
                }
            }
            return result;
        }
      
        public void SaveData()
        {
            GameManager.data.stringMap.Add($"{name}name", name);
            GameManager.data.stringMap.Add($"{name}itemInfo", itemInfo);
            GameManager.data.integer.Add($"{name}atk", atk);
            GameManager.data.integer.Add($"{name}def", def);
            GameManager.data.integer.Add($"{name}health", health);
            GameManager.data.integer.Add($"{name}gold", gold);
            GameManager.data.boolen.Add($"{name}equipped", equipped);
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

        public void SetName(string _name) => name = _name;

        public void SaveData()
        {
            GameManager.data.stringMap.Add("PlayerName", name);

            GameManager.data.integer.Add($"{name}hp", health);
            GameManager.data.integer.Add($"{name}atk", atk);
            GameManager.data.integer.Add($"{name}def", def);
            GameManager.data.integer.Add($"{name}gold", gold);
            GameManager.data.integer.Add($"{name}level", level);
            GameManager.data.integer.Add($"{name}bonusAtk", bonusAtk);
            GameManager.data.integer.Add($"{name}bonusDef", bonusDef);
            
            // 캐릭터 인벤토리 아이템
            foreach(var playerItem in item)
            {
                playerItem.SaveData();
            }
        }

        public void LoadData()
        {
            name = GameManager.data.stringMap.GetData("PlayerName");

            health = GameManager.data.integer.GetData($"{name}hp");
            atk = GameManager.data.integer.GetData($"{name}atk");
            def = GameManager.data.integer.GetData($"{name}def");
            gold = GameManager.data.integer.GetData($"{name}gold");
            level = GameManager.data.integer.GetData($"{name}level");
            bonusAtk = GameManager.data.integer.GetData($"{name}bonusAtk");
            bonusDef = GameManager.data.integer.GetData($"{name}bonusDef");
        }
    }
}
