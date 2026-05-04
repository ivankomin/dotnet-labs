using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrologyInstitute
{
    public class Chassis
    {
        public string Material { get; set; }
        public int MaxDepth { get; set; }
    }

    public class SensorModule
    {
        public string Type { get; set; }
        public double PowerConsumption { get; set; }
    }

    public class PowerUnit
    {
        public string Name { get; set; }
        public double Capacity { get; set; }
    }

    public class OceanRobot
    {
        public string ModelName { get; set; }
        public string Status { get; set; } = "In Development";
        public Chassis Chassis { get; set; }
        public PowerUnit Power { get; set; }
        public List<SensorModule> Sensors { get; } = new List<SensorModule>();
        public List<string> Systems { get; } = new List<string>();

        public void PrintSpecification()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine($"STATUS: {Status}");
            Console.WriteLine($"MODEL:  {ModelName}");
            Console.WriteLine("====================================================");
            Console.WriteLine($"Chassis: {Chassis?.Material} (Rated Depth: {Chassis?.MaxDepth}m)");
            Console.WriteLine($"Power:   {Power?.Name} ({Power?.Capacity}W)");

            Console.WriteLine("\nSensor Modules:");
            if (!Sensors.Any())
                Console.WriteLine(" [ ] No sensors installed.");
            foreach (var s in Sensors)
                Console.WriteLine($" [+] {s.Type} (Power: {s.PowerConsumption}W)");

            Console.WriteLine("\nNavigation & Control Systems:");
            foreach (var sys in Systems)
                Console.WriteLine($" [+] {sys}");

            Console.WriteLine("====================================================\n");
        }
    }

    public interface IRobotBuilder
    {
        void Reset(string modelName);
        void SetChassis(int depth);
        void SetPowerUnit(string type, double capacity);
        void AddSensor(string type, double consumption);
        void AddNavigation();
        void AddSonar();
        void AddSamplingSystem();
        OceanRobot Build();
    }

    public class OceanRobotBuilder : IRobotBuilder
    {
        private OceanRobot _robot;

        public void Reset(string modelName)
        {
            _robot = new OceanRobot { ModelName = modelName };
            Console.WriteLine($"[System]: Started designing platform: {modelName}");
        }

        public void SetChassis(int depth)
        {
            string mat = depth > 5000 ? "Titanium-Alloy" : "Reinforced Composite";
            _robot.Chassis = new Chassis { MaxDepth = depth, Material = mat };
            Console.WriteLine($" -> Chassis set for {depth}m depth ({mat}).");
        }

        public void SetPowerUnit(string type, double capacity)
        {
            _robot.Power = new PowerUnit { Name = type, Capacity = capacity };
            Console.WriteLine($" -> Power unit connected: {type} ({capacity}W).");
        }

        public void AddSensor(string type, double consumption)
        {
            _robot.Sensors.Add(new SensorModule { Type = type, PowerConsumption = consumption });
            Console.WriteLine($" -> Sensor module added: {type}.");
        }

        public void AddNavigation() => _robot.Systems.Add("Acoustic Navigation System");

        public void AddSonar() => _robot.Systems.Add("Side-Scan Sonar");

        public void AddSamplingSystem() => _robot.Systems.Add("Physical Sampling Channel");

        public OceanRobot Build()
        {
            Validate();
            _robot.Status = "SUCCESSFULLY CONSTRUCTED & VERIFIED";
            Console.WriteLine(
                $"\n[Success]: Platform '{_robot.ModelName}' passed all compatibility tests."
            );
            return _robot;
        }

        // AUTOMATIC COMPATIBILITY CHECKS
        private void Validate()
        {
            // 1. Mandatory Components
            if (_robot.Chassis == null || _robot.Power == null)
                throw new Exception("Incomplete configuration: Missing Chassis or Power Unit!");

            // 2. Energy Balance
            double totalPower = _robot.Sensors.Sum(s => s.PowerConsumption);
            totalPower += _robot.Systems.Count * 10; // Overhead for internal systems

            if (totalPower > _robot.Power.Capacity)
                throw new Exception(
                    $"Energy Overload! Consumption ({totalPower}W) > Capacity ({_robot.Power.Capacity}W)."
                );

            // 3. Depth-Sensor Compatibility
            if (
                _robot.Chassis.MaxDepth > 7000
                && _robot.Sensors.Any(s => s.Type == "Standard Camera")
            )
                throw new Exception(
                    "Equipment Conflict: Standard cameras cannot withstand ultra-abyssal depths (>7000m)!"
                );

            // 4. Regional Logic (Example: Seismic Zones)
            if (
                _robot.ModelName.ToLower().Contains("seismic")
                && !_robot.Sensors.Any(s => s.Type.Contains("Pressure"))
            )
                throw new Exception(
                    "Regional Incompatibility: Seismic zone robots must have a Pressure Sensor for data accuracy!"
                );

            // 5. Dependency Check
            bool hasBioScan = _robot.Sensors.Any(s => s.Type.Contains("Bio"));
            bool hasSampling = _robot.Systems.Any(sys => sys.Contains("Sampling"));
            if (hasBioScan && !hasSampling)
                Console.WriteLine(
                    "[Warning]: Bio-scanning without a sampling channel reduces mission efficiency."
                );

            // 6. Safety Minimums
            if (!_robot.Systems.Any(sys => sys.Contains("Navigation")))
                throw new Exception(
                    "Safety Error: Autonomous platforms require a navigation system to operate!"
                );
        }
    }

    // --- 5. DIRECTOR ---

    public class RobotDirector
    {
        public void ConstructMarianaExplorer(IRobotBuilder builder)
        {
            builder.Reset("Mariana-X Deep-Seismic");
            builder.SetChassis(11000);
            builder.SetPowerUnit("Radioisotope Generator", 800);
            builder.AddSensor("Pressure Sensor", 50);
            builder.AddSensor("Chemical Composition", 120);
            builder.AddNavigation();
            builder.AddSonar();
        }

        public void ConstructCoastalScanner(IRobotBuilder builder)
        {
            builder.Reset("Coastal-Scan v2");
            builder.SetChassis(200);
            builder.SetPowerUnit("Li-ion Battery Pack", 150);
            builder.AddSensor("Bio-Scanner", 40);
            builder.AddNavigation();
            builder.AddSamplingSystem();
        }
    }

    // --- 6. CLIENT CODE ---

    class Program
    {
        static void Main()
        {
            var builder = new OceanRobotBuilder();
            var director = new RobotDirector();

            // Platform 1: Mariana Explorer (Valid)
            try
            {
                director.ConstructMarianaExplorer(builder);
                OceanRobot deepRobot = builder.Build();
                deepRobot.PrintSpecification();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR]: {ex.Message}");
            }

            // Platform 2: (Invalid) Overpowered Custom Build
            try
            {
                Console.WriteLine("Attempting to build an overpowered custom platform...");
                builder.Reset("Experimental-Fail-Unit");
                builder.SetChassis(1000);
                builder.SetPowerUnit("Small Battery", 20);
                builder.AddSensor("Heavy LiDAR", 150);
                builder.AddNavigation();

                builder.Build();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[CONSTRUCTION REJECTED]: {ex.Message}");
                Console.ResetColor();
            }

            // Platform 3: (Invalid) Seismic Robot without Pressure Sensor
            try
            {
                Console.WriteLine(
                    "\nAttempting to build a Seismic robot without Pressure sensors..."
                );
                builder.Reset("Seismic-Seeker-Alpha");
                builder.SetChassis(5000);
                builder.SetPowerUnit("Fuel Cell", 500);
                builder.AddSensor("Bio-Scanner", 30);
                builder.AddNavigation();

                builder.Build();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[CONSTRUCTION REJECTED]: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
