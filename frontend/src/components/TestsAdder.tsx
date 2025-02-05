import { OptimizationAlgorithm, ParamInfo } from "@/lib/OptimizationAlgorithm";
import { Button } from "./ui/button";
import { Drawer, DrawerClose, DrawerContent, DrawerDescription, DrawerFooter, DrawerHeader, DrawerTitle, DrawerTrigger } from "./ui/drawer";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "./ui/accordion";
import { Input } from "./ui/input";
import { Label } from "./ui/label";
import { ScrollArea } from "./ui/scroll-area";
import { Plus, X } from "lucide-react";
import { useState } from "react";
import { Select, SelectContent, SelectGroup, SelectItem, SelectLabel, SelectTrigger, SelectValue } from "./ui/select";
import { TestFunction } from "@/lib/TestFunction";
import { NewTestData } from "@/lib/TestInterface";

interface Props {
  algorithms: OptimizationAlgorithm[];
  functions: TestFunction[];
  addTests: (newTests: NewTestData[]) => void;
}

const iterationsParamInfo: ParamInfo = {
  name: "Iterations",
  description: "The number of algorithm iterations",
  defaultValue: 10,
  lowerBoundary: 1,
  upperBoundary: Number.MAX_SAFE_INTEGER
}

export function TestsAdder({ algorithms, functions, addTests }: Props) {
  const [algorithm, setAlgorithm] = useState<OptimizationAlgorithm | undefined>(undefined);
  const [testFunction, setTestFunction] = useState<TestFunction | undefined>(undefined);
  const [dimensions, setDimensions] = useState<number[]>([]);
  const [iterations, setIterations] = useState<number[]>([]);
  const [params, setParams] = useState<number[][]>([]);

  const algoAndFuncPicked = !!algorithm && !!testFunction;
  const canAdd = algoAndFuncPicked && iterations.length > 0 && dimensions.length > 0 && params.every((p) => p.length > 0);
  const testsNumber = canAdd ? iterations.length * dimensions.length * params.reduce((acc, p) => acc * p.length, 1) : 0;

  const onAlgoChange = (v: string) => {
    const newAlgo = algorithms.find((a) => a.name === v);
    setAlgorithm(newAlgo);
    if (!newAlgo) return;
    setParams(newAlgo.paramInfo.map(() => []))
  }

  const onFunctionChange = (v: string) => {
    const newFunction = functions.find((f) => f.name === v);
    setTestFunction(newFunction);
    if (!newFunction) return;
    setDimensions(newFunction.isMultiDimensional ? [] : [newFunction.defaultDimensions])
  }

  const handleAdd = () => {
    const paramsCombinations: number[][] = [];

    function generateCombinations(arrays: number[][], prefix: number[] = [], index = 0) {
      if (index === arrays.length) {
        paramsCombinations.push(prefix)
        return;
      }
      
      for (const element of arrays[index]) {
        generateCombinations(arrays, [...prefix, element], index + 1);
      }
    }
  
    generateCombinations([dimensions, iterations, ...params]);
    const newTests: NewTestData[] = paramsCombinations.map((p) => {
      const [dimensions, iterations, ...params] = p;
      return {
        algorithmName: algorithm!.name,
        functionName: testFunction!.name,
        parameters: params,
        dimensions: dimensions,
        iterations: iterations,
      }
    });
    addTests(newTests);
  }
  
  return (
    <Drawer>
      <DrawerTrigger asChild>
        <Button variant='outline'>
          Add new tests
        </Button>
      </DrawerTrigger>
      
      <DrawerContent>
        <div className="mx-auto w-[50vw]">
          <DrawerHeader>
            <DrawerTitle>Add algorithms</DrawerTitle>
            <DrawerDescription>You can add algorithms here</DrawerDescription>
          </DrawerHeader>

          <ScrollArea className="h-[60vh] px-2">
            <div className="flex flex-col gap-4 mx-2">
              <Label>
                <p className='mb-1'>Algorithm</p>
                <Select value={algorithm?.name} onValueChange={onAlgoChange}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select an algorithm" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      <SelectLabel>Algorithms</SelectLabel>
                      { algorithms.map((a) => <SelectItem key={a.name} value={a.name}>{a.name}</SelectItem>) }
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </Label>

              <Label>
                <p className='mb-1'>Test function</p>
                <Select value={testFunction?.name} onValueChange={onFunctionChange}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select a test function" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectGroup>
                      <SelectLabel>Test functions</SelectLabel>
                      { functions.map((f) => <SelectItem key={f.name} value={f.name}>{f.name}</SelectItem>) }
                    </SelectGroup>
                  </SelectContent>
                </Select>
              </Label>

              { !algoAndFuncPicked ? "Pick an algorithm and a test function" :
                <Accordion type="single" collapsible>
                  <DimensionsPicker
                    dimensions={dimensions}
                    setDimensions={setDimensions}
                    testFunction={testFunction}
                  />
                  <ParamValuesPicker
                    paramInfo={iterationsParamInfo}
                    params={iterations}
                    setParams={(iter) => setIterations(iter.map((it) => Math.floor(it)))}
                  />
                  { algorithm.paramInfo.map((p, i) => {
                    return (
                      <ParamValuesPicker
                        key={i}
                        paramInfo={p}
                        params={params[i]}
                        setParams={(par) => {params[i] = par; setParams([...params])}}
                      />
                    )
                  }) }
                </Accordion>
              }
            </div>
          </ScrollArea>

          <DrawerFooter className="flex-row flex-1">
            <DrawerClose asChild>
              <Button variant="outline" className="flex-1">Cancel</Button>
            </DrawerClose>
            <DrawerClose asChild>
              <Button onClick={handleAdd} className="flex-1" disabled={!canAdd}>Add {testsNumber} tests</Button>
            </DrawerClose>
          </DrawerFooter>
        </div> 
      </DrawerContent>
    </Drawer>
  )
}

