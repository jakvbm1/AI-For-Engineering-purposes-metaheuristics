import { OptimizationAlgorithm, ParamInfo } from "@/lib/OptimizationAlgorithm";
import { Button } from "./ui/button";
import { Drawer, DrawerClose, DrawerContent, DrawerDescription, DrawerFooter, DrawerHeader, DrawerTitle, DrawerTrigger } from "./ui/drawer";
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from "./ui/accordion";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "./ui/table";
import { Input } from "./ui/input";
import { Label } from "./ui/label";
import { ScrollArea } from "./ui/scroll-area";

interface Props {
  algorithms: OptimizationAlgorithm[];
  algorithmsParameters: Record<string, Record<string, number>>;
  setAlgorithmsParameters: (o: Record<string, Record<string, number>>) => void;
  population: number;
  setPopulation: (n: number) => void;
  iterations: number;
  setIterations: (n: number) => void;
  dimensions: number
  setDimensions: (n: number) => void;
}

export function AlgorithmsTuner({ algorithms, algorithmsParameters, setAlgorithmsParameters, population, setPopulation, iterations, setIterations, dimensions, setDimensions }: Props) {
  const algorithmsSettings = algorithms.map((algo) => {
    const content = algo.paramsInfo.length === 0 ?
      "This algorithm has no parameters" :
      <ParamsTable
        paramsInfo={algo.paramsInfo}
        params={algorithmsParameters[algo.name]}
        setParams={(p) => setAlgorithmsParameters({...algorithmsParameters, [algo.name]: p})}
      />

    return (
      <AccordionItem key={algo.name} value={algo.name}>
        <AccordionTrigger>{algo.name}</AccordionTrigger>
        <AccordionContent>
          {content}      
        </AccordionContent>
      </AccordionItem>
    )
  });
  
  return (
    <Drawer>
      <DrawerTrigger asChild>
        <Button variant='outline'>Tune algorithms</Button>
      </DrawerTrigger>
      
      <DrawerContent>
        <div className="mx-auto w-[50vw]">
          <DrawerHeader>
            <DrawerTitle>Algorithms tuner</DrawerTitle>
            <DrawerDescription>You can tune algorithms' parameters here</DrawerDescription>
          </DrawerHeader>

          <ScrollArea className="h-[60vh] px-2">
            <div className="flex flex-col gap-4 mx-2">
              <Label>
                <p className='mb-1'>Population</p>
                <Input type="number" value={population} onChange={(e) => setPopulation(parseInt(e.target.value))} min={1} />
              </Label>

              <Label>
                <p className='mb-1'>Iterations</p>
                <Input type="number" value={iterations} onChange={(e) => setIterations(parseInt(e.target.value))} min={1} />
              </Label>

              <Label>
                <p className='mb-1'>Dimenstions</p>
                <Input type="number" value={dimensions} onChange={(e) => setDimensions(parseInt(e.target.value))} min={1} />
              </Label>

              <Accordion type="single" collapsible>
                {algorithmsSettings}
              </Accordion>
            </div>
          </ScrollArea>

          <DrawerFooter>
            <DrawerClose asChild>
              <Button>Close</Button>
            </DrawerClose>
          </DrawerFooter>
        </div> 
      </DrawerContent>
    </Drawer>
  )
}

interface ParamsTableProps {
  paramsInfo: ParamInfo[];
  params: Record<string, number>;
  setParams: (p: Record<string, number>) => void;
}

function ParamsTable({ paramsInfo, params, setParams }: ParamsTableProps) {
  const parametersSettings = paramsInfo.map((paramInfo) => {
    return (
      <TableRow key={paramInfo.name}>
        <TableCell className="font-medium">{paramInfo.name}</TableCell>
        <TableCell>{paramInfo.description}</TableCell>
        <TableCell>From {paramInfo.lowerBoundary} to {paramInfo.upperBoundary}</TableCell>
        <TableCell>
          <Input
            type="number"
            value={isNaN(params[paramInfo.name]) ? '' : params[paramInfo.name]}
            onChange={(e) => setParams({...params, [paramInfo.name]: parseFloat(e.target.value) })}
            min={paramInfo.lowerBoundary}
            max={paramInfo.upperBoundary}
          />
        </TableCell>
      </TableRow>
    )
  });

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Parameter name</TableHead>
          <TableHead>Parameter description</TableHead>
          <TableHead>Value range</TableHead>
          <TableHead>Value input</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {parametersSettings}
      </TableBody>
    </Table>
  )
}