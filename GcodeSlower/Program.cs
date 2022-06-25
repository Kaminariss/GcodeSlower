using System;
using System.Collections.Generic;
using System.IO;

namespace GcodeSlower {
	internal static class Program {
		static private void Main(string[] args) {
			if (args.Length < 1) {
				Console.WriteLine("No args provided!");
				return;
			}

			var gcodeFileName = args[0];

			var lines = new List<string>(File.ReadAllLines(gcodeFileName));
			var startLineIndexes = new List<int>();

			for (var i = 0; i < lines.Count; i++) {
				var line = lines[i];
				if (line.StartsWith(";LAYER_CHANGE")) {
					startLineIndexes.Add(i);
				}
			}

			var lastFullSpeedLayer = 5;
			var minSpeed = 25;
			for (var i = lastFullSpeedLayer; i >= 0; i--) {
				var speed = (
					minSpeed + (i * (100 - minSpeed) / lastFullSpeedLayer)
				).ToString("D");

				var lineIdx = startLineIndexes[i];
				Console.WriteLine($"M220 S{speed} at line {lineIdx}");
				lines.Insert(lineIdx, $"M220 S{speed}");
			}

			Console.WriteLine("Layers " + startLineIndexes.Count);
			File.WriteAllLines(gcodeFileName, lines);
		}
	}
}