interface ParamValuesPickerProps {
  paramInfo: ParamInfo;
  params: number[];
  setParams: (params: number[]) => void;
}

function ParamValuesPicker({ paramInfo: { name, description, defaultValue, lowerBoundary, upperBoundary }, params, setParams }: ParamValuesPickerProps) {
  const [inputVal, setInputVal] = useState(defaultValue);

  const valueButtons = params.map((value, index) => {
    const handleClick = () => setParams(params.filter((_v, ind) => ind !== index));
    return (
      <Button className="m-2" key={Math.random().toString()} onClick={handleClick}>
        {value}<X />
      </Button>
    )
  })

  const addDimension = () => {
    if (inputVal >= lowerBoundary && inputVal <= upperBoundary) {
      setParams([...params, inputVal])
    }
  }

  return (
    <AccordionItem value={name}>
      <AccordionTrigger>{name}</AccordionTrigger>
      <AccordionContent>
        {description} <br />
        {name} can be a value between {lowerBoundary} and {upperBoundary}.
        <div className="w-full mt-4 p-4 rounded-md border">
          { valueButtons }
          <Button onClick={addDimension} className="m-2">
            <Input
              type="number"
              min={lowerBoundary}
              max={upperBoundary}
              value={inputVal}
              onChange={(e) => setInputVal(parseInt(e.target.value))}
              onClick={e => e.stopPropagation()}
              className="text-black h-6 w-16"
            />
            <Plus />
          </Button>
        </div>
      </AccordionContent>
    </AccordionItem>
  )
}

interface DimensionsPickerProps {
  testFunction: TestFunction;
  dimensions: number[];
  setDimensions: (d: number[]) => void;
}

function DimensionsPicker({ testFunction: { name: Name, defaultDimensions: DefaultDimensions, isMultiDimensional: IsMultiDimensional }, dimensions, setDimensions }: DimensionsPickerProps) {
  const [inputVal, setInputVal] = useState(DefaultDimensions);

  const valueButtons = dimensions.map((value, index) => {
    const handleClick = () => IsMultiDimensional && setDimensions(dimensions.filter((_v, ind) => ind !== index));
    return (
      <Button className="m-2" key={Math.random().toString()} onClick={handleClick}>
        { value }
        { IsMultiDimensional && <X /> }
      </Button>
    )
  })

  const addDimension = () => {
    if (inputVal > 0) {
      setDimensions([...dimensions, inputVal])
    }
  }

  return (
    <AccordionItem value="Dimensions">
      <AccordionTrigger>Dimensions</AccordionTrigger>
      <AccordionContent>
        { IsMultiDimensional ? 
          `The test function "${Name}" accepts any positive number of dimensions` :
          `The test function "${Name}" accepts only ${DefaultDimensions} as number of dimensions` }
        <div className="w-full mt-4 p-4 rounded-md border">
          { valueButtons }
          { IsMultiDimensional && 
            <Button onClick={addDimension} className="m-2">
              <Input
                type="number"
                min={1}
                value={inputVal}
                onChange={(e) => setInputVal(parseInt(e.target.value))}
                onClick={e => e.stopPropagation()}
                className="text-black h-6 w-16"
              />
              <Plus />
            </Button>
          }
        </div>
      </AccordionContent>
    </AccordionItem>
  )
}