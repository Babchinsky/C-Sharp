﻿using System;
using System.Collections.Generic;

namespace Dictionary_3._0
{
    // класс для хранения словаря на определенном языке
    public class Dictionary
    {
        public string Language { get; set; }  // язык словаря
        public List<Word> Words { get; set; }  // список слов с их переводами

        public Dictionary(string language)
        {
            Language = language;
            Words = new List<Word>();
        }

        // добавление слова и его переводов в словарь
        public void AddWord(string term, List<string> translations)
        {
            Words.Add(new Word(term, translations));
        }

        // удаление слова из словаря
        public void RemoveWord(string term)
        {
            foreach (Word word in Words)
            {
                if (term == word.Term)
                {
                    Words.Remove(word);
                    return;
                }
            }
            throw new Exception("Слово не найдено");
        }

        // удаление перевода у слова в словаре (нельзя удалить единственный вариант перевода)
        public void RemoveTranslation(string term, string tran)
        {
            foreach (Word word in Words)
            {
                if (word.Term == term)
                {
                    word.RemoveTranslation(tran);
                    return;
                }
            }
            Console.WriteLine("Слово в словаре не найдено");
            //else throw new Exception("Слово в словаре не найдено");
        }

        public void AddTranslation(string term, string tran)
        {
            foreach (Word word in Words)
            {
                if (word.Term == term)
                {
                    word.AddTranslation(tran);
                    return;
                }
            }
            Console.WriteLine("Слово в словаре не найдено");
            //else throw new Exception("Слово в словаре не найдено");
        }

        public void FindWord(string term)
        {
            foreach (Word word in Words)
            {
                if (word.Term == term)
                {
                    Console.WriteLine(word);
                    return;
                }
            }
            Console.WriteLine("Слово не найдено");
            //throw new Exception("Слово не найдено");
        }

        public bool IsWordIn(string find)
        {
            foreach (Word word in Words)
            {
                if (word.Term == find) return true;
            }
            return false;
        }

        public void ChangeWord(string term_old, string term_new, List<string> translations)
        {
            foreach (Word word in Words)
            {
                if (word.Term == term_old)
                {
                    word.ChangeWord(term_new, translations);
                    return;
                }
                else throw new Exception("Слово не найдено");
            }
        }

        public void ShowDictionary()
        {
            if (Words.Count == 0)
            {
                Console.WriteLine("Словарь пуст");
                return;
            }
            foreach (Word word in Words)
            {
                Console.WriteLine(word);
            }
        }

        public override string ToString()
        {
            return "\t\t\t" + Language + "\n" + string.Join("\n", Words);
        }
    }
}
