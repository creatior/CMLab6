using System;
using System.Linq;

namespace MachineSimulation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Machine machine = new Machine();
            //machine.Simulate(500);

            //double totalProcessingTime = machine.CompletedTasks.Sum(t => t.endTime - t.startTime);
            //double averageProcessingTime = totalProcessingTime / machine.CompletedTasks.Count;
            //double machineUtilization = totalProcessingTime / machine.CompletedTasks.Last().endTime;

            //Console.WriteLine($"Среднее время выполнения задания: {averageProcessingTime:F4} ч\nЗагрузка станка: {machineUtilization:P2}");

            GasStation gasStation = new GasStation();
            gasStation.Simulate(400);

            double avgQueueLength1 = gasStation.AverageQueueLength1();
            double avgQueueLength2 = gasStation.AverageQueueLength2();
            double lostCustomerPercentage = gasStation.LostCustomerPercentage();
            double avgDepartureInterval = gasStation.AverageDepartureInterval();
            double avgTimeInSystem = gasStation.AverageTimeInSystem();

            Console.WriteLine($"Среднее число клиентов в первой очереди: {avgQueueLength1:F2}\n" +
                            $"Среднее число клиентов во второй очереди: {avgQueueLength2:F2}\n" +
                            $"Процент клиентов, которые отказались от обслуживания: {lostCustomerPercentage:P2}\n" +
                            $"Интервалы времени между отъездами клиентов: {avgDepartureInterval:F2}\n" +
                            $"Среднее время пребывания клиента на заправке: {avgTimeInSystem:F2}");

            Console.ReadLine();
        }
    }
}
