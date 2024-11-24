using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSimulation
{
    public class GasStation
    {
        private Random random = new Random();
        private Queue<Task> queue1 = new Queue<Task>();
        private Queue<Task> queue2 = new Queue<Task>();
        private Task currentCustomer1;
        private Task currentCustomer2;
        private double currentTime = 0;
        private double nextCustomerArrivalTime;
        private int lostCustomers = 0;
        private List<double> departureIntervals = new List<double>();

        public List<Task> ServedCustomers { get; private set; } = new List<Task>();

        public GasStation()
        {
            nextCustomerArrivalTime = GenerateNextCustomerArrivalTime();
        }

        public void Simulate(int numberOfCustomers)
        {
            while (ServedCustomers.Count < numberOfCustomers)
            {
                if (currentTime >= nextCustomerArrivalTime)
                {
                    Task newCustomer = new Task { arrivalTime = nextCustomerArrivalTime };
                    if (queue1.Count < 5 || queue2.Count < 5)
                    {
                        if (queue1.Count <= queue2.Count)
                        {
                            queue1.Enqueue(newCustomer);
                        }
                        else
                        {
                            queue2.Enqueue(newCustomer);
                        }
                    }
                    else
                    {
                        lostCustomers++;
                    }
                    nextCustomerArrivalTime = GenerateNextCustomerArrivalTime();
                }

                if (currentCustomer1 == null && queue1.Count > 0)
                {
                    currentCustomer1 = queue1.Dequeue();
                    currentCustomer1.startTime = currentTime;
                    currentCustomer1.endTime = currentCustomer1.startTime + GenerateServiceTime();
                }

                if (currentCustomer2 == null && queue2.Count > 0)
                {
                    currentCustomer2 = queue2.Dequeue();
                    currentCustomer2.startTime = currentTime;
                    currentCustomer2.endTime = currentCustomer2.startTime + GenerateServiceTime();
                }

                if (currentCustomer1 != null && currentTime >= currentCustomer1.endTime)
                {
                    ServedCustomers.Add(currentCustomer1);
                    currentCustomer1 = null;
                    departureIntervals.Add(currentTime - (ServedCustomers.Count > 1 ? ServedCustomers[ServedCustomers.Count - 2].endTime : 0));
                }

                if (currentCustomer2 != null && currentTime >= currentCustomer2.endTime)
                {
                    ServedCustomers.Add(currentCustomer2);
                    currentCustomer2 = null;
                    departureIntervals.Add(currentTime - (ServedCustomers.Count > 1 ? ServedCustomers[ServedCustomers.Count - 2].endTime : 0));
                }

                currentTime += 0.01; // Increment time by a small step
            }
        }

        private double GenerateNextCustomerArrivalTime()
        {
            return currentTime + GenerateExponential(0.1);
        }

        private double GenerateServiceTime()
        {
            return GenerateExponential(0.5);
        }

        private double GenerateExponential(double lambda)
        {
            return -Math.Log(1 - random.NextDouble()) / lambda;
        }

        public double AverageQueueLength1()
        {
            return queue1.Count + (currentCustomer1 != null ? 1 : 0);
        }

        public double AverageQueueLength2()
        {
            return queue2.Count + (currentCustomer2 != null ? 1 : 0);
        }

        public double LostCustomerPercentage()
        {
            return (double)lostCustomers / (ServedCustomers.Count + lostCustomers);
        }

        public double AverageDepartureInterval()
        {
            return departureIntervals.Average();
        }

        public double AverageTimeInSystem()
        {
            return ServedCustomers.Average(c => c.endTime - c.arrivalTime);
        }
    }
}
