import './App.css'
import { useEffect, useState } from 'react'
import { TestsAdder } from './components/TestsAdder'
import { createNewTest, getTestFunctions, getOptimizationAlgorithms, getTestStatus, startTest, stopTest, serverAddress } from './lib/requests';
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

  const addTests = async (newTests: NewTestData[], state: string) => {
    const promises = newTests.map((t) => createNewTest(t, state));
    const t = await Promise.all(promises);
    setTests([...tests, ...t]);
  };

  const checkStatus = async (id: string) => {
    const newData = await getTestStatus(id);
    const testIndex = tests.findIndex(t => t.id === id)
    if (testIndex === undefined) return;
    tests[testIndex] = { ...tests[testIndex], ...newData };
    setTests([...tests]);
    if (tests[testIndex].status === TestStatus.Running || tests[testIndex].status === TestStatus.Pausing) {
      setTimeout(checkStatus, 1000);
    }
  }

  const runTest = async (id: string) => {
    await startTest(id);
    await checkStatus(id);
  }

  const runAllTests = () => {
    tests.forEach((t) => {
      if (![TestStatus.Running, TestStatus.Pausing, TestStatus.Finished].includes(t.status)) {
        runTest(t.id)
      }
    })
  }

  const testCards = tests.map(({ id, algorithmName, functionName, parameters, dimensions, status: status, currentIteration, iterations, fbest, xbest }) => {
    const algo = optimizationAlgorithms.find((a) => a.name === algorithmName)!;
    const parametersString = algo.paramInfo.map((param, index) => `${param.name}: ${parameters[index]}`).join(', ')
    
    const pauseTest = () => stopTest(id)

    return (
      <Card className="w-full" key={id}>
        <CardHeader>
          <CardTitle>{algorithmName} & {functionName} - {statusToString[status]}</CardTitle>
          <CardDescription>Dimensions: {dimensions}, Iterations: {iterations}, {parametersString}</CardDescription>
        </CardHeader>
        <CardContent>
          { fbest !== undefined && <div>Fbest: {fbest}</div> }
          { xbest !== undefined && <div>Xbest: {xbest.map((x) => x.toFixed(5)).join(", ")}</div> }
          <div className='relative w-full mb-[-19px] text-center z-10 text-blue-400 font-semibold'>
            {currentIteration}/{iterations}
          </div>
          <Progress value={(currentIteration * 100) / iterations} />
        </CardContent>
        <CardFooter className="flex justify-between">
          {/* <Button variant="destructive">Delete</Button> */}
          <a href={`${serverAddress}/api/Tests/state/${id}`} target='_blank'>
            <Button variant="outline">Download state</Button>
          </a>
          <a href={`${serverAddress}/api/Tests/pdf-report/${id}`} target='_blank'>
            <Button variant="outline">Download report</Button>
          </a>
          { status === TestStatus.Created || status === TestStatus.Paused ?
            <Button onClick={() => runTest(id)}>Run</Button> : status === TestStatus.Running ?
            <Button onClick={pauseTest}>Pause</Button> : null }
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

      <Button onClick={runAllTests}>Run all tests</Button>

      { testCards }
    </div>
  )
}