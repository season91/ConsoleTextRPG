namespace GamePlay
{
    public class Item
    {
        public string name { get; private init; }
        public string itemInfo { get; private init; }
        public int atk { get; private init; }
        public int def { get; private init; }
        public int health { get; private init; }
        public int gold { get; private init; }
        public bool equipped { get; private set; }

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
            if (atk != 0) return $"공격력 {(atk < 0 ? "-" : "+")} {atk}";
            else if (def != 0) return $"방어력 {(def < 0 ? "-" : "+")} {def}";
            else if (health != 0) return $"체력 {(health < 0 ? "-" : "+")} {health}";

            return "";
        }

        public void EquippedItem(bool _equipped) => equipped = _equipped;
    }

    public abstract class Job
    {
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

        public void SetName(string _name) => name = _name;
    }
}
