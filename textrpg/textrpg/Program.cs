using System.Security.Cryptography.X509Certificates;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace textrpg
{
    internal class Program
    {
        
        class Start
        {
            public void StartInfo()//게임 시작 화면
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전 입장\n5. 휴식하기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
            }

            public int SelectWay()//시작 화면 행동 선택
            {
                int select = 0;
                bool selecting = true;
                while (selecting == true)
                {
                    string select1 = Console.ReadLine();

                    if(int.TryParse(select1, out select))
                    {
                        if(select == 1 || select ==2 || select ==3 || select ==4 || select ==5)
                        {
                            selecting = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");

                    }
                }

                Console.Clear();
                return select;
            }
        }

        [Serializable]
        public class Character : Inventory
        {
            public float attack_extra=0f;
            public float defense_extra=0f;
            public float health_extra=0;
            public float maxHP = 100;

            public struct Character_struct
            {
                public int level;
                public string name;
                public string job;
                public float attack;
                public float defense;
                public float health;
                public int gold;
            }

            public Character_struct StructInstance;

            public Character()
            {
              
                StructInstance = new Character_struct();
                StructInstance.level = 01;
                StructInstance.name = "Chad";
                StructInstance.job = "전사";
                attack_extra = InvenAtt_extra;
                StructInstance.attack = 10 + attack_extra;
                defense_extra = InvenDef_extra;
                StructInstance.defense = 5 + defense_extra;
                health_extra = InvenHea_extra;
                StructInstance.health = 100 + health_extra;
                StructInstance.gold = 1500;
            }

            public void Update()
            {
                float currentHP = StructInstance.health;
                float currentHP_Ex = health_extra;

                for (int i=0; i<dataMax; i++ )
                {
                    if (InvenEquip[i] == "E")
                    {
                        if (InvenList[i].InvenAtt > 0)
                        {
                            attack_extra += InvenList[i].InvenAtt;
                        }
                        else if (InvenList[i].InvenDef > 0)
                        {
                            defense_extra += InvenList[i].InvenDef;
                        }
                        else if(InvenList[i].InvenHea >0)
                        {
                            health_extra += InvenList[i].InvenHea;
                        }
                    }
                }

                maxHP = 100 + health_extra;
                StructInstance.health = currentHP - currentHP_Ex + health_extra;
                StructInstance.attack = 10 + attack_extra;
                StructInstance.defense = 5 + defense_extra;
            }


            public void CharacterDisplay()
            {
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시 됩니다\n");
                
                Console.WriteLine($"Lv. {StructInstance.level}");
                Console.WriteLine($"{StructInstance.name} ( {StructInstance.job} )");
                Console.WriteLine($"공격력 : {StructInstance.attack} ({attack_extra})");
                Console.WriteLine($"방어력 : {StructInstance.defense} ({defense_extra})");
                Console.WriteLine($"체 력 : {StructInstance.health} ({health_extra})");
                Console.WriteLine($"Gold : {StructInstance.gold} G");
                Console.WriteLine("\n 0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
            }


            public bool Exit()
            {
                int exitNumb = 5;
                
                string exitcode = Console.ReadLine();

                if(int.TryParse(exitcode, out exitNumb))
                {
                    if(exitNumb ==0)
                    {
                        Console.Clear();
                        return true;
                    }
                    else
                    {
                        Console.Clear();
                        return false;
                    }
                }
                else
                {
                    Console.Clear();
                    return false;
                }

            }

        }

        public class Inventory : ItemStore
        {
            public float InvenAtt_extra = 0;
            public float InvenDef_extra = 0;
            public float InvenHea_extra = 0;
            public int dataMax = 0;
            public int typeD_count = 0;
            public int typeW_count = 0;
            public string[] InvenEquip= new string[50];
            public struct Inven_Struct
            {
                public string InvenName;
                public char InvenType;
                public float InvenAtt;
                public float InvenDef;
                public float InvenHea;
                public string InvenInfo;

                public Inven_Struct(string a, char b, float c, float d, float e, string f)
                {
                    InvenName = a;
                    InvenType = b;
                    InvenAtt = c;
                    InvenDef = d;
                    InvenHea = e;
                    InvenInfo = f;
                }


            }

            public List<Inven_Struct> InvenList = new List<Inven_Struct>();

            public Inventory()//기본 생성자
            {
                
            }

            public void InvenInitial()
            {
                for (int i = 0; i < ItemMax; i++)
                {
                    if (Purchase[i] == true)
                    {
                        InvenList.Add(new Inven_Struct(StoreList[i].ItemName,StoreList[i].ItemType, StoreList[i].ItemAtt, StoreList[i].ItemDef, StoreList[i].ItemHea, StoreList[i].ItemInfo));
                        InvenEquip[dataMax] = null;
                        dataMax++;
                    }
                }
            }

            public void AddLast(int numb)
            {
                InvenList.Add(new Inven_Struct(StoreList[numb-1].ItemName, StoreList[numb-1].ItemType, StoreList[numb - 1].ItemAtt, StoreList[numb - 1].ItemDef, StoreList[numb - 1].ItemHea, StoreList[numb - 1].ItemInfo));
                dataMax++;
                InvenEquip[dataMax - 1] = null;
            }

            public void MinusLast(int numb)
            {
                int check = 0;
                for(int i=0; i<dataMax; i++)
                {
                    if(InvenList[i].InvenName == StoreList[numb - 1].ItemName)
                    {
                        check = i;
                        break;
                    }
                }

                InvenList.Remove(InvenList[check]);
                InvenEquip[check] = null;

                for(int j=check; j<dataMax-1; j++)
                {
                    InvenEquip[check] = InvenEquip[check+1];
                }
                dataMax--;
            }

            public void InvenDisplay()
            {
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다\n");
                Console.WriteLine("[아이템 목록]\n");

                for(int i=0; i < dataMax; i++)
                {
                    if (InvenList[i].InvenName == null)
                    {
                        break;
                    }

                    Console.Write($"-[{InvenEquip[i]}]{InvenList[i].InvenName}\t|");
                    if (InvenList[i].InvenAtt >0)
                    {
                        Console.Write($"공격력 +{InvenList[i].InvenAtt}|");
                    }
                    else if (InvenList[i].InvenAtt < 0)
                    {
                        Console.Write($"공격력 {InvenList[i].InvenAtt}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }

                    if (InvenList[i].InvenDef >0)
                    {
                        Console.Write($"방어력 +{InvenList[i].InvenDef}|");
                    }
                    else if (InvenList[i].InvenDef < 0)
                    {
                        Console.Write($"방어력 {InvenList[i].InvenDef}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }

                    if (InvenList[i].InvenHea > 0)
                    {
                        Console.Write($"체력 +{InvenList[i].InvenHea}|");
                    }
                    else if (InvenList[i].InvenHea < 0)
                    {
                        Console.Write($"체력 {InvenList[i].InvenHea}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }


                    Console.WriteLine($"{InvenList[i].InvenInfo}");

                }
                Console.WriteLine("");
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입려해주세요");

            }

            public void ItemDisplay(int a)
            {
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다\n");
                Console.WriteLine("[아이템 목록]\n");

                for (int i = 0; i < dataMax; i++)
                {
                    if (string.IsNullOrEmpty(InvenList[i].InvenName))
                    {
                        break;
                    }

                    Console.Write($"-{i + 1} [{InvenEquip[i]}]{InvenList[i].InvenName}\t|");
                    if (InvenList[i].InvenAtt > 0)
                    {
                        Console.Write($"공격력 +{InvenList[i].InvenAtt}|");
                    }
                    else if (InvenList[i].InvenAtt < 0)
                    {
                        Console.Write($"공격력 {InvenList[i].InvenAtt}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }

                    if (InvenList[i].InvenDef > 0)
                    {
                        Console.Write($"방어력 +{InvenList[i].InvenDef}|");
                    }
                    else if (InvenList[i].InvenDef < 0)
                    {
                        Console.Write($"방어력 {InvenList[i].InvenDef}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }

                    if (InvenList[i].InvenHea > 0)
                    {
                        Console.Write($"체력 +{InvenList[i].InvenHea}|");
                    }
                    else if (InvenList[i].InvenHea < 0)
                    {
                        Console.Write($"체력 {InvenList[i].InvenHea}|");
                    }
                    else
                    {
                        Console.Write("\t");
                    }

                    Console.WriteLine($"{InvenList[i].InvenInfo}");
                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입려해주세요");
            }


            public int InvenSelect()
            {
                int exitNumb=9999;

                string exitcode = Console.ReadLine();

                if (int.TryParse(exitcode, out exitNumb))
                {
                    if (exitNumb == 0)
                    {
                        Console.Clear();
                        return exitNumb;
                    }
                    else if(exitNumb == 1)
                    {
                        Console.Clear();
                        return exitNumb;
                    }
                    else
                    {
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.Clear();
                    return 9999;
                }

            }

            public bool InvenSelect(int a)
            {
                int exitNumb=9999;

                string exitcode = Console.ReadLine();

                if (int.TryParse(exitcode, out exitNumb))
                {
                    if (exitNumb == 0)
                    {
                        Console.Clear();
                        return true;
                    }
                    else
                    {
                        if(exitNumb>dataMax || exitNumb<0)
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return false;
                        }
                        else if(exitNumb >=1 && InvenEquip[exitNumb-1] == "E")
                        {
                            InvenEquip[exitNumb-1] = null;
                            if (InvenList[exitNumb-1].InvenType == 'D')
                            {
                                typeD_count--;
                            }
                            else if (InvenList[exitNumb-1].InvenType == 'W')
                            {
                                typeW_count--;
                            }

                        }
                        else if(exitNumb >= 1 && InvenEquip[exitNumb - 1] == null)
                        {
                            if(InvenList[exitNumb - 1].InvenType == 'D')
                            {
                                if(typeD_count == 1)
                                {
                                    Console.WriteLine("1개의 갑옷만 장착하실 수 있습니다");
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    InvenEquip[exitNumb - 1] = "E";
                                    typeD_count++;
                                }     
                            }
                            else if(InvenList[exitNumb - 1].InvenType == 'W')
                            {
                                if(typeW_count == 1)
                                {
                                    Console.WriteLine("1개의 무기만 장착하실 수 있습니다");
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    InvenEquip[exitNumb - 1] = "E";
                                    typeW_count++;
                                } 
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(1000);
                        }
                        Console.Clear();
                        return false;
                    }
                }
                else
                {
                    Console.Clear();
                    return false;
                }
            }
        }


        public class ItemStore
        {
            public int ItemMax = 6;
            public int HaveGold;
            public bool[] Purchase = new bool[100];
            
            public ItemStore()
            {
                Purchase[0] = true;
                Purchase[1] = false;
                Purchase[2] = false;
                Purchase[3] = true;
                Purchase[4] = false;
                Purchase[5] = false;

            }

            public struct Store_Struct
            {
                public string ItemName;
                public char ItemType;
                public float ItemAtt;
                public float ItemDef;
                public float ItemHea;
                public string ItemInfo;
                public int ItemGold;
                public char ItemGoldCurrency;

                public Store_Struct(string a, char b, float c, float d, float e, string f, int g, char h)
                {
                    ItemName =a;
                    ItemType = b;
                    ItemAtt=c;
                    ItemDef=d;
                    ItemHea=e;
                    ItemInfo=f;
                    ItemGold=g;
                    ItemGoldCurrency=h;
                }

            }

            public List<Store_Struct> StoreList = new List<Store_Struct>
            {
                new Store_Struct { ItemName = "수련자 갑옷",ItemType='D', ItemAtt= 0, ItemDef = 5f, ItemHea=0, ItemInfo="수련에 도움을 주는 갑옷입니다.", ItemGold=1000, ItemGoldCurrency='G'},
                new Store_Struct { ItemName = "무쇠 갑옷", ItemType='D',ItemAtt= 0, ItemDef =9f, ItemHea=0, ItemInfo="무쇠로 만들어진 튼튼한 갑옷입니다.", ItemGold=2000, ItemGoldCurrency='G'},
                new Store_Struct { ItemName = "스파르타의 갑옷",ItemType='D', ItemAtt= 0, ItemDef =15f, ItemHea=0, ItemInfo="스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", ItemGold=3500, ItemGoldCurrency='G'},
                new Store_Struct { ItemName = "낡은 검", ItemType='W',ItemAtt= 2f, ItemDef =0, ItemHea=0, ItemInfo="쉽게 볼 수 있는 낡은 검 입니다.", ItemGold=600, ItemGoldCurrency='G'},
                new Store_Struct { ItemName = "청동 도끼", ItemType='W',ItemAtt= 5f, ItemDef =0, ItemHea=0, ItemInfo="어디선가 사용됐던거 같은 도끼입니다.", ItemGold=2000, ItemGoldCurrency='G' },
                new Store_Struct { ItemName = "스파르타의 창",ItemType='W', ItemAtt= 7f, ItemDef =0, ItemHea=0, ItemInfo="스파르타의 전사들이 사용했다는 전설의 창입니다", ItemGold=3500, ItemGoldCurrency='G' }
            };
            

            public void StoreDisplay(int gold)//첫 상점 화면
            {

                HaveGold = gold;
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gold} G\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < ItemMax; i++)
                {

                    Console.Write($"-{StoreList[i].ItemName}\t|");
                    if (StoreList[i].ItemAtt > 0)
                    {
                        Console.Write($" 공격력 +{StoreList[i].ItemAtt}|");
                    }
                    else if (StoreList[i].ItemAtt < 0)
                    {
                        Console.Write($" 공격력 {StoreList[i].ItemAtt}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (StoreList[i].ItemDef > 0)
                    {
                        Console.Write($"방어력 +{StoreList[i].ItemDef}|");
                    }
                    else if (StoreList[i].ItemDef < 0)
                    {
                        Console.Write($"방어력 {StoreList[i].ItemDef}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (StoreList[i].ItemHea > 0)
                    {
                        Console.Write($"체력 +{StoreList[i].ItemHea}|");
                    }
                    else if (StoreList[i].ItemHea < 0)
                    {
                        Console.Write($"체력 {StoreList[i].ItemHea}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (Purchase[i] ==true)
                    {
                        Console.WriteLine($" {StoreList[i].ItemInfo}\t| \t 구매완료");
                    }
                    else
                    {
                        Console.WriteLine($" {StoreList[i].ItemInfo}\t| {StoreList[i].ItemGold} {StoreList[i].ItemGoldCurrency}");
                    }
                    
                }
                Console.WriteLine("");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입려해주세요");
            }

            public void StoreDisplay(int numb, int gold)//아이템 구매화면
            {
                HaveGold = gold;
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gold} G\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < ItemMax; i++)
                {
                    if (string.IsNullOrEmpty(StoreList[i].ItemName))
                    {
                        break;
                    }

                    Console.Write($"-{i+1} {StoreList[i].ItemName}\t|");
                    if (StoreList[i].ItemAtt > 0)
                    {
                        Console.Write($" 공격력 +{StoreList[i].ItemAtt}|");
                    }
                    else if (StoreList[i].ItemAtt < 0)
                    {
                        Console.Write($" 공격력 {StoreList[i].ItemAtt}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (StoreList[i].ItemDef > 0)
                    {
                        Console.Write($"방어력 +{StoreList[i].ItemDef}|");
                    }
                    else if (StoreList[i].ItemDef < 0)
                    {
                        Console.Write($"방어력 {StoreList[i].ItemDef}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (StoreList[i].ItemHea > 0)
                    {
                        Console.Write($"체력 +{StoreList[i].ItemHea}|");
                    }
                    else if (StoreList[i].ItemHea < 0)
                    {
                        Console.Write($"체력 {StoreList[i].ItemHea}|");
                    }
                    else
                    {
                        Console.Write("");
                    }

                    if (Purchase[i] == true)
                    {
                        Console.WriteLine($" {StoreList[i].ItemInfo}\t| \t 구매완료");
                    }
                    else
                    {
                        Console.WriteLine($" {StoreList[i].ItemInfo}\t| {StoreList[i].ItemGold} {StoreList[i].ItemGoldCurrency}");
                    }

                }
                Console.WriteLine("");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입려해주세요");
            }

            public int Storeselect()//아이템 리스트 화면 내 선택지
            {
                string answer = Console.ReadLine();
                int numb;
                if(int.TryParse(answer, out numb))
                {
                    if(numb==1 || numb ==0 || numb==2)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return 9999;
                }
            }

            public int Storeselect(int gold)//아이템 구매 화면 내 선택지
            {
                string answer = Console.ReadLine();
                int numb;
                if (int.TryParse(answer, out numb))
                {
                    if (numb == 0)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else if(numb >0 && numb <= ItemMax)
                    {
                        if (Purchase[numb-1] == true)
                        {
                            Console.WriteLine("이미 구매한 아이템입니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return 9999;
                        }
                        else if(gold - StoreList[numb-1].ItemGold >= 0)
                        {
                            HaveGold = gold - StoreList[numb - 1].ItemGold;
                            Purchase[numb - 1]=true;
                            Console.WriteLine("구매를 완료했습니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return numb;
                        }
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return 9999;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return 9999;
                }
            }

            public int Storesell(int gold)
            {
                string select = Console.ReadLine();
                int numb;
                if (int.TryParse(select, out numb))
                {
                    if (numb == 0)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else if (numb > 0 && numb <= ItemMax)
                    {
                        if (Purchase[numb - 1]==true)
                        {
                            Console.WriteLine($"판매 가격은 구매 가격의 85%인 {StoreList[numb-1].ItemGold * 85/100}G입니다");
                            HaveGold = gold + StoreList[numb - 1].ItemGold * 85 / 100;
                            Purchase[numb - 1] = false;
                            Thread.Sleep(1000);
                            Console.Clear();
                            return numb;
                        }
                        else
                        {
                            Console.WriteLine("보유하신 아이템이 아닙니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return 9999;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(1000);
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    Thread.Sleep(1000);
                    Console.Clear();
                    return 9999;
                }
            }
        }

        public class Dungeon : Character
        {
            public int stage_level = 0;
            public string level;
            public float currentHP;
            public float lastHP;
            public int currentGold;
            public int lastGold;
            public int currentlevel;
            public float currentAtt;
            public float currentDef;

            public Dungeon()
            {
               
            }

            public void DunDisplay()
            {
                Console.WriteLine("던전 입장");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다\n");
                Console.WriteLine("1. 쉬운 던전\t | 방어력 5이상 권장");
                Console.WriteLine("2. 일반 던전\t | 방어력 11이상 권장");
                Console.WriteLine("3. 어려운 던전\t | 방어력 17이상 권장");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");

            }

            public int DunPlaySelect()
            {
                string select = Console.ReadLine();
                int numb;
                if(int.TryParse(select, out numb))
                {
                    if(numb ==0 || numb ==1 || numb ==2 || numb==3)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else
                    {
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.Clear();
                    return 9999;
                }
            }

            public void DunResult(int number)
            {
                float recoDef=0;
                int resultGold=0;

                if(number == 1)
                {
                    recoDef = 5;
                    resultGold = 1000;
                    level = "쉬운";
                    
                }
                else if(number ==2)
                {
                    recoDef = 11;
                    resultGold = 1700;
                    level = "일반";
                }
                else if(number ==3)
                {
                    recoDef = 17;
                    resultGold = 2500;
                    level = "어려움";
                }

                float def = StructInstance.defense - recoDef;

                Random rand = new Random();
                double minValue = 20 - def;
                double maxValue = 36 - def;
                float damage = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
                minValue = StructInstance.attack;
                maxValue = StructInstance.attack * 2;
                float extra = (float)(minValue + (maxValue - minValue) * rand.NextDouble());
                int goldplus = (int)(resultGold + resultGold * extra/100);
                currentHP = StructInstance.health;
                lastHP = StructInstance.health - damage;
                StructInstance.health = lastHP;
                currentGold = StructInstance.gold;
                lastGold = StructInstance.gold + goldplus;
                currentlevel = StructInstance.level;
                currentAtt = StructInstance.attack;
                currentDef = StructInstance.defense;

                if(StructInstance.health >0)
                {
                    StructInstance.attack += 0.5f;
                    StructInstance.defense++;
                    StructInstance.health = lastHP;
                    StructInstance.gold = lastGold;
                    StructInstance.level++;
                }
                else
                {
                    StructInstance.health = 0;
                }
                


            }

            public void DunResultDisplay()
            {

                if (StructInstance.health > 0)
                {
                    Console.WriteLine("던전 클리어");
                    Console.WriteLine($"축하합니다!!\n{level} 던전을 클리어 하였습니다\n");
                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"체력 {currentHP} -> {lastHP}");
                    Console.WriteLine($"Gold {currentGold}G -> {lastGold}G\n");

                    Console.WriteLine("[레벨업]");
                    Console.WriteLine($"level {currentlevel} -> {currentlevel+1}");
                    Console.WriteLine($"공격력 {currentAtt} -> {currentAtt+0.5}");
                    Console.WriteLine($"방어력 {currentDef} -> {currentDef+1}");
                    Console.WriteLine($"체력 {lastHP} -> {lastHP}\n");

                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요");

                    

                }
                else
                {
                    Console.WriteLine("던전 실패");
                    Console.WriteLine($"아쉽게도!!\n{level} 던전을 실패하셨습니다\n");
                    Console.WriteLine("[탐험 결과]");
                    Console.WriteLine($"level {currentlevel} -> {currentlevel}");
                    Console.WriteLine($"체력 {currentHP} -> 0");
                    Console.WriteLine($"Gold {currentGold}G -> {currentGold}G\n");
                    Console.WriteLine("0. 나가기\n");
                    Console.WriteLine("원하시는 행동을 입력해주세요");
                }
            }

            public int DunResultSelect()
            {
                string select = Console.ReadLine();
                int numb;
                if (int.TryParse(select, out numb))
                {
                    if (numb == 0)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else
                    {
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.Clear();
                    return 9999;
                }
            }
        }

        public class Rest : Character
        {
            public Rest()
            {
                
            }

            public void Display(int gold)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine($"500G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {gold}G)\n");
                Console.WriteLine("1. 휴식하기\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");
            }

            public int select(int gold)
            {
                string select = Console.ReadLine();
                int numb;
                if (int.TryParse(select, out numb))
                {
                    if (numb == 1 || numb == 0)
                    {
                        if(numb == 1 && gold-500 >= 0)
                        {
                            Console.Clear();
                            return numb;
                        }
                        else if(numb == 0)
                        {
                            Console.Clear();
                            return numb;
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return 9999;
                        }

                    }
                    else
                    {
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.Clear();
                    return 9999;
                }
            }

            public void restOK(float currentHP, float maxHP)
            {
 
                Console.WriteLine("휴식 결과");
                Console.WriteLine("휴식을 완료했습니다\n");
                Console.WriteLine($"체력 {currentHP} -> {maxHP}\n");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요");

            }

            public int restAnother()
            {
                string select = Console.ReadLine();
                int numb;
                if (int.TryParse(select, out numb))
                {
                    if (numb == 0)
                    {
                        Console.Clear();
                        return numb;
                    }
                    else
                    {
                        Console.Clear();
                        return 9999;
                    }
                }
                else
                {
                    Console.Clear();
                    return 9999;
                }
            }
        }


        static void Main(string[] args)
        {
            Start newS = new Start(); //초기화면 클래스
            Character newcharacter = new Character();//캐릭터 상태, 인벤토리 상속, 상점 상속
            Dungeon newDun = new Dungeon();//던전 클래스, 별개 캐릭터 클래스 상속
            Rest newrest = new Rest();//휴식 클래스, 별개 캐릭터 클래스 상속

            newcharacter.InvenInitial();


            bool isPlaying = true;
            bool exit = false;
            int selection;
            int Firstselection = 0;
           


            while (isPlaying)
            {
                newS.StartInfo();
                Firstselection = newS.SelectWay();

                if (Firstselection == 1)
                {
                    while (exit == false)
                    {
                        newcharacter.CharacterDisplay();
                        exit = newcharacter.Exit();
                    }
                    exit = false;
                }
                else if(Firstselection ==2)
                {
                    while (exit == false)
                    {
                        newcharacter.InvenDisplay();
                        selection = newcharacter.InvenSelect();

                        if(selection == 1)
                        {
                            bool instance = false;
                            while(instance == false)
                            {
                                newcharacter.ItemDisplay(selection);
                                instance = newcharacter.InvenSelect(selection);
                            }
                            newcharacter.Update();
                            
                        }
                        else if(selection == 0)
                        {
                            break;
                        }

                    }
                }
                else if(Firstselection ==3)
                {
                    while(exit == false)
                    {
                        int goldInstance = newcharacter.StructInstance.gold;
                        newcharacter.StoreDisplay(goldInstance);
                        selection = newcharacter.Storeselect();

                        if(selection == 1)
                        {
                            while(selection != 0)
                            {
                                newcharacter.StoreDisplay(selection, goldInstance);
                                selection = newcharacter.Storeselect(goldInstance);

                                if(selection ==9999)
                                {
                                    continue;
                                }
                                else if(selection ==0)
                                {
                                    goldInstance = newcharacter.HaveGold;
                                    break;
                                }
                                else
                                {
                                    newcharacter.AddLast(selection);
                                }  

                                goldInstance = newcharacter.HaveGold;
                            }
                            newcharacter.StructInstance.gold = goldInstance;

                        }
                        else if(selection == 2)
                        {
                            while (selection != 0)
                            {
                                newcharacter.StoreDisplay(selection, goldInstance);
                                selection = newcharacter.Storesell(goldInstance);

                                if (selection == 9999)
                                {
                                    continue;
                                }
                                else if(selection ==0)
                                {
                                    goldInstance = newcharacter.HaveGold;
                                    break;
                                }
                                else
                                {
                                    newcharacter.MinusLast(selection);
                                }

                                goldInstance = newcharacter.HaveGold;

                            }
                            newcharacter.StructInstance.gold = goldInstance;
                            newcharacter.Update();

                        }
                        else if(selection == 0)
                        {
                            break;
                        }
                    }
                }
                else if(Firstselection==4)
                {
                    while(exit == false)
                    {
                        newDun.DunDisplay();
                        selection = newDun.DunPlaySelect();

                        if(selection ==0)
                        {
                            break;
                        }
                        else if(selection == 1 || selection ==2 || selection ==3)
                        {
                            newDun.HaveGold = newcharacter.HaveGold;
                            newDun.StructInstance.health = newcharacter.StructInstance.health;
                            newDun.StructInstance.level = newcharacter.StructInstance.level;
                            newDun.StructInstance.attack = newcharacter.StructInstance.attack;
                            newDun.StructInstance.defense = newcharacter.StructInstance.defense;
                            newDun.StructInstance.gold = newcharacter.StructInstance.gold;

                            newDun.DunResult(selection);
                            while(selection != 0)
                            {
                                newDun.DunResultDisplay();
                                selection = newDun.DunResultSelect();
                            }
                            newDun.HaveGold = newDun.StructInstance.gold;
                            newcharacter.HaveGold = newDun.HaveGold;
                            newcharacter.StructInstance.gold = newDun.StructInstance.gold;
                            newcharacter.StructInstance.level = newDun.StructInstance.level;
                            newcharacter.StructInstance.health = newDun.StructInstance.health;
                            newcharacter.StructInstance.attack = newDun.StructInstance.attack;
                            newcharacter.StructInstance.defense = newDun.StructInstance.defense;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else if(Firstselection == 5)
                {
                    while(exit == false)
                    {
                        newrest.Display(newcharacter.StructInstance.gold);
                        selection = newrest.select(newcharacter.StructInstance.gold);

                        if (selection == 0)
                        {
                            break;
                        }
                        else if (selection == 1)
                        {
                            int gold1 = newcharacter.StructInstance.gold;
                            newcharacter.StructInstance.gold = gold1 - 500;
                            while(true)
                            {
                                newrest.restOK(newcharacter.StructInstance.health, newcharacter.maxHP);
                                int numblast = newrest.restAnother();
                                newcharacter.StructInstance.health = newcharacter.maxHP;
                                if(numblast == 0)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                //데이터 저장
            }
            
        }
    }
}
