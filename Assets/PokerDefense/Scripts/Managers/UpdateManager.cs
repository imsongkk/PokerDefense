using PokerDefense.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PokerDefense.Managers
{
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        private readonly List<IEnumerator> list;


        public UpdateManager() => list = new List<IEnumerator>();

        public static int Length { get => Instance.list.Count; }

        public static void Release() => Instance.list.Clear();

        public static void Add(IEnumerator iterator) => Instance.list.Add(iterator);

        public static void Insert(IEnumerator iterator, int index) => Instance.list.Insert(index, iterator);

        public static void Remove(IEnumerator iterator) => Instance.list.Remove(iterator);

        public static void RemoveAt(int index) => Instance.list.RemoveAt(index);

        private void Update()
        {
            if (list.Count == 0)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                bool isNext = list[i].MoveNext();
                if (isNext)
                {
                    if (list[i].Current is IEnumerator enumerator)
                        list.Add(enumerator);
                }
                else
                    list.RemoveAt(i--);
            }
        }
    }
}