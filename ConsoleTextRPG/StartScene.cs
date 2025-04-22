using GameCharacter;
using GameLogic;
using GameService;

namespace StartScene
{
    public class StartScenes
    {
        public static Job SelectJobScene()
        {
            //직업 추가
            Job[] isJob =
            {
                new Warrior(),
                new Wizard(),
            };

            var input = 0;
            var text = Mathod.LoadAllText("JobSelectionwindow");
            var selectJobText = Mathod.LoadAllText("SelectJobText");

            while (true)
            {
                Console.Clear();

                for (int i = 0; i < text.Length; i++)
                {
                    Console.WriteLine(text[i]);
                }

                Console.Write(">>");

                //키 입력 검사
                if (Mathod.CheckInput(out input))
                {
                    if (input < 1 || isJob.Length < input)
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }

                    else
                    {
                        input--;
                        break;
                    }
                }
            }

            Console.WriteLine($"\n{selectJobText[input]}");
            Thread.Sleep(1500);
            Console.Clear();

            return isJob[input];
        }

        public static string SetNameScene()
        {
            int valueInput = 0;
            string nameInput = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("원하시는 이름을 입력해주세요:");
                Console.Write(">> ");

                nameInput = Console.ReadLine();

                if (string.IsNullOrEmpty(nameInput))
                {
                    Console.WriteLine("최소 한 글자 이상 입력하셔야 합니다.");
                    Thread.Sleep(1000);
                    continue;
                }

                Console.WriteLine("\n1. 저장");
                Console.WriteLine("2. 취소");
                Console.Write(">>");

                if (Mathod.CheckInput(out valueInput))
                {
                    //저장시
                    if (valueInput == 1)
                    {
                        Console.Clear();
                        return nameInput;
                    }

                    //취소시 다시 루프
                    else if (valueInput == 2)
                    {
                        continue;
                    }

                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        public static void ShowStartText(string _playerName)
        {
            Console.WriteLine($"환영합니다, {_playerName}님!");
            Console.WriteLine("아무 키나 눌러 스파르타 마을로 이동합니다...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
