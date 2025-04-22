using System;
using System.Numerics;
using System.Text;
using System.Text.Json;
using GameCharacter;
using GameLogic;
using GameQuest;

namespace GameService
{
    public enum ColorCode
    {
        //해당 프로젝트에서만 사용할 컬러 코드 (ConsoleColor를 보고 추가해도 됨)
        None = 0,
        White = 15,
        Green = 10,
        Blue = 9,
        Red = 12,
        Yellow = 14,
        Magenta = 13,
        Cyan = 11,
        DarkGray = 8,
    }

    public class Mathod
    {
        public static Job JobToClass(int _jobIndex)
        {
            //직업 추가
            Job[] isJob =
            {
                new Warrior(),
                new Wizard(),
            };

            return isJob[_jobIndex];
        }

        public static int JobToIndex(string _chad)
        {
            //해당 이름에 맞는 직업 인덱스 반환
            var selectJobText = LoadAllText("SelectJobText");

            for (int i = 0; i < selectJobText.Length; i++)
            {
                if (selectJobText[i].Contains(_chad)) return i;
            }

            Console.WriteLine($"{_chad}라는 단어는 SelectJobText파일 안에 없음");
            return default;
        }

        public static void BufferClear()
        {
            //덮어쓰기
            Console.SetCursorPosition(0, 0);
            Console.Write(new StringBuilder().ToString());
        }

        public static bool CheckInput(out int _value)
        {
            string input = Console.ReadLine();

            //숫자를 입력했는지 검사
            if (int.TryParse(input, out _value)) return true;

            //숫자를 입력 안했을 경우
            Console.WriteLine("숫자를 입력하세요.");
            Thread.Sleep(1000);
            return false;
        }

        public static void ChangeFontColor(ColorCode _color)
        {
            //글 색 변경
            if (_color == ColorCode.None) Console.ResetColor();
            else Console.ForegroundColor = (ConsoleColor)_color;
        }

        public static void ChaneScreenColor(ColorCode _color)
        {
            //배경 색 변경
            if (_color == ColorCode.None) Console.BackgroundColor = ConsoleColor.Black;
            else Console.BackgroundColor = (ConsoleColor)_color;
        }

        public static string[] LoadAllText(string _textFileName)
        {
            //프로젝트/bin/폴더를 거슬러 올라가 프로젝트 파일 안에 Text라는 폴더를 찾음
            string projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\.."));
            string file = Path.Combine(projectPath, "Text", $"{_textFileName}.txt");

            //파일 존재 유무
            if (!File.Exists(file))
            {
                Console.WriteLine($"Text폴더에 {file}라는 파일은 존재하지 않음");
                return default;
            }

            //\n 기준으로 분리
            var reader = new StreamReader(file);
            var data = reader.ReadToEnd();
            var text = data.Split('\n'); //,StringSplitOptions.RemoveEmptyEntries);

            return text;
        }

        public static void PrintTextFile(string _textFileName)
        {
            //해당 텍스트 파일 그대로 출력
            var text = LoadAllText(_textFileName);

            //로드가 안됐을 경우
            if(text == null)
            {
                Console.WriteLine($"{_textFileName}을 불러올 수 없음");
                return;
            }

            //텍스트 파일 내용 모두 출력
            for (int i = 0; i < text.Length; i++)
            {
                Console.WriteLine(text[i]);
            }
        }
    }

    public struct Vector
    {
        //저장을 위한 커스텀 벡터 구조체
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
        //벡터 저장 전용 클래스
        public Dictionary<string, Vector> data { get; set; } = new Dictionary<string, Vector>();

        public void Add(string _dataName, Vector3 _data)
        {
            if (data.ContainsKey(_dataName)) data[_dataName] = new Vector(_data.X, _data.Y, _data.Z);
            else data.Add(_dataName, new Vector(_data.X, _data.Y, _data.Z));
        }

        public Vector3 GetVector3(string _dataName)
        {
            if (data.ContainsKey(_dataName)) return data[_dataName].ToVector3();
            else return default;
        }

        public Vector2 GetVector2(string _dataName)
        {
            if (data.ContainsKey(_dataName)) return data[_dataName].ToVector2();
            else return default;
        }
    }

    public class Data<T>
    {
        //기본 데이터 타입 저장 전용 클래스
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
        public Data<string> stringMap { get; set; } = new Data<string>();
        public Vector32 vector { get; set; } = new Vector32();

        public void Save()
        {
            //최종적으로 게임 데이터를 저장할 경우에만 호출
            var jsonFile = new JsonSerializerOptions { WriteIndented = true };
            string toJson = JsonSerializer.Serialize(this, jsonFile);

            File.WriteAllText("SaveFile.json", toJson);
        }

        public void Load()
        {
            //이어하기 여부
            if (File.Exists("SaveFile.json"))
            {
                var loadFile = File.ReadAllText("SaveFile.json");

                if (!string.IsNullOrWhiteSpace(loadFile))
                {
                    var loadData = JsonSerializer.Deserialize<GameData>(loadFile);

                    integer = loadData.integer;
                    floating = loadData.floating;
                    boolen = loadData.boolen;
                    stringMap = loadData.stringMap;
                    vector = loadData.vector;
                }
            }
        }
    }
}
