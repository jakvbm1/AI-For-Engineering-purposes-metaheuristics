import { OptimizationAlgorithm } from "./OptimizationAlgorithm";
import { TestFunction } from "./TestFunction";
import { NewTestData, Test, TestStatus } from "./TestInterface";

const serverAddress = "http://127.0.0.1:5032"

export async function getOptimizationAlgorithms(): Promise<OptimizationAlgorithm[]> {
  const resp = await fetch(`${serverAddress}/api/Tests/algorithms`, { headers: { "Content-Type": "application/json" } })
  return await resp.json();
}

export async function getTestFunctions(): Promise<TestFunction[]> {
  const resp = await fetch(`${serverAddress}/api/Tests/functions`, { headers: { "Content-Type": "application/json" } })
  return await resp.json();
}

export async function createNewTest(newTest: NewTestData): Promise<Test> {
  const options = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(newTest)
  }
  const resp = await fetch(`${serverAddress}/api/Tests/create`, options)
  const { id } = await resp.json();

  return {
    ...newTest,
    id: id,
    status: TestStatus.Created,
    currentIteration: 0
  }
}