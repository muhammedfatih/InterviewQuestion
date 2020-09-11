using FastMember;
using InterviewQuestion.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace InterviewQuestion
{
    public class Program
    {
        static void Main()
        {
            // QueryElement Objects
            List<QueryElement> queryElements = QueryElementList.GetList();

            // QueryResultTable Objects
            List<QueryResultTable> queryResultTables = QueryResultTableList.GetList();

            // Start Timer
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ////////////////////////////////////
            /// Place your answer after this section
            ///////////////////////////////////

            var type = typeof(QueryResultTable);
            var accessor = TypeAccessor.Create(type);
            var properties = accessor.GetMembers();

            var propertyQueryElements = properties.Join(queryElements, property => property.Name, queryElement => $"column{queryElement.Index}", (property, queryElement) => new { property = property.Name, aggregate = queryElement.Aggregate }).ToList();

            queryResultTables.AsEnumerable().AsParallel().GroupBy(x => x.perfdate, (_, groups) =>
            {
                var first = groups.FirstOrDefault();
                foreach (var propertyQueryElement in propertyQueryElements)
                {
                    var aggregate = propertyQueryElement.aggregate;
                    var property = propertyQueryElement.property;
                    var sourceAsIntList = groups.Where(x => accessor[x, property]!=null && !string.IsNullOrWhiteSpace(accessor[x, property].ToString())).Select(x => Convert.ToInt32(accessor[x, property]));

                    if (aggregate == Enums.AggregateType.avg) accessor[first, property] = sourceAsIntList.Average().ToString();
                    if (aggregate == Enums.AggregateType.max) accessor[first, property] = sourceAsIntList.Max().ToString();
                    if (aggregate == Enums.AggregateType.min) accessor[first, property] = sourceAsIntList.Min().ToString();
                    if (aggregate == Enums.AggregateType.sum) accessor[first, property] = sourceAsIntList.Sum().ToString();
                }
                return first;
            }
            ).ToList();

            ////////////////////////////////////
            /// Place your answer before this section
            ///////////////////////////////////

            // Stop Timer
            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            ////////////////////////////
            // Data Validation
            ////////////////////////////
            
            if (queryResultTables[0].column1 != "800")
            {
                Console.WriteLine("ERROR IN CALCULATION for column1!");
            }

            if (queryResultTables[0].column2 != "160")
            {
                Console.WriteLine("ERROR IN CALCULATION for column2!");
            }

            if (queryResultTables[0].column3 != "180")
            {
                Console.WriteLine("ERROR IN CALCULATION for column3!");
            }

            if (queryResultTables[0].column4 != "320")
            {
                Console.WriteLine("ERROR IN CALCULATION for column4!");
            }

            if (queryResultTables[0].column5 != "780")
            {
                Console.WriteLine("ERROR IN CALCULATION for column5!");
            }

            if (queryResultTables[0].column6 != "15")
            {
                Console.WriteLine("ERROR IN CALCULATION for column6!");
            }

            Console.WriteLine("Execution duration: " + elapsedTime);

            Thread.Sleep(10000);
        }
    }
}
