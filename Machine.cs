using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSimulation
{
    // Класс, моделирующий работу станка с поломками
    public class Machine
    {
        private Random random = new Random();
        private Queue<Task> taskQueue = new Queue<Task>();
        private Task currentTask;
        private double currentTime = 0;
        private double nextBreakdownTime;
        private double nextTaskArrivalTime;

        public List<Task> CompletedTasks { get; private set; } = new List<Task>();

        public Machine() {
            this.nextBreakdownTime = GenerateNextBreakdownTime();
            this.nextTaskArrivalTime = GenerateNextTaskArrivalTime();
        }

        public void Simulate(int numberOfTasks)
        {
            while (CompletedTasks.Count < numberOfTasks)
            {
                if (currentTime >= nextTaskArrivalTime)
                {
                    Task newTask = new Task { arrivalTime = nextTaskArrivalTime };
                    taskQueue.Enqueue(newTask);
                    nextTaskArrivalTime = GenerateNextTaskArrivalTime();
                }

                if (currentTask == null && taskQueue.Count > 0)
                {
                    currentTask = taskQueue.Dequeue();
                    currentTask.startTime = currentTime + GenerateSetupTime();
                }

                if (currentTask != null && currentTime >= currentTask.startTime)
                {
                    double processingTime = GenerateProcessingTime();
                    currentTask.endTime = currentTask.startTime + processingTime;

                    if (currentTime >= nextBreakdownTime)
                    {
                        HandleBreakdown();
                    }
                    else if (currentTime >= currentTask.endTime)
                    {
                        CompletedTasks.Add(currentTask);
                        currentTask = null;
                    }
                }

                currentTime += 0.01; // увеличение времени
            }
        }

        private double GenerateNextTaskArrivalTime()
        {
            return currentTime + GenerateExponential(1.0);
        }

        private double GenerateSetupTime()
        {
            return random.NextDouble() * (0.5 - 0.2) + 0.2;
        }

        private double GenerateProcessingTime()
        {
            return GenerateNormal(0.5, 0.1);
        }

        private double GenerateNextBreakdownTime()
        {
            return this.currentTime + GenerateNormal(20, 2);
        }

        private void HandleBreakdown()
        {
            double repairTime = random.NextDouble() * (0.5 - 0.1) + 0.1;
            currentTime += repairTime;
            nextBreakdownTime = GenerateNextBreakdownTime();

            if (currentTask != null)
            {
                taskQueue.Enqueue(currentTask);
                currentTask = null;
            }
        }

        private double GenerateExponential(double lambda)
        {
            return -Math.Log(1 - random.NextDouble()) / lambda;
        }

        private double GenerateNormal(double mean, double stdDev)
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }
    }
}
