using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace lab
{

    internal class Programm
    {

        public class BankUser
        {
            [Key]
            public int Id { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public double Money { get; set; }

        }


        public class ApplicationContext : DbContext
        {
            public DbSet<BankUser> BankUsers { get; set; }

            public ApplicationContext()
            {
                //Database.EnsureDeleted();
                Database.EnsureCreated();
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=sixDatabase;Trusted_Connection=True;");
            }
        }


        static void Main(string[] args)
        {

            using (ApplicationContext db = new ApplicationContext())
            {
                //BankUser user1 = new BankUser { FirstName = "Alex", LastName = "Lion", Money = 100000 };
                //BankUser user2 = new BankUser { FirstName = "Marty", LastName = "Zebra", Money = 925521.25 };

                //db.BankUsers.Add(user1);
                //db.BankUsers.Add(user2);
                //db.SaveChanges();

                
                ConsoleKeyInfo key;
                bool flag = true;
                while (flag)
                {
                    Console.Clear();
                    Console.WriteLine("\nВыберите опцию");
                    Console.WriteLine("\n1 - Посмотреть все записи таблицы\n2 - Удалить запись\n3 - Добавить новую запись" +
                        "\n4 - редактивровать запись\n0 - выйти из программы");
                    key = Console.ReadKey();
                    switch (key.KeyChar)
                    {
                        case '1':
                            Console.Clear();
                            ShowInformation();
                            Console.ReadKey();
                            break;
                        case '2':
                            Console.Clear();
                            DeleteBankUser();
                            Console.ReadKey();
                            break;
                        case '3':
                            Console.Clear();
                            AddBankUser();
                            Console.ReadKey();
                            break;
                        case '4':
                            Console.Clear();
                            UpdateBankUser();
                            Console.ReadKey();
                            break;
                        case '0':
                            flag = false;
                            break;

                    }
                }
                

                void ShowInformation()
                {
                    //Console.Clear();
                    foreach (BankUser u in db.BankUsers.ToList())
                    {
                        Console.WriteLine($"{u.Id}.{u.FirstName} {u.LastName} -- {u.Money}");
                    }
                    Console.WriteLine("Все пользователи показаны выше");
                }

                void DeleteBankUser()
                {
                    //Console.Clear();
                    Console.WriteLine("Какого из следующих пользователей вы хотите удалить? (Введите Id. Для выхода введите 0)");
                    ShowInformation();
                    string userType = Console.ReadLine();
                    int realId;

                    if (userType != "0")
                    {
                        if (!Int32.TryParse(userType, out realId) || !db.BankUsers.Where(bu => bu.Id == realId).Any())
                        {
                            while (!Int32.TryParse(userType, out realId) || !db.BankUsers.Where(bu => bu.Id == realId).Any() || userType != "0")
                            {
                                Console.WriteLine("Вы ввели неправильный Id");
                                userType = Console.ReadLine();
                            }
                        }

                        BankUser del = db.BankUsers.Where(bu => bu.Id == realId).First();
                        db.BankUsers.Remove(del);
                        db.SaveChanges();
                        Console.WriteLine("Пользователь был успешно удален. Для выхода нажмите любую клавишу");
                    }

                }


                void AddBankUser()
                {
                    BankUser user = new BankUser();
                    Console.WriteLine("Введите имя");
                    user.FirstName = Console.ReadLine();
                    Console.WriteLine("Введите фамилию");
                    user.LastName = Console.ReadLine();

                    Console.WriteLine("Введите сумму");
                    double m;
                    if (Double.TryParse(Console.ReadLine(), out m))
                    {
                        user.Money = m;
                    } else
                    {
                        user.Money = 0;
                    }
                    db.BankUsers.Add(user);
                    db.SaveChanges();
                    Console.WriteLine("Пользователь добавлен");
                }

                void UpdateBankUser()
                {
                    Console.WriteLine("Выберите поля какого пользователя хотите изменить (Введите Id. Для выхода введите 0)");
                    ShowInformation();
                    string userType = Console.ReadLine();
                    int realId;

                    if (userType != "0")
                    {
                        if (!Int32.TryParse(userType, out realId) || !db.BankUsers.Where(bu => bu.Id == realId).Any())
                        {
                            while (!Int32.TryParse(userType, out realId) || !db.BankUsers.Where(bu => bu.Id == realId).Any() || userType != "0")
                            {
                                Console.WriteLine("Вы ввели неправильный Id");
                                userType = Console.ReadLine();
                            }
                        }

                        BankUser changing = db.BankUsers.Where(bu => bu.Id == realId).First();

                        Console.WriteLine("Введите характеристику, которую хотите изменить:\n1 - Имя\n2 - Фамилия\n3 - Количество денег");
                        ConsoleKeyInfo userCh = Console.ReadKey();
                        switch (userCh.KeyChar)
                        {
                            case '1':
                                Console.WriteLine("\nВведите новое имя");
                                changing.FirstName = Console.ReadLine();
                                break;
                            case '2':
                                Console.WriteLine("\nВведите новую фамилию");
                                changing.LastName = Console.ReadLine();
                                break;
                            case '3':
                                Console.WriteLine("\nВведите новое количество денег");
                                double s;
                                if (Double.TryParse(Console.ReadLine(), out s))
                                {
                                    changing.Money = s;
                                }
                                break;
                            default:
                                break;

                        }

                    }
                    Console.WriteLine("Изменения применены");
                    db.SaveChanges();
                }

            }


        }

    }
  

}


