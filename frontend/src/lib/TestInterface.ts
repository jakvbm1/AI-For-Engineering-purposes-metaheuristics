export interface Test {
  Id: string;
  Status: TestStatus;
  AlgorithmName: string;
  FunctionName: string;
  Parameters: number[];
  Dimensions: number;
  CurrentIteration: number;
  Iterations: number;
}

export enum TestStatus {
  Created = 1,
  Running = 2,
  Pausing = 3,
  Paused = 4,
  Finished = 5
}

export type NewTestData = Omit<Test, "Id" | "Status" | "CurrentIteration">;

export const statusToString: { [status in TestStatus]: string } = {
  [TestStatus.Created]: "Created",
  [TestStatus.Running]: "Running",
  [TestStatus.Pausing]: "Pausing",
  [TestStatus.Paused]: "Paused",
  [TestStatus.Finished]: "Finished",
}