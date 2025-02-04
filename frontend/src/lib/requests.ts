import { OptimizationAlgorithm } from "./OptimizationAlgorithm";
import { TestFunction } from "./TestFunction";
import { NewTestData, Test, TestStatus } from "./TestInterface";

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

export async function getTestFunctions(): Promise<TestFunction[]> {
  return [
    {
      Name: "RosenBrociek",
      DefaultDimensions: 2,
      IsMultiDimensional: true
    },
    {
      Name: "Rasputin",
      DefaultDimensions: 2,
      IsMultiDimensional: false
    }
  ]
}

export async function createNewTest(newTest: NewTestData): Promise<Test> {
  return {
    ...newTest,
    Id: Math.random().toString(),
    Status: TestStatus.Created,
    CurrentIteration: 0
  }
}