using System;
using System.Collections.Generic;
using System.Text;

namespace CatRenta.Application.Interfaces
{
    public delegate void InsertCatDelegate(int i);  // delegate
    public interface ICatService
    {
        //викликається коли додали один елемент
        event InsertCatDelegate EventInsertItem; // event
        /// <summary>
        /// повертаємо кількість елеменів БД
        /// </summary>
        /// <returns>кількість елементів</returns>
        int Count();
        /// <summary>
        /// Додати можину елементів в БД
        /// </summary>
        /// <param name="count"></param>
        void InsertCats(int count);
    }
}
