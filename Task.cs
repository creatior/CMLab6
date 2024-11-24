using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSimulation
{
    public class Task
    {
        // Время поступления задания
        public double arrivalTime { get; set; }
        // Время начала выполнения задания
        public double startTime { get; set; }
        // Время окончания выполнения задания
        public double endTime { get; set; }
    }
}
