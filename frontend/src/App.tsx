import './App.css'
import { useEffect, useState } from 'react'
import { AlgorithmsTuner } from './components/AlgorithmsTuner'
import { getFitnessFunctions, getOptimizationAlgorithms } from './lib/requests';
import { OptimizationAlgorithm } from './lib/OptimizationAlgorithm';
import { Loader2 } from 'lucide-react';
import { Label } from './components/ui/label';
import { Button } from './components/ui/button';
import { MultiSelect } from './components/MultiSelect';

interface AppData {
  optimizationAlgorithms: OptimizationAlgorithm[];
  fitnessFunctions: string[];
}

export function App() {
  const [data, setData] = useState<AppData | null>(null);

  useEffect(() => {
    const getData = async () => {
      const [optimizationAlgorithms, fitnessFunctions] = await Promise.all([getOptimizationAlgorithms(), getFitnessFunctions()]);
      setData({optimizationAlgorithms, fitnessFunctions});
    }
    getData()
  }, []);

  const content = data ? 
    <LoadedApp {...data} /> :
    <Loader2 className="size-8 animate-spin text-blue-500" />
  
  return (
    <main className='w-[100vw] h-[100vh] flex flex-col justify-center items-center'>
      <h1 className="text-4xl font-extrabold tracking-tight mb-8">
        Metaheuristics tester
      </h1>
      {content}
    </main>
  )
}

function LoadedApp({ fitnessFunctions, optimizationAlgorithms }: AppData) {
  const [algorithmsParameters, setAlgorithmsParameters] = useState(getAlgorithmsParameters(optimizationAlgorithms));
  const [selectedFitnessFunctions, setSelectedFitnessFunction] = useState<string[]>([]);
  const [selectedAlgorithm, setSelectedAlgorithm] = useState<string[]>([]);
  const [population, setPopulation] = useState(30);
  const [iterations, setIterations] = useState(50);
  const [dimensions, setDimensions] = useState(5);

  return (
    <div className='w-[40vw] flex flex-col gap-4'>
      <Label>
        <p className='mb-1'>Fitness functions</p>
        <MultiSelect
          placeholder='Select a fitness function...'
          options={fitnessFunctions}
          checkedOptions={selectedFitnessFunctions}
          onCheck={setSelectedFitnessFunction}
        />
      </Label>

      <Label>
        <p className='mb-1'>Optimization algorithm</p>
        <MultiSelect
          placeholder='Select an optimization algorithm...'
          options={optimizationAlgorithms.map((o) => o.name)}
          checkedOptions={selectedAlgorithm}
          onCheck={setSelectedAlgorithm}
        />
      </Label>

      <AlgorithmsTuner
        algorithms={optimizationAlgorithms}
        algorithmsParameters={algorithmsParameters}
        setAlgorithmsParameters={setAlgorithmsParameters}
        population={population}
        setPopulation={setPopulation}
        iterations={iterations}
        setIterations={setIterations}
        dimensions={dimensions}
        setDimensions={setDimensions}
      />

      <Button variant='outline'>Pick state file</Button>

      <Button>Start</Button>
    </div>
  )
}

function getAlgorithmsParameters(a: OptimizationAlgorithm[]): Record<string, Record<string, number>> {
  return a.reduce((acc, algo) => {
    const params = algo.paramsInfo.reduce((acc, param) => {
      return { ...acc, [param.name]: param.defaultValue };
    }, {});
    return {...acc, [algo.name]: params};
  }, {})
}