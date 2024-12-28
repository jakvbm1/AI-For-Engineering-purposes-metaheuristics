import { OptimizationAlgorithm } from "./OptimizationAlgorithm";

export async function getOptimizationAlgorithms(): Promise<OptimizationAlgorithm[]> {
  return [
    {
      name: "Pink Wolf Optimizer",
      paramsInfo: [
        {
          name: "α",
          description: "Shade of wolf's fur",
          lowerBoundary: -1.5,
          upperBoundary: 200,
          defaultValue: 10
        },
        {
          name: "β",
          description: "Wolf speed",
          lowerBoundary: 0,
          upperBoundary: 10,
          defaultValue: 5
        }
      ]
    },
    {
      name: "Grey Wolf Optimizer",
      paramsInfo: []
    },
  ]
}

export async function getFitnessFunctions(): Promise<string[]> {
  return [
    "RosenBrociek",
    "Rasputin"
  ]
}