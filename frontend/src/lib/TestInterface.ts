export interface Test {
  id: string;
  status: TestStatus;
  algorithmName: string;
  functionName: string;
  parameters: number[];
  dimensions: number;
  currentIteration: number;
  iterations: number;
}

export enum TestStatus {
  Created = 1,
  Running = 2,
  Pausing = 3,
  Paused = 4,
  Finished = 5
}

export type NewTestData = Omit<Test, "id" | "status" | "currentIteration">;

export const statusToString: { [status in TestStatus]: string } = {
  [TestStatus.Created]: "Created",
  [TestStatus.Running]: "Running",
  [TestStatus.Pausing]: "Pausing",
  [TestStatus.Paused]: "Paused",
  [TestStatus.Finished]: "Finished",
}