import './App.css'
import { useEffect, useState } from 'react'
import { TestsAdder } from './components/TestsAdder'
import { createNewTest, getTestFunctions, getOptimizationAlgorithms } from './lib/requests';
import { OptimizationAlgorithm } from './lib/OptimizationAlgorithm';
import { Loader2 } from 'lucide-react';
import { Button } from './components/ui/button';
import { NewTestData, statusToString, Test, TestStatus } from './lib/TestInterface';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "./components/ui/card"
import { Progress } from './components/ui/progress';
import { TestFunction } from './lib/TestFunction';

interface AppData {
  optimizationAlgorithms: OptimizationAlgorithm[];
  fitnessFunctions: TestFunction[];
}

export function App() {
  const [data, setData] = useState<AppData | null>(null);

  useEffect(() => {
    const getData = async () => {
      const [optimizationAlgorithms, fitnessFunctions] = await Promise.all([getOptimizationAlgorithms(), getTestFunctions()]);
      setData({optimizationAlgorithms, fitnessFunctions});
    }
    getData()
  }, []);

  const content = data ? 
    <LoadedApp {...data} /> :
    <Loader2 className="size-8 animate-spin text-blue-500" />
  
  return (
    <main className='w-full h-[100vh] flex flex-col items-center'>
      <h1 className="text-4xl font-extrabold tracking-tight my-8">
        Metaheuristics tester
      </h1>
      {content}
    </main>
  )
}

function LoadedApp({ fitnessFunctions, optimizationAlgorithms }: AppData) {
  const [tests, setTests] = useState<Test[]>([]);

  const addTests = async (newTests: NewTestData[]) => {
    const promises = newTests.map((t) => createNewTest(t));
    const t = await Promise.all(promises);
    setTests([...tests, ...t]);
  };

  const testCards = tests.map(({ id: Id, algorithmName: AlgorithmName, functionName: FunctionName, parameters: Parameters, dimensions: Dimensions, status: Status, currentIteration: CurrentIteration, iterations: Iterations }) => {
    const algo = optimizationAlgorithms.find((a) => a.name === AlgorithmName)!;
    const parameters = algo.paramInfo.map((param, index) => `${param.name}: ${Parameters[index]}`).join(', ')

    return (
      <Card className="w-full" key={Id}>
        <CardHeader>
          <CardTitle>{AlgorithmName} & {FunctionName} - {statusToString[Status]}</CardTitle>
          <CardDescription>Dimensions: {Dimensions}, Iterations: {Iterations} {parameters}</CardDescription>
        </CardHeader>
        <CardContent>
          <div className='relative w-full mb-[-19px] text-center z-10 text-blue-400 font-semibold'>
            {CurrentIteration}/{Iterations}
          </div>
          <Progress value={(CurrentIteration * 100) / Iterations} />
        </CardContent>
        <CardFooter className="flex justify-between">
          <Button variant="destructive">Delete</Button>
          <Button variant="outline">Download state</Button>
          <Button variant="outline">Download report</Button>
          { Status === TestStatus.Created || Status === TestStatus.Paused ?
            <Button>Run</Button> : Status === TestStatus.Running ?
            <Button>Pause</Button> : null }
        </CardFooter>
      </Card>
    )
  })

  return (
    <div className='w-[40vw] flex flex-col gap-4 flex-1'>
      <TestsAdder
        algorithms={optimizationAlgorithms}
        functions={fitnessFunctions}
        addTests={addTests}
      />

      <Button>Start all tests</Button>

      { testCards }
    </div>
  )
}