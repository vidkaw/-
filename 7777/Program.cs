﻿using System;
using System.Collections.Generic;

public class Рейс
{
    public string Номер { get; }
    public string Направление { get; }
    public DateTime ВремяВылета { get; }
    public decimal Стоимость { get; }
    public bool Забронирован { get; private set; }

    public Рейс(string номер, string направление, DateTime времяВылета, decimal стоимость)
    {
        Номер = номер;
        Направление = направление;
        ВремяВылета = времяВылета;
        Стоимость = стоимость;
        Забронирован = false;
    }

    public bool Забронировать()
    {
        if (Забронирован)
        {
            return false; // Рейс уже забронирован
        }
        Забронирован = true; // Бронируем рейс
        return true; // Успешная бронь
    }
}

public class Клиент
{
    public string Имя { get; }
    public string Фамилия { get; }

    public Клиент(string имя, string фамилия)
    {
        Имя = имя;
        Фамилия = фамилия;
    }
}

public class Администратор
{
    public void ПодтвердитьБронь(Клиент клиент, Рейс рейс)
    {
        Console.WriteLine($"Бронь на рейс {рейс.Номер} подтверждена для клиента {клиент.Имя} {клиент.Фамилия}.");
    }

    public void ВыставитьСчет(Клиент клиент, decimal сумма)
    {
        Console.WriteLine($"Счет для клиента {клиент.Имя} {клиент.Фамилия}: {сумма} рублей.");
    }

    public void ПодтвердитьОплату(Клиент клиент, decimal сумма)
    {
        Console.WriteLine($"Оплата в размере {сумма} рублей подтверждена для клиента {клиент.Имя} {клиент.Фамилия}.");
    }
}

public class МенеджерРейсов
{
    private List<Рейс> рейсы;

    public МенеджерРейсов()
    {
        рейсы = new List<Рейс>();
    }

    public void ДобавитьРейс(Рейс рейс)
    {
        рейсы.Add(рейс);
    }

    public void ПоказатьРейсы()
    {
        Console.WriteLine("Доступные рейсы:");
        for (int i = 0; i < рейсы.Count; i++)
        {
            Рейс рейс = рейсы[i];
            Console.WriteLine($"{i + 1}. {рейс.Номер} - {рейс.Направление}, Время вылета: {рейс.ВремяВылета}, Стоимость: {рейс.Стоимость}, Забронирован: {(рейс.Забронирован ? "Да" : "Нет")}");
        }
    }

    public Рейс GetРейс(int индекс)
    {
        if (индекс < 0 || индекс >= рейсы.Count)
        {
            throw new IndexOutOfRangeException("Некорректный индекс рейса.");
        }
        return рейсы[индекс];
    }
}

public class Программа
{
    public static void Main()
    {
        МенеджерРейсов менеджерРейсов = new МенеджерРейсов();

        // Добавляем несколько рейсов
        менеджерРейсов.ДобавитьРейс(new Рейс("AA123", "Нью-Йорк", new DateTime(2023, 12, 1, 15, 30, 0), 15000));
        менеджерРейсов.ДобавитьРейс(new Рейс("BA456", "Лондон", new DateTime(2023, 12, 5, 20, 0, 0), 20000));
        менеджерРейсов.ДобавитьРейс(new Рейс("CA789", "Париж", new DateTime(2023, 12, 10, 10, 15, 0), 18000));

        // Показываем доступные рейсы
        менеджерРейсов.ПоказатьРейсы();

        // Ввод имени и фамилии клиента
        Console.Write("Введите имя клиента: ");
        string имя = Console.ReadLine();
        Console.Write("Введите фамилию клиента: ");
        string фамилия = Console.ReadLine();

        Клиент клиент = new Клиент(имя, фамилия);

        List<Рейс> выбранныеРейсы = new List<Рейс>();
        decimal общийСчет = 0;

        while (true)
        {
            Console.Write("Выберите номер рейса для бронирования (введите номер или 'exit' для завершения выбора): ");
            string ввод = Console.ReadLine();
            if (ввод.ToLower() == "exit")
            {
                break; // Завершаем выбор
            }

            if (int.TryParse(ввод, out int номерРейса))
            {
                try
                {
                    Рейс выбранныйРейс = менеджерРейсов.GetРейс(номерРейса - 1);

                    if (выбранныйРейс.Забронировать())
                    {
                        выбранныеРейсы.Add(выбранныйРейс);
                        общийСчет += выбранныйРейс.Стоимость;
                        Console.WriteLine($"Рейс {выбранныйРейс.Номер} успешно забронирован.");
                    }
                    else
                    {
                        Console.WriteLine($"Не удалось забронировать рейс {выбранныйРейс.Номер}, он уже забронирован.");
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите номер рейса или 'exit'.");
            }
        }

        // Обработка выбранных рейсов
        Администратор администратор = new Администратор();

        foreach (var рейс in выбранныеРейсы)
        {
            администратор.ПодтвердитьБронь(клиент, рейс);
        }

        // Выставляем общий счет
        администратор.ВыставитьСчет(клиент, общийСчет);

        // Подтверждаем оплату
        администратор.ПодтвердитьОплату(клиент, общийСчет);

        // Пример завершения программы
        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}