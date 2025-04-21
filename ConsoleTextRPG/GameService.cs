using System.Numerics;
using System.Text.Json;

namespace GameService
{
    public enum ColorCode
    {
        //해당 프로젝트에서만 사용할 컬러 코드 (추가해도 됨)
        None = 0,
        White = 15,
        Green = 10,
        Blue = 9,
        Red = 12,
        Yellow = 14,
        Magenta = 13,
        Cyan = 11,
    }

    public class Mathod
    {
        public static bool CheckInput(out int _value)
        {
            //글자를 입력했는지 검사
            string input = Console.ReadLine();

            if (int.TryParse(input, out _value)) return true;

            Console.WriteLine("숫자를 입력하세요.");
            Thread.Sleep(1000);
            return false;
        }

        public static void ChangeFontColor(ColorCode _color)
        {
            //글 색 변경
            if(_color == ColorCode.None) Console.ResetColor();
            else Console.ForegroundColor = (ConsoleColor)_color;
        }

        public static void ChaneBackGroundColor(ColorCode _color)
        {
            //배경 색 변경
            if(_color == ColorCode.None) Console.BackgroundColor = ConsoleColor.Black;
            else Console.BackgroundColor = (ConsoleColor)_color;
        }
    }

    public struct Vector
    {
        //커스텀 벡터 구조체
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Vector(float _x = 0f, float _y = 0f, float _z = 0f)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }

    public class Vector32
    {
        //벡터 전용 저장 전용 클래스
        public Dictionary<string, Vector> data { get; set; } = new Dictionary<string, Vector>();

        public void Add(string _dataName, Vector3 _data)
        {
            if (data.ContainsKey(_dataName)) data[_dataName] = new Vector(_data.X, _data.Y, _data.Z);
            else data.Add(_dataName, new Vector(_data.X, _data.Y, _data.Z));
        }

        public Vector3 GetData(string _dataName)
        {
            if (data.ContainsKey(_dataName)) return data[_dataName].ToVector3();
            else return default;
        }
    }

    public class Data<T>
    {
        //기본 타입 데이터 저장 제네릭 클래스
        public Dictionary<string, T> data { get; set; } = new Dictionary<string, T>();

        public void Add(string _dataName, T _data)
        {
            if (this.data.ContainsKey(_dataName)) this.data[_dataName] = _data;
            else this.data.Add(_dataName, _data);
        }

        public T GetData(string _dataName)
        {
            if (data.ContainsKey(_dataName)) return data[_dataName];
            else return default;
        }
    }

    public class GameData
    {
        //게임 저장 전용 클래스
        public Data<int> integer { get; set; } = new Data<int>();
        public Data<float> floating { get; set; } = new Data<float>();
        public Data<bool> boolen { get; set; } = new Data<bool>();
        public Data<string> text { get; set; } = new Data<string>();
        public Vector32 vector { get; set; } = new Vector32();

        public void Save()
        {
            var jsonFile = new JsonSerializerOptions { WriteIndented = true };
            string toJson = JsonSerializer.Serialize(this, jsonFile);

            File.WriteAllText("SaveFile.json", toJson);
        }

        public void Load(bool _isLoad)
        {
            //이어하기 여부
            if (_isLoad && File.Exists("SaveFile.json"))
            {
                var loadFile = File.ReadAllText("SaveFile.json");

                if (!string.IsNullOrWhiteSpace(loadFile))
                {
                    var loadData = JsonSerializer.Deserialize<GameData>(loadFile);

                    integer = loadData.integer;
                    floating = loadData.floating;
                    boolen = loadData.boolen;
                    text = loadData.text;
                    vector = loadData.vector;
                }
            }
        }
    }
}
