using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Transport.Data.Models;

namespace Transport.Data
{
    public class TransportDbContext : DbContext
    {
        public DbSet<BusStop> BusStops { get; set; }
        public DbSet<BusLine> BusLines { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TransportSystem.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusStop>().Property(s => s.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<BusLine>().Property(l => l.Number).IsRequired();

            modelBuilder.Entity<BusStop>().HasData(
                new BusStop { Id = 1, Name = "Терминал Изгрев" },
                new BusStop { Id = 2, Name = "Акве Калиде" },
                new BusStop { Id = 3, Name = "Добри Чинтулов" },
                new BusStop { Id = 4, Name = "Велека" },
                new BusStop { Id = 5, Name = "Никола Петков" },
                new BusStop { Id = 6, Name = "Зорница" },
                new BusStop { Id = 7, Name = "Стефан Стамболов" },
                new BusStop { Id = 8, Name = "Александровска" },
                new BusStop { Id = 9, Name = "Опера" },
                new BusStop { Id = 10, Name = "Терминал Юг" },
                new BusStop { Id = 11, Name = "Спортна" },
                new BusStop { Id = 12, Name = "Езеро Вая" },
                new BusStop { Id = 13, Name = "Захари Стоянов" },
                new BusStop { Id = 14, Name = "Резвая" },
                new BusStop { Id = 15, Name = "Бадемите" },
                new BusStop { Id = 16, Name = "Терминал М. Рудник" },
                new BusStop { Id = 17, Name = "Терминал Славейков" },
                new BusStop { Id = 18, Name = "Янко Комитов" },
                new BusStop { Id = 19, Name = "А. Страшимиров" },
                new BusStop { Id = 20, Name = "Тракия" },
                new BusStop { Id = 21, Name = "Младост" },
                new BusStop { Id = 22, Name = "Ст. Стамболов" },
                new BusStop { Id = 23, Name = "Т. М. Рудник" },
                new BusStop { Id = 24, Name = "Славейков, бл. 35" },
                new BusStop { Id = 25, Name = "Славейков, бл. 37" },
                new BusStop { Id = 26, Name = "У-тет Проф. Ас. Златаров" },
                new BusStop { Id = 27, Name = "Транспортна" },
                new BusStop { Id = 28, Name = "Изгрев, бл. 40" },
                new BusStop { Id = 29, Name = "Парк Езеро" },
                new BusStop { Id = 30, Name = "Стадион Лазур" },
                new BusStop { Id = 31, Name = "Копривщица" },
                new BusStop { Id = 32, Name = "К. Величков" },
                new BusStop { Id = 33, Name = "Сан Стефано" },
                new BusStop { Id = 34, Name = "Ген. Гурко" },
                new BusStop { Id = 35, Name = "Булаир" },
                new BusStop { Id = 36, Name = "Транспортна болница" },
                new BusStop { Id = 37, Name = "Иван Вазов" },
                new BusStop { Id = 38, Name = "Сливница" },
                new BusStop { Id = 39, Name = "Мария Луиза" },
                new BusStop { Id = 40, Name = "Автогара Запад" },
                new BusStop { Id = 41, Name = "Одрин" },
                new BusStop { Id = 42, Name = "Трапезица" },
                new BusStop { Id = 43, Name = "Струга" },
                new BusStop { Id = 44, Name = "Славейков, бл. 1" },
                new BusStop { Id = 45, Name = "Лазар Маджаров" },
                new BusStop { Id = 46, Name = "Славейков, бл. 9" },
                new BusStop { Id = 47, Name = "Славейков, бл. 16" },
                new BusStop { Id = 48, Name = "Славейков, бл. 30" },
                new BusStop { Id = 49, Name = "Славейков, бл. 31" },
                new BusStop { Id = 50, Name = "24-ти пехотен полк" },
                new BusStop { Id = 51, Name = "Изгрев, бл. 39" },
                new BusStop { Id = 52, Name = "МБАЛ Сърце и мозък" },
                new BusStop { Id = 53, Name = "Автогара Юг" },
                new BusStop { Id = 54, Name = "Дунав" },
                new BusStop { Id = 55, Name = "Демокрация" },
                new BusStop { Id = 56, Name = "Изгрев, бл. 3" },
                new BusStop { Id = 57, Name = "Гробищен парк" },
                new BusStop { Id = 58, Name = "Летище Бургас" },
                new BusStop { Id = 59, Name = "Сарафово, Октомври" },
                new BusStop { Id = 60, Name = "Сарафово, Драва" },
                new BusStop { Id = 61, Name = "Сарафово, Брацигово" },
                new BusStop { Id = 62, Name = "Сарафово, Ради Николов" },
                new BusStop { Id = 63, Name = "Сарафово, Лазурна" },
                new BusStop { Id = 64, Name = "Сарафово, А. Димитров" },
                new BusStop { Id = 65, Name = "Антон Страшимиров" },
                new BusStop { Id = 66, Name = "Капчето" },
                new BusStop { Id = 67, Name = "Тополица" },
                new BusStop { Id = 68, Name = "Въстаническа" },
                new BusStop { Id = 69, Name = "Найден Геров" },
                new BusStop { Id = 70, Name = "М. рудник, бл. 485" },
                new BusStop { Id = 71, Name = "М. рудник, бл. 101" },
                new BusStop { Id = 72, Name = "Терминал М. рудник" }
                );
            modelBuilder.Entity<BusLine>().HasData(
                new BusLine { Id = 1, Number = "Б1" },
                new BusLine { Id = 2, Number = "Б2" },
                new BusLine { Id = 3, Number = "Б11" },
                new BusLine { Id = 4, Number = "Б12" },
                new BusLine { Id = 5, Number = "11" },
                new BusLine { Id = 6, Number = "12" },
                new BusLine { Id = 7, Number = "15" },
                new BusLine { Id = 8, Number = "Нощна Линия" }
                );
        }
    }
}
