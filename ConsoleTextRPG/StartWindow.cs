using GameLogic;
using GameService;
using Manager;

namespace StartScene
{
    public class StartScenes
    {
        private static Job SelectJobScene()
        {
            var input = 0;
            var selectJobText = Mathod.LoadAllText("SelectJobText");
            var text = new string[selectJobText.Length];

            for (int i = 0; i < text.Length; i++)
            {
                var jobText = selectJobText[i].Split('/', '}');
                text[i] = jobText[1];
            }

            while (true)
            {
                Console.Clear();
                Mathod.FontColorOnce("직업을 선택해주세요 !\n\n", ColorCode.Yellow);

                for (int i = 0; i < text.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {text[i]}");
                }

                Console.Write("\n>>");

                //키 입력 검사
                if (Mathod.CheckInput(out input))
                {
                    if (input < 1 || selectJobText.Length < input)
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

            var tempText = selectJobText[input].Split('{', '}');

            Console.WriteLine($"\n{tempText[2]}");
            Thread.Sleep(1500);
            Console.Clear();

            return Mathod.JobToClass(input);
        }

        private static string SetNameScene()
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

                Mathod.MenuFont("\n1", "저장");
                Mathod.MenuFont("\n2", "취소");
                Console.Write("\n>>");

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

        public static void ShowStartText()
        {
            Console.Clear();
            Console.Write("환영합니다, ");

            Mathod.FontColorOnce($"{GameManager.player.name}", ColorCode.Blue);
            Console.WriteLine("님 !!");

            Mathod.FontColorOnce("아무 키", ColorCode.DarkGray);
            Console.WriteLine("나 눌러 스파르타 마을로 이동합니다...");

            Console.ReadKey();
            Console.Clear();
        }

        public static void LoadScene()
        {
            var input = 0;

            if (File.Exists("SaveFile.json"))
            {
                while (true)
                {
                    Console.Clear();
                    Mathod.ChangeFontColor(ColorCode.Green);
                    Console.WriteLine($"현재 데이터 파일이 존재합니다.");
                    Console.WriteLine($"파일을 불러오시겠습니까?\n");
                    Mathod.ChangeFontColor(ColorCode.None);

                    Mathod.MenuFont("1", "네.\n");
                    Mathod.MenuFont("2", "아니오.\n", ColorCode.DarkGray);

                    Console.Write(">>");

                    if (Mathod.CheckInput(out input))
                    {
                        if (input == 1)
                        {
                            GameManager.SpawnPlayer();
                            break;
                        }

                        else if (input == 2)
                        {
                            var nickName = SetNameScene();
                            var player = SelectJobScene();

                            GameManager.SpawnPlayer(nickName, player);
                            break;
                        }

                        else
                        {
                            Console.WriteLine("\n잘못된 입력입니다.");
                            Thread.Sleep(1000);
                        }
                    }
                }
            }

            else
            {
                var nickName = SetNameScene();
                var player = SelectJobScene();

                GameManager.SpawnPlayer(nickName, player);
            }
        }
    }
}
