using CatRenta.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatRenta.EFData
{
    public class DataSeed
    {
        public static void SeedDataAsync(EFDataContext context)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (!context.Cats.Any())
            {
                var cats = new List<AppCat>
                                {
                                    new AppCat
                                        {
                                            Name = "Барсік",
                                            Gender=true,
                                            Birthday=new DateTime(2018, 5,23),
                                            Details="Стан критичний",
                                            Image="https://ichef.bbci.co.uk/news/800/cpsprodpb/14236/production/_104368428_gettyimages-543560762.jpg"
                                        },

                                };

                foreach (var cat in cats)
                {
                    context.Add(cat);
                    context.SaveChanges();
                }
            }

            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Debug.WriteLine("Сідер 1 закінчив свою роботу: " + elapsedTime);

            stopWatch = new Stopwatch();
            stopWatch.Start();

            if (!context.CatPrices.Any())
            {
                var cat = context.Cats.FirstOrDefault();
                var catPrices = new List<AppCatPrice>
                                {
                                    new AppCatPrice
                                        {
                                            CatId=cat.Id,
                                            DateCreate=DateTime.Now,
                                            Price=450.23M
                                        },
                                    new AppCatPrice
                                        {
                                            CatId=cat.Id,
                                            DateCreate=new DateTime(2021,1,14),
                                            Price=250.75M
                                        }
                                };

                foreach (var price in catPrices)
                {
                    context.Add(price);
                    context.SaveChanges();
                }
            }

            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Debug.WriteLine("Сідер 2 закінчив свою роботу: " + elapsedTime);
        }
    }
}
